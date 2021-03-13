using System;
using System.Collections.Generic;
using System.Text;
using tdb.appsettings;
using tdb.common;

namespace tdb.account.common.Config
{
    /// <summary>
    /// appsettings.json配置
    /// </summary>
    public class AppConfig
    {
        /// <summary>
        /// 本服务url
        /// </summary>
        [AppsettingsConfig("Kestrel:EndPoints:Server:Url")]
        public string LocalUrl { get; set; }

        /// <summary>
        /// consul服务配置
        /// </summary>
        public ConsulServiceConfig Consul { get; set; }

        /// <summary>
        /// 拼好IP：端口的地址
        /// </summary>
        public string ApiUrl
        {
            get
            {
                var ip = CommHelper.GetLocalIP();
                var url = this.LocalUrl;
                url = url.Replace("*", ip);

                return url;
            }
        }
    }

    /// <summary>
    /// consul服务配置
    /// </summary>
    public class ConsulServiceConfig
    {
        /// <summary>
        /// IP
        /// </summary>
        [AppsettingsConfig("Consul:IP")]
        public string IP { get; set; }

        /// <summary>
        /// Port
        /// </summary>
        [AppsettingsConfig("Consul:Port")]
        public int Port { get; set; }

        /// <summary>
        /// 服务编码
        /// </summary>
        [AppsettingsConfig("Consul:ServiceCode")]
        public string ServiceCode { get; set; }
    }
}
