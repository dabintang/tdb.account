using System;
using System.Collections.Generic;
using System.Text;
using tdb.framework.webapi.Validation.Attributes;

namespace tdb.account.dto.User
{
    /// <summary>
    /// 更新用户启用状态
    /// </summary>
    public class UpdateUserEnableReq
    {
        /// <summary>
        /// 登录名
        /// </summary>
        [TdbRequired(ParamName = "登录名")]
        public string LoginName { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>           
        public bool Enable { get; set; }
    }
}
