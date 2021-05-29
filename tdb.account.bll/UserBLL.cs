using Autofac;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using tdb.account.common;
using tdb.account.common.Config;
using tdb.account.dto;
using tdb.account.dto.Common;
using tdb.account.dto.User;
using tdb.account.ibll;
using tdb.account.idal;
using tdb.account.model;
using tdb.common;
using tdb.framework.webapi.Auth;
using tdb.framework.webapi.DTO;

namespace tdb.account.bll
{
    /// <summary>
    /// 用户
    /// </summary>
    public class UserBLL : BaseBLL, IUserBLL
    {
        /// <summary>
        /// Autofac上下文
        /// </summary>
        private readonly IComponentContext componentContext;

        /// <summary>
        /// 用户
        /// </summary>
        private readonly IUserDAL userDAL;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_componentContext">Autofac上下文</param>
        /// <param name="_userDAL">用户</param>
        public UserBLL(IComponentContext _componentContext, IUserDAL _userDAL)
        {
            this.componentContext = _componentContext;
            this.userDAL = _userDAL;
        }

        #region 实现接口

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="req">条件</param>
        /// <returns>toekn</returns>
        public BaseItemRes<string> Login(LoginReq req)
        {
            //获取用户
            var user = this.userDAL.GetUser(req.LoginName);
            //判断用户是否存在
            if (user == null)
            {
                return AccHelper.FailItemRes(AccConfig.Msg.ErrLoginNameOrPassword, "");
            }

            //是否可用
            if (user.Enable == false)
            {
                return AccHelper.FailItemRes(AccConfig.Msg.DisableUser, "");
            }

            //密码加密成MD5
            var pwd = EncryptHelper.Md5(req.Password);

            //判断密码是否正确
            if (user.Password != pwd)
            {
                return AccHelper.FailItemRes(AccConfig.Msg.ErrLoginNameOrPassword, "");
            }

            //获取角色
            var userRoleDAL = this.GetUserRoleDAL();
            var lstUserRole = userRoleDAL.QueryUserRole(user.LoginName);

            //token
            var token = this.CreateToken(user, lstUserRole);

            #region 登录日志

            var log = new LoginLog();
            log.LoginName = user.LoginName;
            log.ServiceCode = req.ServiceCode;
            log.Token = token;
            this.SetCreateFields(log, null);

            var loginLogDAL = this.GetLoginLogDAL();
            loginLogDAL.AddLoginLogAsync(log);

            #endregion

            return AccHelper.OkItemRes(token);
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="req">用户信息</param>
        /// <param name="oper">操作者信息</param>
        public BaseItemRes<bool> AddUser(AddUserReq req, OperatorInfo oper)
        {
            //判断登录名是否已存在
            if (this.userDAL.ExistUser(req.LoginName))
            {
                return AccHelper.FailItemRes(AccConfig.Msg.ExistLoginName, false);
            }

            //用户实体
            var model = new User();
            model.LoginName = req.LoginName;
            model.UserName = req.UserName;
            model.Password = EncryptHelper.Md5(req.Password);
            model.Gender = req.Gender;
            model.Birthday = req.Birthday;
            model.MobilePhone = req.MobilePhone;
            model.Email = req.Email;
            model.Enable = true;
            this.SetCreateUpdateFields(model, oper);

            //插入数据库
            this.userDAL.AddUser(model);

            return AccHelper.OkItemRes(true);
        }

        /// <summary>
        /// 检查用户是否存在
        /// </summary>
        /// <param name="req">条件</param>
        /// <returns>true：已存在；false：不存在</returns>
        public bool ExistUser(ExistUserReq req)
        {
            return this.userDAL.ExistUser(req.LoginName);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="req">条件</param>
        /// <returns>用户信息</returns>
        public UserInfo GetUser(GetUserReq req)
        {
            var user = this.userDAL.GetUser(req.LoginName);
            var userInfo = AccMapper.Ins.Map<UserInfo>(user);

            return userInfo;
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="req">用户信息</param>
        /// <param name="oper">操作者信息</param>
        public BaseItemRes<bool> UpdateUser(UpdateUserReq req, OperatorInfo oper)
        {
            //判断用户是否存在
            var user = this.userDAL.GetUser(req.LoginName);
            if (user == null)
            {
                return AccHelper.FailItemRes(AccConfig.Msg.UserNotExist, false);
            }

            user.UserName = req.UserName;
            user.Gender = req.Gender;
            user.Birthday = req.Birthday;
            user.MobilePhone = req.MobilePhone;
            user.Email = req.Email;
            this.SetUpdateFields(user, oper);

            //更新数据库
            this.userDAL.UpdateUser(user);

            return AccHelper.OkItemRes(true);
        }

        /// <summary>
        /// 修改用户启用状态
        /// </summary>
        /// <param name="req">条件</param>
        /// <param name="oper">操作者信息</param>
        public BaseItemRes<bool> UpdateUserEnable(UpdateUserEnableReq req, OperatorInfo oper)
        {
            //判断用户是否存在
            var user = this.userDAL.GetUser(req.LoginName);
            if (user == null)
            {
                return AccHelper.FailItemRes(AccConfig.Msg.UserNotExist, false);
            }

            user.Enable = req.Enable;
            this.SetUpdateFields(user, oper);

            //更新数据库
            this.userDAL.UpdateUser(user);

            return AccHelper.OkItemRes(true);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="req">条件</param>
        /// <param name="oper">操作者信息</param>
        public BaseItemRes<bool> UpdatePassword(UpdatePasswordReq req, OperatorInfo oper)
        {
            //判断用户是否存在
            var user = this.userDAL.GetUser(req.LoginName);
            if (user == null)
            {
                return AccHelper.FailItemRes(AccConfig.Msg.UserNotExist, false);
            }

            user.Password = req.Password;
            this.SetUpdateFields(user, oper);

            //更新数据库
            this.userDAL.UpdateUser(user);

            return AccHelper.OkItemRes(true);
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 生成token信息
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="lstUserRole">用户角色</param>
        /// <returns>token</returns>
        private string CreateToken(User user, List<UserRole> lstUserRole)
        {
            //用户信息
            var lstClaim = new List<Claim>();
            lstClaim.Add(new Claim(TdbClaimTypes.SID, user.LoginName));
            lstClaim.Add(new Claim(TdbClaimTypes.Name, user.UserName));

            //权限
            foreach (var userRole in lstUserRole)
            {
                lstClaim.Add(new Claim(TdbClaimTypes.Role, userRole.RoleCode));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(lstClaim),
                Issuer = AccConfig.Consul.Token.Issuer,
                Audience = AccConfig.Consul.Token.Audience,
                Expires = DateTime.UtcNow.AddSeconds(AccConfig.Consul.Token.TimeoutSeconds),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AccConfig.Consul.Token.SecretKey)), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        /// <summary>
        /// 登录日志
        /// </summary>
        private ILoginLogDAL GetLoginLogDAL()
        {
            return this.componentContext.Resolve<ILoginLogDAL>();
        }

        /// <summary>
        /// 用户角色
        /// </summary>
        private IUserRoleDAL GetUserRoleDAL()
        {
            return this.componentContext.Resolve<IUserRoleDAL>();
        }

        #endregion
    }
}
