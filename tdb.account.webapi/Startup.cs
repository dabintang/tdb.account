using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using tdb.account.common.Config;
using tdb.account.ibll;
using tdb.framework.webapi.Auth;
using tdb.framework.webapi.Cache;
using tdb.framework.webapi.Exceptions;
using tdb.framework.webapi.Log;
using tdb.framework.webapi.Swagger;
using tdb.framework.webapi.Validation;

namespace tdb.account.webapi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //配置
            services.AddTdbConfig();

            //缓存
            services.AddTdbRedisCache(AccConfig.Consul.Redis.ConnectString.ToArray());

            //日志
            services.AddTdbMySqlNLogger(AccConfig.Consul.DBLogConnStr, $"{AccConfig.App.Consul.ServiceCode}_{AccConfig.App.ApiUrl}");

            //JWT
            services.AddTdbAuth();

            //参数验证
            services.AddTdbParamValidate();

            services.AddControllers(option => {
                //异常处理
                option.AddTdbGlobalException();
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

                //生成token输入框
                c.OperationFilter<SwaggerTokenFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

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

        public void ConfigureContainer(ContainerBuilder builder)
        {
            //新模块组件注册    
            builder.RegisterModule<AutofacModuleRegister>();

            ////所有要实现依赖注入的借口都要继承该接口
            //var baseType = typeof(common.IAutofacDependency);

            ////获取需要注册的程序集名称集合
            //var lstAssemblyName = this.GetRegisterAssemblyNames();

            //foreach (var assemblyName in lstAssemblyName)
            //{
            //    //注册程序集中的对象
            //    builder.RegisterAssemblyTypes(GetAssemblyByName(assemblyName)).Where(m => baseType.IsAssignableFrom(m) && m != baseType)
            //           .AsImplementedInterfaces()//表示注册的类型，以接口的方式注册
            //                                     //.EnableInterfaceInterceptors()//引用Autofac.Extras.DynamicProxy,使用接口的拦截器，在使用特性 [Attribute] 注册时，注册拦截器可注册到接口(Interface)上或其实现类(Implement)上。使用注册到接口上方式，所有的实现类都能应用到拦截器。
            //           .InstancePerLifetimeScope();//同一个Lifetime生成的对象是同一个实例
            //}
        }

        /// <summary>
        /// 根据程序集名称获取程序集
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <returns></returns>
        private System.Reflection.Assembly GetAssemblyByName(string assemblyName)
        {
            return System.Reflection.Assembly.Load(assemblyName);
        }

        /// <summary>
        /// 获取需要注册的程序集名称集合
        /// </summary>
        /// <returns></returns>
        protected virtual List<string> GetRegisterAssemblyNames()
        {
            return new List<string>() { "tdb.account.dal", "tdb.account.bll" };
        }
    }
}
