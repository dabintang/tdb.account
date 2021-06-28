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
        /// 配置
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //配置
            services.AddTdbConfig();

            //是否已设置consul配置
            if (AccConfig.Consul.HadConsulConfig() == false)
            {
                Console.WriteLine("检测到未设置consul配置，以临时模式启动！设置consul配置后请【重新启动】！");
                Console.ReadKey();
                return;
            }

            //缓存
            services.AddTdbRedisCache(AccConfig.Consul.Redis.ConnectString.ToArray());

            //日志
            services.AddTdbMySqlNLogger(AccConfig.Consul.DBLogConnStr, AccConfig.App.Consul.ServiceCode, AccConfig.App.ApiUrl);

            //参数验证
            services.AddTdbParamValidate();

            //认证
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
                        //是否验Audience
                        ValidateAudience = true,
                    ValidAudience = AccConfig.Consul.Token.Audience,
                        //是否验Issuer
                        ValidateIssuer = true,
                    ValidIssuer = AccConfig.Consul.Token.Issuer,
                        //允许的服务器时间偏移量
                        ClockSkew = TimeSpan.Zero,
                };
                o.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = context =>
                    {
                        Logger.Ins.Fatal(context.Exception, "认证授权异常");
                        return Task.CompletedTask;
                    },
                    OnForbidden = context =>
                    {
                        context.Response.Clear();
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = 403;
                        context.Response.WriteAsync("权限不足");
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.Clear();
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = 401;
                        context.Response.WriteAsync("认证未通过");
                        return Task.CompletedTask;
                    }
                };
            });

            //授权
            services.AddAuthorization(options =>
            {
                //需要用户管理权限
                options.AddPolicy(
                    CstPolicy.NeedAuthorityManageUser,
                    policy => policy.Requirements.Add(new AuthorityRequirement(CstAuthority.ManageUser)));
            });

            //SqlSugar.IOC
            services.AddSqlSugar(new IocConfig()
            {
                ConnectionString = AccConfig.Consul.DBConnStr,
                DbType = IocDbType.MySql,
                IsAutoCloseConnection = true    //开启自动释放模式
            });
            //SqlSugar AOP日志
            services.ConfigurationSugar(db =>
            {
                if (Logger.Ins.IsTraceEnabled)
                {
                    db.Aop.OnLogExecuting = (sql, pars) =>
                    {
                        Logger.Ins.Trace($"执行SQL：{sql}{Environment.NewLine}{db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value))}");
                    };
                }
            });

            //swagger
            services.AddTdbSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v1",
                    Title = "用户服务接口文档",
                    Description = "用户服务接口文档"
                });

                //接口注释
                var xmlAPI = Path.Combine(AppContext.BaseDirectory, "tdb.account.webapi.xml");
                c.IncludeXmlComments(xmlAPI, true);

                //参数注释
                var xmlDTO = Path.Combine(AppContext.BaseDirectory, "tdb.account.dto.xml");
                c.IncludeXmlComments(xmlDTO, true);

                //添加Authorization
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
                //异常处理
                option.AddTdbGlobalException();

                //接口调用日志
                option.Filters.Add(typeof(APILogActionFilterAttribute));
            })
            .AddJsonOptions(options =>
            {
                //json字段名原样输出（null：不改变大小写；JsonNamingPolicy.CamelCase=驼峰法）
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

            //认证
            app.UseAuthentication();

            //授权
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

            //本地服务URI
            var localUri = new Uri(AccConfig.App.ApiUrl);

            //注册consul
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
        /// autofac容器注册服务
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //新模块组件注册    
            builder.RegisterModule<AutofacModuleRegister>();

            // 类型注入
            builder.Register(c => new TdbCacheInterceptor()).SingleInstance();
        }
    }
}
