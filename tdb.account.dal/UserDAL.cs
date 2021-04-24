using SqlSugar.IOC;
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
    public class UserDAL : Repository<User>, IUserDAL
    {
        #region 实现接口

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user">用户信息</param>
        public void AddUser(User user)
        {
            this.AsInsertable(user).ExecuteCommand();
        }

        /// <summary>
        /// 检查用户是否存在
        /// </summary>
        /// <param name="userCode">用户编码</param>
        /// <returns>true：已存在；false：不存在</returns>
        public bool ExistUser(string userCode)
        {
            var count = this.AsQueryable().Count(m => m.UserCode == userCode);
            return count > 0;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userCode">用户编码</param>
        /// <returns>用户信息</returns>
        public User GetUser(string userCode)
        {
            return this.GetSingle(m => m.UserCode == userCode);
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="user">用户信息</param>
        public void UpdateUser(User user)
        {
            this.AsUpdateable(user).ExecuteCommand();
        }

        #endregion
    }
}
