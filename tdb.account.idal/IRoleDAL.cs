using System;
using System.Collections.Generic;
using System.Text;
using tdb.account.model;
using tdb.framework.webapi.IocAutofac;

namespace tdb.account.idal
{
    /// <summary>
    /// 角色
    /// </summary>
    public interface IRoleDAL : IBaseDAL
    {
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="role">角色信息</param>
        void AddRole(Role role);

        /// <summary>
        /// 检查角色是否存在
        /// </summary>
        /// <param name="roleCode">角色编码</param>
        /// <returns>true：已存在；false：不存在</returns>
        bool ExistRole(string roleCode);

        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="roleCode">角色编码</param>
        /// <returns>角色信息</returns>
        Role GetRole(string roleCode);

        /// <summary>
        /// 修改角色信息
        /// </summary>
        /// <param name="role">角色信息</param>
        void UpdateRole(Role role);
    }
}
