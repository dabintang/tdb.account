using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
            // appsettings.json配置
            services.AddTdbJsonConfig();
            App = LocalConfigurator.Ins.GetConfig<AppConfig>();

            //consul配置
            services.AddTdbConsulConfig(App.Consul.IP, App.Consul.Port, App.Consul.ServiceCode + "_");
            Consul = DistributedConfigurator.Ins.GetConfig<ConsulConfig>();

            //消息配置
            var msgFullFileName = CommHelper.GetFullFileName("message.json");
            var msgJsonStr = File.ReadAllText(msgFullFileName);
            Msg = JsonConvert.DeserializeObject<MsgConfig>(msgJsonStr);
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
