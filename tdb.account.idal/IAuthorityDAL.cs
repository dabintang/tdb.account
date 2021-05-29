using System;
using System.Collections.Generic;
using System.Text;
using tdb.account.model;

namespace tdb.account.idal
{
    /// <summary>
    /// 权限
    /// </summary>
    public interface IAuthorityDAL : IBaseDAL
    {
        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="role">权限信息</param>
        void AddAuthority(Authority authority);

        /// <summary>
        /// 检查权限是否存在
        /// </summary>
        /// <param name="authorityCode">权限编码</param>
        /// <returns>true：已存在；false：不存在</returns>
        bool ExistAuthority(string authorityCode);

    }
}
