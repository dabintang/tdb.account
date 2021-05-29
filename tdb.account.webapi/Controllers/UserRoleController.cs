using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tdb.account.common.Const;
using tdb.account.dto.UserRole;
using tdb.account.ibll;
using tdb.framework.webapi.APILog;
using tdb.framework.webapi.DTO;
using tdb.framework.webapi.Log;

namespace tdb.account.webapi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class UserRoleController : BaseController
    {
        /// <summary>
        /// 用户角色
        /// </summary>
        private readonly IUserRoleBLL userRoleBLL;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_userRoleBLL">用户角色</param>
        public UserRoleController(IUserRoleBLL _userRoleBLL)
        {
            this.userRoleBLL = _userRoleBLL;
        }

        #region 接口

        /// <summary>
        /// 设置用户角色
        /// </summary>
        /// <param name="req">条件</param>
        /// <returns>是否设置成功</returns>
        [HttpPost]
        [Authorize(Roles = CstRole.SuperAdmin)]
        [APILog(Level = EnumLogLevel.Info)]
        public BaseItemRes<bool> SetUserRole([FromBody] SetUserRoleReq req)
        {
            return this.userRoleBLL.SetUserRole(req, this.CurUser);
        }

        #endregion
    }
}
