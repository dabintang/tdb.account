using System;
using System.Collections.Generic;
using System.Text;

namespace tdb.account.dto.Common
{
    /// <summary>
    /// 操作人信息
    /// </summary>
    public class OperatorInfo
    {
        /// <summary>
        /// 登录名
        /// </summary>           
        public string UserCode { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>           
        public string UserName { get; set; }
    }
}
