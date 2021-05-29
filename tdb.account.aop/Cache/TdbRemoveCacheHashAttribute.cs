using System;
using System.Collections.Generic;
using System.Text;
using tdb.account.aop.Cache;

namespace tdb.account.aop
{
    /// <summary>
    /// 清除hash类型缓存特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class TdbRemoveCacheHashAttribute : Attribute
    {
        /// <summary>
        /// key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key">key</param>
        public TdbRemoveCacheHashAttribute(string key)
        {
            this.Key = key;
        }
    }
}
