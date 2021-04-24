using SqlSugar;
using SqlSugar.IOC;
using System;
using System.Collections.Generic;
using System.Text;

namespace tdb.account.dal
{
    /// <summary>
    /// 仓储
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Repository<T> : SimpleClient<T> where T : class, new()
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        public Repository(ISqlSugarClient context = null) : base(context)//注意这里要有默认值等于null
        {
            base.Context = DbScoped.Sugar;
        }
    }
}
