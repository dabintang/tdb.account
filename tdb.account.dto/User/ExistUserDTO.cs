using System;
using System.Collections.Generic;
using System.Text;

namespace tdb.account.dto.User
{
    /// <summary>
    /// 用户是否存在
    /// </summary>
    public class ExistUserReq
    {
        /// <summary>
        /// 登录名
        /// </summary>           
        public string UserCode { get; set; }
    }
}
