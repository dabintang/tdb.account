using System;
using System.Collections.Generic;
using System.Text;
using tdb.framework.webapi.Validation.Attributes;

namespace tdb.account.dto.User
{
    /// <summary>
    /// 更新自己的用户信息
    /// </summary>
    public class UpdateMyUserInfoReq
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [TdbRequired(ParamName = "用户名")]
        [TdbStringLength(64, ParamName = "用户名")]
        public string UserName { get; set; }

        /// <summary>
        /// 性别（0：未知；1：男；2：女）
        /// </summary>
        [TdbRegex(RegexText = "^[012]$", ParamName = "性别", ErrMsg = "请传入正确的性别")]
        public int Gender { get; set; }

        /// <summary>
        /// 生日
        /// </summary>           
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [TdbStringLength(16, ParamName = "手机号码")]
        [TdbNotNull(ParamName = "手机号码")]
        public string MobilePhone { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        [TdbStringLength(128, ParamName = "电子邮箱")]
        [TdbNotNull(ParamName = "电子邮箱")]
        public string Email { get; set; }
    }
}
