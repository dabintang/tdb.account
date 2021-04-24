using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using tdb.framework.webapi.IocAutofac;

namespace tdb.account.idal
{
    /// <summary>
    /// 基接口
    /// </summary>
    public interface IBaseDAL : IAutofacDependency
    {
        /// <summary>
        /// 多租户上下文
        /// </summary>
        ITenant AsTenant();
    }
}
