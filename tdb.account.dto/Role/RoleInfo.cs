using System;
using System.Collections.Generic;
using System.Text;

namespace tdb.account.dto.Role
{
    /// <summary>
    /// 角色信息
    /// </summary>
    public class RoleInfo
    {
        /// <summary>
        /// 角色编码
        /// </summary>           
        public string RoleCode { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>           
        public string RoleName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>           
        public string Remark { get; set; }

    }
}
