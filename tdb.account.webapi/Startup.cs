using Autofac;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SqlSugar.IOC;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tdb.account.common.Config;
using tdb.framework.webapi.Cache;
using tdb.framework.webapi.Exceptions;
using tdb.framework.webapi.Log;
using tdb.framework.webapi.Swagger;
using tdb.framework.webapi.Validation;

namespace tdb.account.webapi
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// ����
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //����
            services.AddTdbConfig();

            //����
            services.AddTdbRedisCache(AccConfig.Consul.Redis.ConnectString.ToArray());

            //��־
            services.AddTdbMySqlNLogger(AccConfig.Consul.DBLogConnStr, $"{AccConfig.App.Consul.ServiceCode}_{AccConfig.App.ApiUrl}");

            //������֤
            services.AddTdbParamValidate();

            services.AddControllers(option =>
            {
                //�쳣����
                option.AddTdbGlobalException();
            });

            //��֤��Ȩ
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AccConfig.Consul.Token.SecretKey)),
                    //����Audience
                    ValidateAudience = false,
                    //����Issuer
                    ValidateIssuer = false,
                    //����ķ�����ʱ��ƫ����
                    ClockSkew = TimeSpan.Zero,
                };
                o.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = context =>
                    {
                        Logger.Ins.Fatal(context.Exception, "��֤��Ȩ�쳣");
                        return Task.CompletedTask;
                    },
                    OnForbidden = context =>
                    {
                        context.Response.Clear();
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = 403;
                        context.Response.WriteAsync("Ȩ�޲���");
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.Clear();
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = 401;
                        context.Response.WriteAsync("��֤δͨ��");
                        return Task.CompletedTask;
                    }
                };
            });

            //SqlSugar.IOC
            services.AddSqlSugar(new IocConfig()
            {
                ConnectionString = AccConfig.Consul.DBConnStr,
                DbType = IocDbType.MySql,
                IsAutoCloseConnection = true    //�����Զ��ͷ�ģʽ
            });
            //SqlSugar AOP��־
            services.ConfigurationSugar(db =>
            {
                if (Logger.Ins.IsTraceEnabled)
                {
                    db.Aop.OnLogExecuting = (sql, pars) =>
                    {
                        Logger.Ins.Trace($"ִ��SQL��{sql}{Environment.NewLine}{db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value))}");
                    };
                }
            });

            //swagger
            services.AddTdbSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v1",
                    Title = "�û�����ӿ��ĵ�",
                    Description = "�û�����ӿ��ĵ�"
                });

                //�ӿ�ע��
                var xmlAPI = Path.Combine(AppContext.BaseDirectory, "tdb.account.webapi.xml");
                c.IncludeXmlComments(xmlAPI, true);

                //����ע��
                var xmlDTO = Path.Combine(AppContext.BaseDirectory, "tdb.account.dto.xml");
                c.IncludeXmlComments(xmlDTO, true);

                //���Authorization
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "bearer",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new List<string>()
                    }
                });
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            //��֤
            app.UseAuthentication();
            //��Ȩ
            app.UseAuthorization();

            //swagger
            app.UseTdbSwagger();
            app.UseTdbSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs");
                c.DocExpansion(DocExpansion.None);
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// autofac����ע�����
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //��ģ�����ע��    
            builder.RegisterModule<AutofacModuleRegister>();
        }
    }
}
