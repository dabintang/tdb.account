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
using tdb.account.dal;
using tdb.account.dto.Common;
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
        private readonly IManageTools manageTolls;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_manageTolls">管理工具</param>
        public ManageToolsController(IManageTools _manageTolls)
        {
            this.manageTolls = _manageTolls;
        }

        #region 接口

        /// <summary>
        /// 获取appsettings.json配置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = CstRole.SuperAdmin)]
        public BaseItemRes<AppConfig> GetAppConfig()
        {
            return BaseItemRes<AppConfig>.Ok(AccConfig.App);
        }

        /// <summary>
        /// 获取consul配置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = CstRole.SuperAdmin)]
        public BaseItemRes<ConsulConfig> GetConsulConfig()
        {
            return BaseItemRes<ConsulConfig>.Ok(AccConfig.Consul);
        }

        /// <summary>
        /// 还原consul配置
        /// </summary>
        /// <param name="file">配置文件（.json文件）</param>
        /// <returns>还原结果</returns>
        [HttpPost]
        [Authorize(Roles = CstRole.SuperAdmin)]
        public BaseItemRes<string> RestoreConsulConfig(IFormFile file)
        {
            this.manageTolls.RestoreConsulConfig(file);
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
            var fullFileName = this.manageTolls.BackupConsulConfig();
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
            this.manageTolls.CreateDBModels();
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
            var result = this.manageTolls.InitDB();
            var data = result ? "初始化数据库完成" : "初始化数据库失败";
            return BaseItemRes<string>.Ok(data);
        }

        /// <summary>
        /// 测试
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public BaseItemRes<InitDataInfo> Test()
        {
            var userDAL = new UserDAL();
            var lstUser = userDAL.AsQueryable().ToList();

            var roleDAL = new RoleDAL();
            var lstRole = roleDAL.AsQueryable().ToList();

            var authorityDAL = new AuthorityDAL();
            var lstAuthority = authorityDAL.AsQueryable().ToList();

            var roleAuthorityDAL = new RoleAuthorityDAL();
            var lstRoleAuthority = roleAuthorityDAL.AsQueryable().ToList();

            var userRoleDAL = new UserRoleDAL();
            var lstUserRole = userRoleDAL.AsQueryable().ToList();

            var initData = new InitDataInfo()
            {
                Users = lstUser,
                Roles = lstRole,
                Authoritys = lstAuthority,
                RoleAuthoritys = lstRoleAuthority,
                UserRoles = lstUserRole
            };

            return BaseItemRes<InitDataInfo>.Ok(initData);
        }

        #endregion
    }
}
