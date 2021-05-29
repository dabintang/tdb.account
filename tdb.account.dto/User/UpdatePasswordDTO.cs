using System;
using System.Collections.Generic;
using System.Text;
using tdb.framework.webapi.Validation.Attributes;

namespace tdb.account.dto.User
{
    /// <summary>
    /// 修改密码
    /// </summary>
    public class UpdatePasswordReq
    {
        /// <summary>
        /// 登录名
        /// </summary>
        [TdbRequired(ParamName = "登录名")]
        public string LoginName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [TdbRequired(ParamName = "密码")]
        public string Password { get; set; }
    }
}
