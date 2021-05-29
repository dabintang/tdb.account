using System;
using System.Collections.Generic;
using System.Text;
using tdb.framework.webapi.Validation.Attributes;

namespace tdb.account.dto.Role
{
    /// <summary>
    /// 更新角色
    /// </summary>
    public class UpdateRoleReq
    {
        /// <summary>
        /// 角色编码
        /// </summary>
        [TdbRequired(ParamName = "角色编码")]
        public string RoleCode { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        [TdbRequired(ParamName = "角色名称")]
        [TdbStringLength(64, ParamName = "角色名称")]
        public string RoleName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [TdbStringLength(255, ParamName = "备注")]
        [TdbNotNull(ParamName = "备注")]
        public string Remark { get; set; }
    }
}
