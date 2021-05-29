using System;
using System.Collections.Generic;
using System.Text;
using tdb.account.idal;
using tdb.account.model;

namespace tdb.account.dal
{
    /// <summary>
    /// 权限
    /// </summary>
    public class AuthorityDAL : Repository<Authority>, IAuthorityDAL
    {
        #region 实现接口

        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="role">权限信息</param>
        public void AddAuthority(Authority authority)
        {
            this.AsInsertable(authority).ExecuteCommand();
        }

        /// <summary>
        /// 检查权限是否存在
        /// </summary>
        /// <param name="authorityCode">权限编码</param>
        /// <returns>true：已存在；false：不存在</returns>
        public bool ExistAuthority(string authorityCode)
        {
            var count = this.AsQueryable().Count(m => m.AuthorityCode == authorityCode);
            return count > 0;
        }

        #endregion
    }
}
