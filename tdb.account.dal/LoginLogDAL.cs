using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tdb.account.idal;
using tdb.account.model;

namespace tdb.account.dal
{
    /// <summary>
    ///  登录日志
    /// </summary>
    public class LoginLogDAL : DBContext, ILoginLogDAL
    {
        #region 实现接口

        /// <summary>
        /// 添加登录日志（异步）
        /// </summary>
        /// <param name="log">登录日志信息</param>
        /// <returns>主键ID</returns>
        public async Task<long> AddLoginLogAsync(LoginLog log)
        {
            var id = await this.DB.Insertable(log).ExecuteReturnBigIdentityAsync();
            return id;
        }

        #endregion
    }
}
