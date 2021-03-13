using System;
using System.Collections.Generic;
using System.Text;
using tdb.account.common;
using tdb.account.common.Config;
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
        /// 用户
        /// </summary>
        private readonly IUserDAL _userDAL;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userDAL">用户</param>
        public UserBLL(IUserDAL userDAL)
        {
            this._userDAL = userDAL;
        }

        #region 实现接口

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="req">条件</param>
        /// <returns>token</returns>
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

            //token信息
            var token = this.CreateToken(user);

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
            model.Password = req.Password;
            model.IsAdmin = false;
            model.Enable = false;
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
        /// <returns></returns>
        private string CreateToken(User user)
        {
            //承载信息
            var info = new OperatorInfo();
            info.UserCode = user.UserCode;
            info.UserName = user.UserName;
            info.IsAdmin = user.IsAdmin;

            var token = Verifier.Ins.Encode(info, AccConfig.Consul.Token.Secret, AccConfig.Consul.Token.TimeoutSeconds);
            return token;
        }

        #endregion
    }
}
