using System;
using System.Collections.Generic;
using System.Text;

namespace tdb.account.dto.User
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 登录名
        /// </summary>           
        public string LoginName { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 性别（0：未知；1：男；2：女）
        /// </summary>           
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

        /// <summary>
        /// 是否启用
        /// </summary>           
        public bool Enable { get; set; }
    }
}
