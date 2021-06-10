using System;
using System.Collections.Generic;
using System.Text;
using tdb.consul.kv;

namespace tdb.account.common.Config
{
    /// <summary>
    /// consul配置
    /// </summary>
    public class ConsulConfig
    {
        /// <summary>
        /// 是否已设置consul配置
        /// </summary>
        public bool HadConsulConfig()
        {
            return !string.IsNullOrEmpty(DBLogConnStr);
        }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        [ConsulConfig("DBConnStr")]
        public string DBConnStr { get; set; }

        /// <summary>
        /// 日志数据库连接字符串
        /// </summary>
        [ConsulConfig("DBLogConnStr")]
        public string DBLogConnStr { get; set; }

        /// <summary>
        /// redis配置
        /// </summary>
        [ConsulConfig("Redis")]
        public RedisConfig Redis { get; set; }

        /// <summary>
        /// 认证相关配置
        /// </summary>
        [ConsulConfig("Token")]
        public TokenConfig Token { get; set; }

        /// <summary>
        /// 默认密码
        /// </summary>
        [ConsulConfig("DefaultPassword")]
        public string DefaultPassword { get; set; }

        /// <summary>
        /// HttpReports配置
        /// </summary>
        [ConsulConfig("HttpReports")]
        public HttpReportsConfig HttpReports { get; set; }

        #region 内部类

        /// <summary>
        /// redis配置
        /// </summary>
        public class RedisConfig
        {
            /// <summary>
            /// 连接字符串
            /// </summary>
            public List<string> ConnectString { get; set; }
        }

        /// <summary>
        /// 认证相关配置
        /// </summary>
        public class TokenConfig
        {
            /// <summary>
            /// 发行者
            /// </summary>
            public string Issuer { get; set; }

            /// <summary>
            /// 接收者
            /// </summary>
            public string Audience { get; set; }

            /// <summary>
            /// 秘钥（至少16位）
            /// </summary>
            public string SecretKey { get; set; }

            /// <summary>
            /// 超时时间（秒）
            /// </summary>
            public int TimeoutSeconds { get; set; }
        }

        /// <summary>
        /// HttpReports配置
        /// </summary>
        public class HttpReportsConfig
        {
            /// <summary>
            /// Transport配置
            /// </summary>
            public HttpReportsTransportConfig Transport { get; set; }

            /// <summary>
            /// 是否开启收集数据
            /// </summary>
            public bool Switch { get; set; }

            /// <summary>
            /// 数据过滤，用 * 来模糊匹配
            /// (如：["/tdbaccount/Sys/HealthCheck*"])
            /// </summary>
            public List<string> RequestFilter { get; set; }

            /// <summary>
            /// 是否记录接口的入参
            /// </summary>
            public bool WithRequest { get; set; }

            /// <summary>
            /// 是否记录接口的出参
            /// </summary>
            public bool WithResponse { get; set; }

            /// <summary>
            /// 是否记录Cookie 信息
            /// </summary>
            public bool WithCookie { get; set; }

            /// <summary>
            /// 是否记录请求Header信息
            /// </summary>
            public bool WithHeader { get; set; }
        }

        /// <summary>
        /// HttpReports.Transport配置
        /// </summary>
        public class HttpReportsTransportConfig
        {
            /// <summary>
            /// 数据发送的地址，配置Dashboard 的项目地址即可
            /// (如：http://127.0.0.1:11201)
            /// </summary>
            public string CollectorAddress { get; set; }

            /// <summary>
            /// 批量数据入库的秒数，建议值 5-60
            /// </summary>
            public int DeferSecond { get; set; }

            /// <summary>
            /// 批量数据入库的数量，建议值100-300
            /// </summary>
            public int DeferThreshold { get; set; }
        }

        #endregion
    }
}
