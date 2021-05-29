using System;
using System.Collections.Generic;
using System.Text;

namespace tdb.account.aop.Cache
{
    /// <summary>
    /// 缓存key获取方法特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class TdbCacheKeyAttribute : Attribute
    {
        /// <summary>
        /// 参数位置
        /// </summary>
        public int ParamIndex { get; set; }

        /// <summary>
        /// 从执行属性获取
        /// </summary>
        public string FromPropertyName { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="paramIndex">参数位置</param>
        public TdbCacheKeyAttribute(int paramIndex)
        {
            this.ParamIndex = paramIndex;
        }
    }
}
