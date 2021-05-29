using System;
using System.Collections.Generic;
using System.Text;
using tdb.framework.webapi.Validation.Attributes;

namespace tdb.account.dto.UserRole
{
    /// <summary>
    /// 用户角色信息
    /// </summary>
    public class UserRoleInfo
    {
        /// <summary>
        /// 登录名
        /// </summary>           
        public string LoginName { get; set; }

        /// <summary>
        /// 角色编码
        /// </summary>           
        public string RoleCode { get; set; }
    }
}
