using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using tdb.framework.webapi.IocAutofac;

namespace tdb.account.webapi
{
    /// <summary>
    /// autofac注册模块
    /// </summary>
    public class AutofacModuleRegister : AutofacModule
    {
        /// <summary>
        /// 获取需要注册的程序集集合
        /// </summary>
        /// <returns></returns>
        protected override List<Assembly> GetRegisterAssemblys()
        {
            var list = new List<Assembly>();
            list.Add(Assembly.GetExecutingAssembly());
            list.Add(Assembly.Load("tdb.account.dal"));
            list.Add(Assembly.Load("tdb.account.bll"));

            return list;
        }
    }
}
