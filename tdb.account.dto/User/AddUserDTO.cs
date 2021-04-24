using System;
using System.Collections.Generic;
using System.Text;
using tdb.framework.webapi.Validation.Attributes;

namespace tdb.account.dto.User
{
    /// <summary>
    /// 添加用户
    /// </summary>
    public class AddUserReq
    {
        /// <summary>
        /// 登录名
        /// </summary>
        [TdbRequired(ParamName = "登录名")]
        public string UserCode { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [TdbRequired(ParamName = "用户名")]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [TdbRequired(ParamName = "密码")]
        public string Password { get; set; }

        /// <summary>
        /// 性别（0：未知；1：男；2：女）
        /// </summary>   
        [TdbRegex(RegexText = "^[012]$", ParamName = "性别", ErrMsg="请传入正确的性别")]
        public int Gender { get; set; }

        /// <summary>
        /// 生日
        /// </summary>           
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>           
        public string MobilePhone { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>           
        public string Email { get; set; }
    }
}
