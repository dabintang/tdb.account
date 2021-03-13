using System;
using System.Collections.Generic;
using System.Text;
using tdb.account.idal;
using tdb.account.model;

namespace tdb.account.dal
{
    /// <summary>
    /// 用户
    /// </summary>
    public class UserDAL : DBContext, IUserDAL
    {
        #region 实现接口

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user">用户信息</param>
        public void AddUser(User user)
        {
            this.DB.Insertable(user).ExecuteCommand();
        }

        /// <summary>
        /// 检查用户是否存在
        /// </summary>
        /// <param name="userCode">用户编码</param>
        /// <returns>true：已存在；false：不存在</returns>
        public bool ExistUser(string userCode)
        {
            var count = this.DB.Queryable<User>().Count(m => m.UserCode == userCode);
            return count > 0;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userCode">用户编码</param>
        /// <returns>用户信息</returns>
        public User GetUser(string userCode)
        {
            var user = this.DB.Queryable<User>().Where(m => m.UserCode == userCode).First();
            return user;
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="user">用户信息</param>
        public void UpdateUser(User user)
        {
            this.DB.Updateable(user).ExecuteCommand();
        }

        #endregion
    }
}
