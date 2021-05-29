using System;
using System.Collections.Generic;
using System.Text;
using tdb.account.model;

namespace tdb.account.idal
{
    /// <summary>
    /// 角色权限
    /// </summary>
    public interface IRoleAuthorityDAL : IBaseDAL
    {
        /// <summary>
        /// 设置角色权限
        /// </summary>
        /// <param name="roleCode">角色编码</param>
        /// <param name="lstRoleAuthority">角色权限</param>
        bool SetRoleAuthority(string roleCode, List<RoleAuthority> lstRoleAuthority);

        /// <summary>
        /// 查询指定角色的权限
        /// </summary>
        /// <param name="roleCode">角色编码</param>
        /// <returns>角色权限</returns>
        List<RoleAuthority> QueryRoleAuthority(string roleCode);

        ///// <summary>
        ///// 查询指定角色的权限
        ///// </summary>
        ///// <param name="lstRoleCode">角色编码</param>
        ///// <returns>角色权限</returns>
        //List<RoleAuthority> QueryRoleAuthority(List<string> lstRoleCode);
    }
}
