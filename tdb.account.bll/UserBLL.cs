using Autofac;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using tdb.account.common;
using tdb.account.common.Config;
using tdb.account.dto.Common;
using tdb.account.dto.User;
using tdb.account.ibll;
using tdb.account.idal;
using tdb.account.model;
using tdb.common;
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
        private readonly IComponentContext _componentContext;

        /// <summary>
        /// 用户
        /// </summary>
        private readonly IUserDAL _userDAL;

        /// <summary>
        /// 登录日志
        /// </summary>
        private ILoginLogDAL _loginLogDAL
        { 
            get
            {
                return this._componentContext.Resolve<ILoginLogDAL>();
            }
        }

        /// <summary>
        /// 用户角色
        /// </summary>
        private IUserRoleDAL _userRoleDAL
        {
            get
            {
                return this._componentContext.Resolve<IUserRoleDAL>();
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="componentContext">Autofac上下文</param>
        /// <param name="userDAL">用户</param>
        public UserBLL(IComponentContext componentContext, IUserDAL userDAL)
        {
            this._componentContext = componentContext;
            this._userDAL = userDAL;
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
            var user = this._userDAL.GetUser(req.UserCode);
            //判断用户是否存在
            if (user == null)
            {
                return AccHelper.FailItemRes(AccConfig.Msg.ErrUserCodeOrPassword, "");
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
                return AccHelper.FailItemRes(AccConfig.Msg.ErrUserCodeOrPassword, "");
            }

            //获取角色
            var lstUserRole = this._userRoleDAL.QueryUserRole(user.UserCode);

            //token
            var token = this.CreateToken(user, lstUserRole);

            //登录日志
            var log = new LoginLog();
            log.UserCode = user.UserCode;
            log.ServiceCode = req.ServiceCode;
            log.Token = token;
            this.SetCreateFields(log, null);
            this._loginLogDAL.AddLoginLogAsync(log);

            return AccHelper.OkItemRes(token); ;
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="req">用户信息</param>
        /// <param name="oper">操作者信息</param>
        public BaseItemRes<bool> AddUser(AddUserReq req, OperatorInfo oper)
        {
            //判断登录名是否已存在
            if (this._userDAL.ExistUser(req.UserCode))
            {
                return AccHelper.FailItemRes(AccConfig.Msg.ExistUserCode, false);
            }

            //用户实体
            var model = new User();
            model.UserCode = req.UserCode;
            model.UserName = req.UserName;
            model.Password = EncryptHelper.Md5(req.Password);
            model.Gender = req.Gender;
            model.Birthday = req.Birthday;
            model.MobilePhone = req.MobilePhone;
            model.Email = req.Email;
            model.Enable = true;
            this.SetCreateUpdateFields(model, oper);

            //插入数据库
            this._userDAL.AddUser(model);

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
            lstClaim.Add(new Claim(ClaimTypes.Sid, user.UserCode));
            lstClaim.Add(new Claim(ClaimTypes.Name, user.UserName));

            //权限
            foreach (var userRole in lstUserRole)
            {
                lstClaim.Add(new Claim(ClaimTypes.Role, userRole.RoleCode));
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

        #endregion
    }
}
