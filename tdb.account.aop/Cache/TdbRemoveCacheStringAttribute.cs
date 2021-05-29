using System;
using System.Collections.Generic;
using System.Text;
using tdb.account.aop.Cache;

namespace tdb.account.aop
{
    /// <summary>
    /// 清除key-value类型缓存特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class TdbRemoveCacheStringAttribute : Attribute
    {
        /// <summary>
        /// key前缀
        /// </summary>
        public string KeyPrefix { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="keyPrefix">key前缀</param>
        public TdbRemoveCacheStringAttribute(string keyPrefix)
        {
            this.KeyPrefix = keyPrefix;
        }
    }
}
