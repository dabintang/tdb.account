using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tdb.account.common.Config;
using tdb.framework.webapi.Log;

namespace tdb.account.dal
{
    /// <summary>
    /// 数据库上下文
    /// </summary>
    public class DBContext
    {
        /// <summary>
        /// 数据库上下文
        /// </summary>
        public SqlSugarClient DB { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public DBContext()
        {
            this.DB = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = AccConfig.Consul.DBConnStr,
                DbType = DbType.MySql,
                InitKeyType = InitKeyType.Attribute,//从特性读取主键和自增列信息
                IsAutoCloseConnection = true//开启自动释放模式
            });

            //调式代码 用来打印SQL 
            if (Logger.Ins.IsTraceEnabled)
            {
                this.DB.Aop.OnLogExecuting = (sql, pars) =>
                {
                    Logger.Ins.Trace($"执行SQL：{sql}{Environment.NewLine}{this.DB.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value))}");
                };
            }
        }
    }
}
