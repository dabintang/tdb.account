using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tdb.account.common.Config;
using tdb.account.common.Const;
using tdb.account.ibll;
using tdb.framework.webapi.Config;
using tdb.framework.webapi.DTO;

namespace tdb.account.webapi.Controllers
{
    /// <summary>
    /// 管理工具
    /// </summary>
    public class ManageToolsController : BaseController
    {
        /// <summary>
        /// 管理工具
        /// </summary>
        private readonly IManageTools _manageTolls;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="manageTolls">管理工具</param>
        public ManageToolsController(IManageTools manageTolls)
        {
            this._manageTolls = manageTolls;
        }

        #region 接口

        /// <summary>
        /// 还原consul配置
        /// </summary>
        /// <param name="file">配置文件（.json文件）</param>
        /// <returns>还原结果</returns>
        [HttpPost]
        [Authorize(Roles = CstRole.SuperAdmin)]
        public BaseItemRes<string> RestoreConsulConfig(IFormFile file)
        {
            this._manageTolls.RestoreConsulConfig(file);
            return BaseItemRes<string>.Ok("还原完成");
        }

        /// <summary>
        /// 备份consul配置
        /// </summary>
        /// <returns>备份配置完整文件名</returns>
        [HttpPost]
        [Authorize(Roles = CstRole.SuperAdmin)]
        public BaseItemRes<string> BackupConsulConfig()
        {
            var fullFileName = this._manageTolls.BackupConsulConfig();
            return BaseItemRes<string>.Ok(fullFileName);
        }

        /// <summary>
        /// 根据数据库生成实体类
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public BaseItemRes<string> CreateDBModels()
        {
            this._manageTolls.CreateDBModels();
            return BaseItemRes<string>.Ok("实体类已生成");
        }

        /// <summary>
        /// 初始化数据库
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public BaseItemRes<string> InitDB()
        {
            var result = this._manageTolls.InitDB();
            var data = result ? "初始化数据库完成" : "初始化数据库失败";
            return BaseItemRes<string>.Ok(data);
        }

        /// <summary>
        /// 测试
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public BaseItemRes<bool> Test()
        {
            this._manageTolls.Test();
            return BaseItemRes<bool>.Ok(true);
        }

        #endregion
    }
}
