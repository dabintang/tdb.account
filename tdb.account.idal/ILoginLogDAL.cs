using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tdb.account.model;
using tdb.framework.webapi.IocAutofac;

namespace tdb.account.idal
{
    /// <summary>
    /// 登录日志
    /// </summary>
    public interface ILoginLogDAL : IAutofacDependency
    {
        /// <summary>
        /// 添加登录日志（异步）
        /// </summary>
        /// <param name="log">登录日志信息</param>
        /// <returns>主键ID</returns>
        Task<long> AddLoginLogAsync(LoginLog log);
    }
}
