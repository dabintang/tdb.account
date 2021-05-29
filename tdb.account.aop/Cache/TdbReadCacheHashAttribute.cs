using System;
using System.Collections.Generic;
using System.Text;
using tdb.account.aop.Cache;

namespace tdb.account.aop
{
    /// <summary>
    /// 读取hash类型缓存特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class TdbReadCacheHashAttribute : Attribute
    {
        /// <summary>
        /// key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 缓存天数（默认1天）
        /// </summary>
        public int TimeoutDays { get; set; } = 1;

        /// <summary>
        /// 过期时间（格式：HH:mm:ss，默认值："01:00:00"）
        /// </summary>
        public string ExpireAtTime { get; set; } = "01:00:00";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key">key</param>
        public TdbReadCacheHashAttribute(string key)
        {
            this.Key = key;
        }
    }
}
