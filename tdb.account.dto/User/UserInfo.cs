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
        public string UserCode { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>           
        public string UserName { get; set; }

        /// <summary>
        /// 是否管理员
        /// </summary>           
        public bool IsAdmin { get; set; }
    }
}
