using SqlSugar;
using System;

namespace tdb.account.dbFirst
{
    class Program
    {
        static void Main(string[] args)
        {
            // 连接数据库
            var db = GetDB();

            //表映射
            db.MappingTables.Add("LoginLog", "login_log");
            db.MappingTables.Add("User", "user");

            // 生成路径
            var classDirectory = @"..\..\..\..\tdb.account.model";
            // 实体命名空间
            string classNamespace = "tdb.account.model";
            // 生成实体类
            db.DbFirst.IsCreateAttribute().CreateClassFile(classDirectory, classNamespace);

            Console.WriteLine("实体类已生成");
        }

        /// <summary>
        /// 获取数据库连接客户端
        /// </summary>
        /// <returns></returns>
        private static SqlSugarClient GetDB()
        {
            return new SqlSugar.SqlSugarClient(new SqlSugar.ConnectionConfig()
            {
                DbType = DbType.MySql,
                ConnectionString = "Host=127.0.0.1;Database=tdb.account;UserName=root;Password=123456;Port=3306;CharSet=utf8;Allow Zero Datetime=true",
                IsAutoCloseConnection = true,
                InitKeyType = SqlSugar.InitKeyType.Attribute
            });
        }
    }
}
