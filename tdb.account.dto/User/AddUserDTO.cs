using System;
using System.Collections.Generic;
using System.Text;

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
        public string UserCode { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>           
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>           
        public string Password { get; set; }
    }
}
