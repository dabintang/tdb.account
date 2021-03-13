using System;
using System.Collections.Generic;
using System.Text;

namespace tdb.account.dto.User
{
    /// <summary>
    /// 获取用户信息
    /// </summary>
    public class GetUserReq
    {
        /// <summary>
        /// 登录名
        /// </summary>           
        public string UserCode { get; set; }
    }
}
