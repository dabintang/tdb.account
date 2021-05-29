using Castle.DynamicProxy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using tdb.common;
using tdb.framework.webapi.Cache;
using tdb.framework.webapi.Exceptions;

namespace tdb.account.aop.Cache
{
    /// <summary>
    /// 缓存拦截器
    /// </summary>
    public class TdbCacheInterceptor : IInterceptor
    {
        /// <summary>
        /// 拦截方法
        /// </summary>
        /// <param name="invocation">包含被拦截方法的信息</param>
        public void Intercept(IInvocation invocation)
        {
            var keyFields = this.GetKey(invocation);

            #region 读取key-value类型缓存

            var attrReadString = invocation.MethodInvocationTarget.GetCustomAttributes(typeof(TdbReadCacheStringAttribute), true).FirstOrDefault() as TdbReadCacheStringAttribute;
            if (attrReadString != null)
            {
                var key = $"{attrReadString.KeyPrefix}{keyFields}";
                var returnValue = Cacher.Ins.CacheShell(key, TimeSpan.FromSeconds(attrReadString.TimeoutSeconds), () =>
                {
                    //执行被拦截的方法
                    invocation.Proceed();
                    return invocation.ReturnValue;
                });
                invocation.ReturnValue = returnValue;

                return;
            }

            #endregion

            #region 读取hash类型缓存

            var attrReadHash = invocation.MethodInvocationTarget.GetCustomAttributes(typeof(TdbReadCacheHashAttribute), true).FirstOrDefault() as TdbReadCacheHashAttribute;
            if (attrReadHash != null)
            {
                //过期时间点
                var expireAt = DateTime.Today.AddDays(attrReadHash.TimeoutDays);
                expireAt = Convert.ToDateTime($"{expireAt.ToString("yyyy-MM-dd")} {attrReadHash.ExpireAtTime}", new DateTimeFormatInfo() { FullDateTimePattern = "yyyy-MM-dd HH:mm:ss" });

                var returnValue = Cacher.Ins.CacheShell(attrReadHash.Key, keyFields, expireAt, () =>
                {
                    //执行被拦截的方法
                    invocation.Proceed();
                    return JsonConvert.SerializeObject(invocation.ReturnValue);
                });
                invocation.ReturnValue = JsonConvert.DeserializeObject(returnValue, invocation.MethodInvocationTarget.ReturnType);

                return;
            }

            #endregion

            #region 清除key-value类型缓存

            var attrRemoveString = invocation.MethodInvocationTarget.GetCustomAttributes(typeof(TdbRemoveCacheStringAttribute), true).FirstOrDefault() as TdbRemoveCacheStringAttribute;
            if (attrRemoveString != null)
            {
                //执行被拦截的方法
                invocation.Proceed();

                var key = $"{attrRemoveString.KeyPrefix}{keyFields}";
                Cacher.Ins.Del(key);

                return;
            }

            #endregion

            #region 清除hash类型缓存

            var attrRemoveHash = invocation.MethodInvocationTarget.GetCustomAttributes(typeof(TdbRemoveCacheHashAttribute), true).FirstOrDefault() as TdbRemoveCacheHashAttribute;
            if (attrRemoveHash != null)
            {
                //执行被拦截的方法
                invocation.Proceed();

                Cacher.Ins.HDel(attrRemoveHash.Key, keyFields);

                return;
            }

            #endregion

            //执行被拦截的方法
            invocation.Proceed();
        }

        /// <summary>
        /// 获取key
        /// </summary>
        /// <param name="invocation"></param>
        /// <returns></returns>
        private string GetKey(IInvocation invocation)
        {
            var sb = new StringBuilder();
            var attrKeys = invocation.MethodInvocationTarget.GetCustomAttributes(typeof(TdbCacheKeyAttribute), true).Select(m => m as TdbCacheKeyAttribute);
            foreach (var attrKey in attrKeys)
            {
                //获取参数值
                var param = invocation.GetArgumentValue(attrKey.ParamIndex);

                //直接获取
                if (string.IsNullOrWhiteSpace(attrKey.FromPropertyName))
                {
                    sb.Append(this.ToStr(param));
                }
                //从属性获取
                else
                {
                    ///属性不存在
                    if (CommHelper.IsExistProperty(param, attrKey.FromPropertyName) == false)
                    {
                        throw new TdbException($"[缓存拦截器]找不到属性：{param.GetType().Name}.{attrKey.FromPropertyName}");
                    }

                    var paramValue = CommHelper.ReflectGet(param, attrKey.FromPropertyName);
                    sb.Append(this.ToStr(paramValue));
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 参数转字符串
        /// </summary>
        /// <param name="param">参数</param>
        /// <returns></returns>
        private string ToStr(object param)
        {
            if (param is string)
            {
                return param as string;
            }

            return JsonConvert.SerializeObject(param);
        }
    }
}
