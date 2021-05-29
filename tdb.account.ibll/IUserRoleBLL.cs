using System;
using System.Collections.Generic;
using System.Text;
using tdb.account.dto.Common;
using tdb.account.dto.UserRole;
using tdb.framework.webapi.DTO;
using tdb.framework.webapi.IocAutofac;

namespace tdb.account.ibll
{
    /// <summary>
    /// 用户角色
    /// </summary>
    public interface IUserRoleBLL : IAutofacDependency
    {
        /// <summary>
        /// 设置指定用户角色
        /// </summary>
        /// <param name="req">条件</param>
        /// <param name="oper">操作者信息</param>
        /// <returns></returns>
        BaseItemRes<bool> SetUserRole(SetUserRoleReq req, OperatorInfo oper);
    }
}
