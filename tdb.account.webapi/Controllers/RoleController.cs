//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using tdb.account.common.Const;
//using tdb.account.dto.Role;
//using tdb.account.ibll;
//using tdb.framework.webapi.APILog;
//using tdb.framework.webapi.DTO;
//using tdb.framework.webapi.Log;

//namespace tdb.account.webapi.Controllers
//{
//    /// <summary>
//    /// 角色
//    /// </summary>
//    public class RoleController : BaseController
//    {
//        /// <summary>
//        /// 角色
//        /// </summary>
//        private readonly IRoleBLL roleBLL;

//        /// <summary>
//        /// 构造函数
//        /// </summary>
//        /// <param name="_roleBLL">角色</param>
//        public RoleController(IRoleBLL _roleBLL)
//        {
//            this.roleBLL = _roleBLL;
//        }

//        #region 接口

//        /// <summary>
//        /// 添加角色
//        /// </summary>
//        /// <param name="req">角色信息</param>
//        /// <returns>是否添加成功</returns>
//        [HttpPost]
//        [Authorize(Roles = CstRole.SuperAdmin)]
//        [APILog(Level = EnumLogLevel.Info)]
//        public BaseItemRes<bool> AddRole([FromBody] AddRoleReq req)
//        {
//            return this.roleBLL.AddRole(req, this.CurUser);
//        }

//        /// <summary>
//        /// 获取角色信息
//        /// </summary>
//        /// <param name="req">条件</param>
//        /// <returns>用户信息</returns>
//        [HttpGet]
//        [Authorize(Roles = CstRole.SuperAdmin)]
//        public BaseItemRes<RoleInfo> GetRole([FromQuery] GetRoleReq req)
//        {
//            var roleInfo = this.roleBLL.GetRole(req);
//            return BaseItemRes<RoleInfo>.Ok(roleInfo);
//        }

//        /// <summary>
//        /// 修改角色信息
//        /// </summary>
//        /// <param name="req">角色信息</param>
//        /// <returns>是否更新成功</returns>
//        [HttpPost]
//        [Authorize(Roles = CstRole.SuperAdmin)]
//        [APILog(Level = EnumLogLevel.Info)]
//        public BaseItemRes<bool> UpdateRole([FromBody] UpdateRoleReq req)
//        {
//            return this.roleBLL.UpdateRole(req, this.CurUser);
//        }

//        /// <summary>
//        /// 修改角色启用状态
//        /// </summary>
//        /// <param name="req">用户信息</param>
//        /// <returns>是否更新成功</returns>
//        [HttpPost]
//        [Authorize(Roles = CstRole.SuperAdmin)]
//        [APILog(Level = EnumLogLevel.Info)]
//        public BaseItemRes<bool> UpdateRoleEnable([FromBody] UpdateRoleEnableReq req)
//        {
//            return this.roleBLL.UpdateRoleEnable(req, this.CurUser);
//        }

//        #endregion
//    }
//}
