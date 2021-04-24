using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using tdb.framework.webapi.IocAutofac;

namespace tdb.account.ibll
{
    /// <summary>
    /// 管理工具
    /// </summary>
    public interface IManageTools : IAutofacDependency
    {
        /// <summary>
        /// 备份consul配置
        /// </summary>
        /// <returns>备份配置完整文件名</returns>
        string BackupConsulConfig();

        /// <summary>
        /// 还原consul配置
        /// </summary>
        /// <param name="file">配置文件（.json文件）</param>
        void RestoreConsulConfig(IFormFile file);

        /// <summary>
        /// 根据数据库表结构生成实体类
        /// </summary>
        void CreateDBModels();

        /// <summary>
        /// 初始化数据库
        /// </summary>
        bool InitDB();

        /// <summary>
        /// 测试
        /// </summary>
        void Test();
    }
}
