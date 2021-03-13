using System;
using System.Collections.Generic;
using System.Text;
using tdb.account.model;
using tdb.framework.webapi.IocAutofac;

namespace tdb.account.idal
{
    /// <summary>
    /// 用户
    /// </summary>
    public interface IUserDAL : IAutofacDependency
    {
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user">用户信息</param>
        void AddUser(User user);

        /// <summary>
        /// 检查用户是否存在
        /// </summary>
        /// <param name="userCode">用户编码</param>
        /// <returns>true：已存在；false：不存在</returns>
        bool ExistUser(string userCode);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userCode">用户编码</param>
        /// <returns>用户信息</returns>
        User GetUser(string userCode);

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="user">用户信息</param>
        void UpdateUser(User user);
    }
}
