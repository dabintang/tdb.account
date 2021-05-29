using System;
using System.Collections.Generic;
using System.Text;

namespace tdb.account.dto.Common
{
    /// <summary>
    /// 初始化数据信息
    /// </summary>
    public class InitDataInfo
    {
        /// <summary>
        /// 用户
        /// </summary>
        public List<tdb.account.model.User> Users { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public List<tdb.account.model.Role> Roles { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        public List<tdb.account.model.Authority> Authoritys { get; set; }

        /// <summary>
        /// 角色权限
        /// </summary>
        public List<tdb.account.model.RoleAuthority> RoleAuthoritys { get; set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        public List<tdb.account.model.UserRole> UserRoles { get; set; }
    }
}
