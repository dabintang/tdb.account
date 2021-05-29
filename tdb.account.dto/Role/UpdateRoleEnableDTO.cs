using System;
using System.Collections.Generic;
using System.Text;
using tdb.framework.webapi.Validation.Attributes;

namespace tdb.account.dto.Role
{
    /// <summary>
    /// 修改角色启用状态
    /// </summary>
    public class UpdateRoleEnableReq
    {
        /// <summary>
        /// 角色编码
        /// </summary>
        [TdbRequired(ParamName = "角色编码")]
        public string RoleCode { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>           
        public bool Enable { get; set; }
    }
}
