using System;
using System.Collections.Generic;
using System.Text;
using tdb.account.idal;
using tdb.account.model;

namespace tdb.account.dal
{
    /// <summary>
    /// 角色
    /// </summary>
    public class RoleDAL : Repository<Role>, IRoleDAL
    {
        #region 实现接口

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="role">角色信息</param>
        public void AddRole(Role role)
        {
            this.AsInsertable(role).ExecuteCommand();
        }

        /// <summary>
        /// 检查角色是否存在
        /// </summary>
        /// <param name="roleCode">角色编码</param>
        /// <returns>true：已存在；false：不存在</returns>
        public bool ExistRole(string roleCode)
        {
            var count = this.AsQueryable().Count(m => m.RoleCode == roleCode);
            return count > 0;
        }

        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="roleCode">角色编码</param>
        /// <returns>角色信息</returns>
        public Role GetRole(string roleCode)
        {
            return this.GetSingle(m => m.RoleCode == roleCode);
        }

        /// <summary>
        /// 修改角色信息
        /// </summary>
        /// <param name="role">角色信息</param>
        public void UpdateRole(Role role)
        {
            this.AsUpdateable(role).ExecuteCommand();
        }

        #endregion
    }
}
