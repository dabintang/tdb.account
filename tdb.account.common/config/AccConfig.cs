using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using tdb.common;
using tdb.framework.webapi.Config;

namespace tdb.account.common.Config
{
    /// <summary>
    /// 配置信息
    /// </summary>
    public static class AccConfig
    {
        /// <summary>
        /// 添加配置服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddTdbConfig(this IServiceCollection services)
        {
            //appsettings.json配置
            services.AddTdbJsonConfig();
            //读取appsettings.json配置
            ReadAppConfig();
            //配置有改动时重新读取
            LocalConfigurator.Ins.ConfigReload += RefreshAppConfig;

            //consul配置
            services.AddTdbConsulConfig(App.Consul.IP, App.Consul.Port, App.Consul.ServiceCode + "_");
            //读取consul配置
            ReadConsulConfig();
            //每分钟自动刷新consul配置信息
            RefreshConsulConfigOnTask();

            //消息配置
            var msgFullFileName = CommHelper.GetFullFileName("message.json");
            var msgJsonStr = File.ReadAllText(msgFullFileName);
            Msg = JsonConvert.DeserializeObject<MsgConfig>(msgJsonStr);
        }

        /// <summary>
        /// 读取appsettings.json配置
        /// </summary>
        private static void ReadAppConfig()
        {
            App = LocalConfigurator.Ins.GetConfig<AppConfig>();
        }

        /// <summary>
        /// appsettings.json配置有变动时更新
        /// </summary>
        private static void RefreshAppConfig(IConfigurationRoot config)
        {
            //读取appsettings.json配置
            ReadAppConfig();
        }

        /// <summary>
        /// 读取consul配置
        /// </summary>
        private static void ReadConsulConfig()
        {
            //consul配置
            Consul = DistributedConfigurator.Ins.GetConfig<ConsulConfig>();
        }

        /// <summary>
        /// 每分钟自动刷新consul配置信息
        /// </summary>
        private static void RefreshConsulConfigOnTask()
        {
            Task.Factory.StartNew(async () =>
            {
                while(true)
                {
                    //等待一分钟
                    await Task.Delay(1000*60);

                    //读取consul配置
                    ReadConsulConfig();
                }
            });
        }

        /// <summary>
        /// appsettings.json配置
        /// </summary>
        public static AppConfig App { get; private set; }

        /// <summary>
        /// consul配置
        /// </summary>
        public static ConsulConfig Consul { get; private set; }

        /// <summary>
        /// 回报消息配置
        /// </summary>
        public static MsgConfig Msg { get; private set; }
    }
}
