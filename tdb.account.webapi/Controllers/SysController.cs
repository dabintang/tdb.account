using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tdb.account.webapi.Controllers
{
    /// <summary>
    /// 通用
    /// </summary>
    public class SysController : BaseController
    {
        #region 接口

        /// <summary>
        /// 心跳检查
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult HealthCheck()
        {
            return Ok();
        }

        #endregion
    }
}
