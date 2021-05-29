using System;
using System.Collections.Generic;
using System.Text;
using tdb.framework.webapi.Validation.Attributes;

namespace tdb.account.dto.User
{
    /// <summary>
    /// 登录
    /// </summary>
    public class LoginReq
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

        /// <summary>
        /// 服务编码
        /// </summary>
        [TdbNotNull(ParamName = "服务编码")]
        public string ServiceCode { get; set; }
    }
}
