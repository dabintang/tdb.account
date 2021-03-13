using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tdb.account.common.Config;
using tdb.account.dal;
using tdb.account.ibll;
using tdb.framework.webapi.Config;
using tdb.framework.webapi.DTO;

namespace tdb.account.webapi.Controllers
{
    /// <summary>
    /// 管理工具
    /// </summary>
    public class ManageToolsController : BaseController
    {
        /// <summary>
        /// 初始化
        /// </summary>
        private readonly IInitBLL _initBLL;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="initBLL">初始化</param>
        public ManageToolsController(IInitBLL initBLL)
        {
            this._initBLL = initBLL;
        }

        #region 接口

        /// <summary>
        /// 还原consul配置
        /// </summary>
        /// <param name="file">配置文件（.json文件）</param>
        /// <returns>还原结果</returns>
        [HttpPost]
        [AllowAnonymous]
        public BaseItemRes<string> RestoreConfig(IFormFile file)
        {
            using (var ms = file.OpenReadStream())
            {
                var buf = new byte[ms.Length];
                ms.Read(buf, 0, (int)ms.Length);

                var jsonTxt = Encoding.Default.GetString(buf);
                var config = JsonConvert.DeserializeObject<ConsulConfig>(jsonTxt);

                DistributedConfigurator.Ins.RestoreConfig<ConsulConfig>(config);
                return BaseItemRes<string>.Ok("还原完成");
            }
        }

        /// <summary>
        /// 备份配置
        /// </summary>
        /// <param name="req">条件</param>
        /// <returns>备份配置完整文件名</returns>
        [HttpPost]
        [AllowAnonymous]
        public BaseItemRes<string> BackupConfig()
        {
            var data = DistributedConfigurator.Ins.BackupConfig<ConsulConfig>();
            return BaseItemRes<string>.Ok(data);
        }

        /// <summary>
        /// 根据数据库生成实体类
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public BaseItemRes<string> CreateDBModels()
        {
            // 连接数据库
            var db = new DBContext().DB;

            //表映射
            db.MappingTables.Add("LoginLog", "login_log");
            db.MappingTables.Add("User", "user");

            // 生成路径
            var classDirectory = @"..\..\..\..\tdb.account.model";
            // 实体命名空间
            string classNamespace = "tdb.account.model";
            // 生成实体类
            db.DbFirst.IsCreateAttribute().CreateClassFile(classDirectory, classNamespace);

            return BaseItemRes<string>.Ok("实体类已生成");
        }

        /// <summary>
        /// 初始化数据库等
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public BaseItemRes<string> Init()
        {
            this._initBLL.Init();

            return BaseItemRes<string>.Ok("初始化完成");
        }

        #endregion
    }
}
