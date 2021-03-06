using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using tdb.account.dto.Common;
using tdb.framework.webapi.APILog;
using tdb.framework.webapi.Auth;

namespace tdb.account.webapi.Controllers
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    [Route("tdbaccount/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class BaseController : ControllerBase
    {
        private OperatorInfo _CurUser;
        /// <summary>
        /// 当前用户
        /// </summary>
        protected virtual OperatorInfo CurUser
        {
            get
            { 
                //无认证用户
                if (HttpContext.User == null)
                {
                    return null;
                }

                if (this._CurUser == null)
                {
                    this._CurUser = new OperatorInfo();
                    this._CurUser.LoginName = HttpContext.User.FindFirst(TdbClaimTypes.SID).Value;
                    this._CurUser.UserName = HttpContext.User.Identity.Name;
                }

                return this._CurUser;
            }
        }

        /// <summary>
        /// 当前用户拥有的角色
        /// </summary>
        protected virtual List<string> CurUserRoles
        {
            get
            {
                //无认证用户
                if (HttpContext.User == null)
                {
                    return new List<string>();
                }

                var lstRoleClaim = HttpContext.User.FindAll(TdbClaimTypes.Role);
                var lstRoleCode = lstRoleClaim.Select(m => m.Value).ToList();

                return lstRoleCode;
            }
        }
    }
}
