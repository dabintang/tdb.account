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
            //����
            services.AddTdbConfig();

            //����
            services.AddTdbRedisCache(AccConfig.Consul.Redis.ConnectString.ToArray());

            //��־
            services.AddTdbMySqlNLogger(AccConfig.Consul.DBLogConnStr, $"{AccConfig.App.Consul.ServiceCode}_{AccConfig.App.ApiUrl}");

            //JWT
            services.AddTdbAuth();

            //������֤
            services.AddTdbParamValidate();

            services.AddControllers(option => {
                //�쳣����
                option.AddTdbGlobalException();
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

                //����token�����
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
            //��ģ�����ע��    
            builder.RegisterModule<AutofacModuleRegister>();

            ////����Ҫʵ������ע��Ľ�ڶ�Ҫ�̳иýӿ�
            //var baseType = typeof(common.IAutofacDependency);

            ////��ȡ��Ҫע��ĳ������Ƽ���
            //var lstAssemblyName = this.GetRegisterAssemblyNames();

            //foreach (var assemblyName in lstAssemblyName)
            //{
            //    //ע������еĶ���
            //    builder.RegisterAssemblyTypes(GetAssemblyByName(assemblyName)).Where(m => baseType.IsAssignableFrom(m) && m != baseType)
            //           .AsImplementedInterfaces()//��ʾע������ͣ��Խӿڵķ�ʽע��
            //                                     //.EnableInterfaceInterceptors()//����Autofac.Extras.DynamicProxy,ʹ�ýӿڵ�����������ʹ������ [Attribute] ע��ʱ��ע����������ע�ᵽ�ӿ�(Interface)�ϻ���ʵ����(Implement)�ϡ�ʹ��ע�ᵽ�ӿ��Ϸ�ʽ�����е�ʵ���඼��Ӧ�õ���������
            //           .InstancePerLifetimeScope();//ͬһ��Lifetime���ɵĶ�����ͬһ��ʵ��
            //}
        }

        /// <summary>
        /// ���ݳ������ƻ�ȡ����
        /// </summary>
        /// <param name="assemblyName">��������</param>
        /// <returns></returns>
        private System.Reflection.Assembly GetAssemblyByName(string assemblyName)
        {
            return System.Reflection.Assembly.Load(assemblyName);
        }

        /// <summary>
        /// ��ȡ��Ҫע��ĳ������Ƽ���
        /// </summary>
        /// <returns></returns>
        protected virtual List<string> GetRegisterAssemblyNames()
        {
            return new List<string>() { "tdb.account.dal", "tdb.account.bll" };
        }
    }
}
