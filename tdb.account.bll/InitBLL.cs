using System;
using System.Collections.Generic;
using System.Text;
using tdb.account.ibll;
using tdb.account.idal;
using tdb.account.model;
using tdb.common;

namespace tdb.account.bll
{
    /// <summary>
    /// 初始化
    /// </summary>
    public class InitBLL : IInitBLL
    {
        /// <summary>
        /// 用户
        /// </summary>
        private readonly IUserDAL _userDAL;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userDAL">用户</param>
        public InitBLL(IUserDAL userDAL)
        {
            this._userDAL = userDAL;
        }

        #region 实现接口

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            this.InitDB();
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 初始化数据库数据
        /// </summary>
        private void InitDB()
        {
            #region 添加管理员账号

            var userAdmin = new User();
            userAdmin.UserCode = "admin";
            userAdmin.UserName = "管理员";
            userAdmin.Password = EncryptHelper.Md5("123456");
            userAdmin.IsAdmin = true;
            userAdmin.Enable = true;
            userAdmin.Creater = "0";
            userAdmin.CreateTime = DateTime.Now;
            userAdmin.Updater = "0";
            userAdmin.UpdateTime = DateTime.Now;

            if (_userDAL.ExistUser(userAdmin.UserCode) == false)
            {
                _userDAL.AddUser(userAdmin);
            }

            #endregion
        }

        #endregion
    }
}
