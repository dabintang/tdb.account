using System;
using System.Collections.Generic;
using System.Text;
using tdb.framework.webapi.IocAutofac;

namespace tdb.account.ibll
{
    /// <summary>
    /// 初始化
    /// </summary>
    public interface IInitBLL : IAutofacDependency
    {
        /// <summary>
        /// 初始化
        /// </summary>
        void Init();
    }
}
