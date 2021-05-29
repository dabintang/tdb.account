using System;
using System.Collections.Generic;
using System.Text;
using tdb.framework.webapi.Validation.Attributes;

namespace tdb.account.dto.User
{
    /// <summary>
    /// 更新用户信息
    /// </summary>
    public class UpdateUserReq : UpdateMyUserInfoReq
    {
        /// <summary>
        /// 登录名
        /// </summary>
        [TdbRequired(ParamName = "登录名")]
        public string LoginName { get; set; }
    }
}
