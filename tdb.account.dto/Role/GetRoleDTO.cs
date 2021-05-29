using System;
using System.Collections.Generic;
using System.Text;
using tdb.framework.webapi.Validation.Attributes;

namespace tdb.account.dto.Role
{
    /// <summary>
    /// 获取角色
    /// </summary>
    public class GetRoleReq
    {
        /// <summary>
        /// 角色编码
        /// </summary>
        [TdbRequired(ParamName = "角色编码")]
        public string RoleCode { get; set; }
    }
}
