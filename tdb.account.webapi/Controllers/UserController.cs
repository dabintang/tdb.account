using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tdb.account.dto.User;
using tdb.account.ibll;
using tdb.framework.webapi.DTO;

namespace tdb.account.webapi.Controllers
{
    /// <summary>
    /// 用户
    /// </summary>
    public class UserController : BaseController
    {
        /// <summary>
        /// 初始化
        /// </summary>
        private readonly IUserBLL _userBLL;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userBLL">初始化</param>
        public UserController(IUserBLL userBLL)
        {
            this._userBLL = userBLL;
        }

        #region 接口

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="req">条件</param>
        /// <returns>token</returns>
        [HttpPost]
        [AllowAnonymous]
        public BaseItemRes<string> Login([FromBody]LoginReq req)
        {
            var res = this._userBLL.Login(req);
            return res;
        }

        ///// <summary>
        ///// 添加用户
        ///// </summary>
        ///// <param name="req">用户信息</param>
        //[HttpPost]
        //public BaseItemRes<bool> AddUser(AddUserReq req)
        //{

        //}



        #endregion
    }
}
