using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tdb.account.common.Config;
using tdb.account.common.Const;
using tdb.account.dto;
using tdb.account.dto.User;
using tdb.account.ibll;
using tdb.framework.webapi.APILog;
using tdb.framework.webapi.DTO;
using tdb.framework.webapi.Log;

namespace tdb.account.webapi.Controllers
{
    /// <summary>
    /// 用户
    /// </summary>
    public class UserController : BaseController
    {
        /// <summary>
        /// 用户
        /// </summary>
        private readonly IUserBLL userBLL;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_userBLL">用户</param>
        public UserController(IUserBLL _userBLL)
        {
            this.userBLL = _userBLL;
        }

        #region 接口

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="req">条件</param>
        /// <returns>token</returns>
        [HttpPost]
        [AllowAnonymous]
        public BaseItemRes<string> Login([FromBody] LoginReq req)
        {
            var res = this.userBLL.Login(req);
            return res;
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="req">用户信息</param>
        /// <returns>是否添加成功</returns>
        [HttpPost]
        [Authorize(Policy = CstPolicy.NeedAuthorityManageUser)]
        [APILog(Level = EnumLogLevel.Info)]
        public BaseItemRes<bool> AddUser([FromBody] AddUserReq req)
        {
            return this.userBLL.AddUser(req, this.CurUser);
        }

        /// <summary>
        /// 检查用户是否存在
        /// </summary>
        /// <param name="req">条件</param>
        /// <returns>存在：true；否则：false</returns>
        [HttpGet]
        public BaseItemRes<bool> ExistUser([FromQuery] ExistUserReq req)
        {
            var exist = this.userBLL.ExistUser(req);
            return BaseItemRes<bool>.Ok(exist);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="req">条件</param>
        /// <returns>用户信息</returns>
        [HttpGet]
        [Authorize(Policy = CstPolicy.NeedAuthorityManageUser)]
        public BaseItemRes<UserInfo> GetUser([FromQuery] GetUserReq req)
        {
            var ip = HttpContext.Connection.RemoteIpAddress.ToString();

            var userInfo = this.userBLL.GetUser(req);
            return BaseItemRes<UserInfo>.Ok(userInfo);
        }

        /// <summary>
        /// 获取客户Ip
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string GetClientUserIp(HttpContext context)
        {
            var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Connection.RemoteIpAddress.ToString();
            }
            return ip;
        }

        /// <summary>
        /// 获取自己的用户信息
        /// </summary>
        /// <returns>用户信息</returns>
        [HttpGet]
        public BaseItemRes<UserInfo> GetMyUserInfo()
        {
            var req = new GetUserReq();
            req.LoginName = this.CurUser.LoginName;

            var userInfo = this.userBLL.GetUser(req);
            return BaseItemRes<UserInfo>.Ok(userInfo);
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="req">用户信息</param>
        /// <returns>是否更新成功</returns>
        [HttpPost]
        [Authorize(Policy = CstPolicy.NeedAuthorityManageUser)]
        [APILog(Level = EnumLogLevel.Info)]
        public BaseItemRes<bool> UpdateUser([FromBody] UpdateUserReq req)
        {
            return this.userBLL.UpdateUser(req, this.CurUser);
        }

        /// <summary>
        /// 修改自己的用户信息
        /// </summary>
        /// <param name="req">用户信息</param>
        /// <returns>是否更新成功</returns>
        [HttpPost]
        [APILog(Level = EnumLogLevel.Info)]
        public BaseItemRes<bool> UpdateMyUserInfo([FromBody] UpdateMyUserInfoReq req)
        {
            var reqIn = AccMapper.Ins.Map<UpdateUserReq>(req);
            reqIn.LoginName = this.CurUser.LoginName;

            return this.userBLL.UpdateUser(reqIn, this.CurUser);
        }

        /// <summary>
        /// 修改用户启用状态
        /// </summary>
        /// <param name="req">用户信息</param>
        /// <returns>是否更新成功</returns>
        [HttpPost]
        [Authorize(Policy = CstPolicy.NeedAuthorityManageUser)]
        [APILog(Level = EnumLogLevel.Info)]
        public BaseItemRes<bool> UpdateUserEnable([FromBody] UpdateUserEnableReq req)
        {
            return this.userBLL.UpdateUserEnable(req, this.CurUser);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="req">用户信息</param>
        /// <returns>是否更新成功</returns>
        [HttpPost]
        [APILog(Level = EnumLogLevel.Info)]
        public BaseItemRes<bool> UpdatePassword([FromBody] UpdatePasswordReq req)
        {
            return this.userBLL.UpdatePassword(req, this.CurUser);
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="req">用户信息</param>
        /// <returns>是否更新成功</returns>
        [HttpPost]
        [Authorize(Policy = CstPolicy.NeedAuthorityManageUser)]
        [APILog(Level = EnumLogLevel.Info)]
        public BaseItemRes<bool> ResetPassword([FromBody] ResetPasswordReq req)
        {
            var reqIn = new UpdatePasswordReq();
            reqIn.LoginName = req.LoginName;
            reqIn.Password = AccConfig.Consul.DefaultPassword;

            return this.userBLL.UpdatePassword(reqIn, this.CurUser);
        }

        #endregion

    }
}
