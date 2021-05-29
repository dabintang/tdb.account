using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tdb.account.webapi.AuthPolicys
{
    /// <summary>
    /// 权限要求
    /// </summary>
    public class AuthorityRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// 权限编码
        /// </summary>
        public string AuthorityCode { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="authorityCode">权限编码</param>
        public AuthorityRequirement(string authorityCode)
        {
            this.AuthorityCode = authorityCode;
        }
    }
}
