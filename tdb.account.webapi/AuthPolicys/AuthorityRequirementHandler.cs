using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tdb.account.idal;
using tdb.account.model;
using tdb.framework.webapi.Auth;
using tdb.framework.webapi.IocAutofac;

namespace tdb.account.webapi.AuthPolicys
{
    /// <summary>
    /// 权限要求处理
    /// </summary>
    public class AuthorityRequirementHandler : AuthorizationHandler<AuthorityRequirement>, IAutofacDependency
    {
        /// <summary>
        /// 角色权限
        /// </summary>
        private readonly IRoleAuthorityDAL roleAuthorityDAL;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_roleAuthorityDAL">角色权限</param>
        public AuthorityRequirementHandler(IRoleAuthorityDAL _roleAuthorityDAL)
        {
            this.roleAuthorityDAL = _roleAuthorityDAL;
        }

        /// <summary>
        /// 处理
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorityRequirement requirement)
        {
            //获取角色
            var lstRoleClaim = context.User.FindAll(TdbClaimTypes.Role);
            var lstRoleCode = lstRoleClaim.Select(m => m.Value).ToList();

            //获取权限
            //var lstRoleAuthority = this.roleAuthorityDAL.QueryRoleAuthority(lstRoleCode);
            var lstRoleAuthority = new List<RoleAuthority>();
            foreach (var roleCode in lstRoleCode)
            {
                lstRoleAuthority.AddRange(this.roleAuthorityDAL.QueryRoleAuthority(roleCode));
            }

            //判断是否有权限
            if (lstRoleAuthority.Exists(m => requirement.AuthorityCode.Equals(m.AuthorityCode, StringComparison.OrdinalIgnoreCase)))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
