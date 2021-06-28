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
using tdb.account.common.Const;
using tdb.account.webapi.AuthPolicys;
using tdb.consul.services;
using tdb.framework.webapi.APILog;
using tdb.framework.webapi.Auth;
using tdb.framework.webapi.Cache;
using tdb.framework.webapi.Exceptions;
using tdb.framework.webapi.IocAutofac.CacheAOP;
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

            //�Ƿ�������consul����
            if (AccConfig.Consul.HadConsulConfig() == false)
            {
                Console.WriteLine("��⵽δ����consul���ã�����ʱģʽ����������consul���ú��롾������������");
                Console.ReadKey();
                return;
            }

            //����
            services.AddTdbRedisCache(AccConfig.Consul.Redis.ConnectString.ToArray());

            //��־
            services.AddTdbMySqlNLogger(AccConfig.Consul.DBLogConnStr, AccConfig.App.Consul.ServiceCode, AccConfig.App.ApiUrl);

            //������֤
            services.AddTdbParamValidate();

            //��֤
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = TdbClaimTypes.Name,
                    RoleClaimType = TdbClaimTypes.Role,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AccConfig.Consul.Token.SecretKey)),
                        //�Ƿ���Audience
                        ValidateAudience = true,
                    ValidAudience = AccConfig.Consul.Token.Audience,
                        //�Ƿ���Issuer
                        ValidateIssuer = true,
                    ValidIssuer = AccConfig.Consul.Token.Issuer,
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

            //��Ȩ
            services.AddAuthorization(options =>
            {
                //��Ҫ�û�����Ȩ��
                options.AddPolicy(
                    CstPolicy.NeedAuthorityManageUser,
                    policy => policy.Requirements.Add(new AuthorityRequirement(CstAuthority.ManageUser)));
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

            //httpreports
            services.AddHttpReports(o =>
                    {
                        o.Switch = AccConfig.Consul.HttpReports.Switch;
                        o.RequestFilter = AccConfig.Consul.HttpReports.RequestFilter.ToArray();
                        o.WithRequest = AccConfig.Consul.HttpReports.WithRequest;
                        o.WithResponse = AccConfig.Consul.HttpReports.WithResponse;
                        o.WithCookie = AccConfig.Consul.HttpReports.WithCookie;
                        o.WithHeader = AccConfig.Consul.HttpReports.WithHeader;
                    })
                    .AddHttpTransport(o =>
                    {
                        o.CollectorAddress = new Uri(AccConfig.Consul.HttpReports.Transport.CollectorAddress);
                        o.DeferSecond = AccConfig.Consul.HttpReports.Transport.DeferSecond;
                        o.DeferThreshold = AccConfig.Consul.HttpReports.Transport.DeferThreshold;
                    });

            services.AddControllers(option =>
            {
                //�쳣����
                option.AddTdbGlobalException();

                //�ӿڵ�����־
                option.Filters.Add(typeof(APILogActionFilterAttribute));
            })
            .AddJsonOptions(options =>
            {
                //json�ֶ���ԭ�������null�����ı��Сд��JsonNamingPolicy.CamelCase=�շ巨��
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="appLifetime"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime appLifetime)
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

            //httpreports
            app.UseHttpReports();

            //���ط���URI
            var localUri = new Uri(AccConfig.App.ApiUrl);

            //ע��consul
            ConsulServicesHelper.RegisterToConsul(
                AccConfig.App.Consul.IP,
                AccConfig.App.Consul.Port,
                localUri.Host,
                localUri.Port,
                AccConfig.App.Consul.ServiceCode,
                $"{AccConfig.App.ApiUrl}/tdbaccount/Sys/HealthCheck",//"http://10.1.49.45:5000/api/Consul/HealthCheck",
                TimeSpan.FromMinutes(10),
                TimeSpan.FromMinutes(1),
                TimeSpan.FromSeconds(30),
                appLifetime);

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

            // ����ע��
            builder.Register(c => new TdbCacheInterceptor()).SingleInstance();
        }
    }
}
