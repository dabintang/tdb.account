using System;
using System.Collections.Generic;
using System.Text;
using tdb.framework.webapi.Validation.Attributes;

namespace tdb.account.dto.UserRole
{
    /// <summary>
    /// 设置用户角色
    /// </summary>
    public class SetUserRoleReq
    {
        /// <summary>
        /// 登录名
        /// </summary>
        [TdbRequired(ParamName = "登录名")]

        public string LoginName { get; set; }

        /// <summary>
        /// 角色编码集合
        /// </summary>
        public List<string> LstRoleCode { get; set; }
    }
}
