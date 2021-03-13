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
        /// redis配置
        /// </summary>
        [ConsulConfig("Token")]
        public TokenConfig Token { get; set; }

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
        /// token相关配置
        /// </summary>
        public class TokenConfig
        {
            /// <summary>
            /// 秘钥
            /// </summary>
            public string Secret { get; set; }

            /// <summary>
            /// 超时时间（秒）
            /// </summary>
            public int TimeoutSeconds { get; set; }
        }

        #endregion
    }
}
