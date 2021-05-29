using System;
using System.Collections.Generic;
using System.Text;
using tdb.account.dto.Common;
using tdb.account.dto.Role;
using tdb.framework.webapi.DTO;
using tdb.framework.webapi.IocAutofac;

namespace tdb.account.ibll
{
    /// <summary>
    /// 角色
    /// </summary>
    public interface IRoleBLL : IAutofacDependency
    {
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="req">角色信息</param>
        /// <param name="oper">操作者信息</param>
        BaseItemRes<bool> AddRole(AddRoleReq req, OperatorInfo oper);

        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="req">条件</param>
        /// <returns>角色信息</returns>
        RoleInfo GetRole(GetRoleReq req);

        /// <summary>
        /// 修改角色信息
        /// </summary>
        /// <param name="req">角色信息</param>
        /// <param name="oper">操作者信息</param>
        BaseItemRes<bool> UpdateRole(UpdateRoleReq req, OperatorInfo oper);

        /// <summary>
        /// 修改角色启用状态
        /// </summary>
        /// <param name="req">条件</param>
        /// <param name="oper">操作者信息</param>
        BaseItemRes<bool> UpdateRoleEnable(UpdateRoleEnableReq req, OperatorInfo oper);

    }
}
