using System;
using System.Collections.Generic;
using System.Text;
using tdb.account.model;

namespace tdb.account.idal
{
    /// <summary>
    /// 用户角色
    /// </summary>
    public interface IUserRoleDAL : IBaseDAL
    {
        /// <summary>
        /// 设置指定用户角色
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="lstUserRole">用户角色</param>
        bool SetUserRole(string loginName, List<UserRole> lstUserRole);

        /// <summary>
        /// 查询指定用户的用户角色
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <returns>用户角色</returns>
        List<UserRole> QueryUserRole(string loginName);
    }
}
