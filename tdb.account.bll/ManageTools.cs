using Autofac;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using tdb.account.common.Config;
using tdb.account.common.Const;
using tdb.account.dto.Enums;
using tdb.account.ibll;
using tdb.account.idal;
using tdb.account.model;
using tdb.common;
using tdb.framework.webapi.Config;
using tdb.framework.webapi.Log;

namespace tdb.account.bll
{
    /// <summary>
    /// 管理工具
    /// </summary>
    public class ManageTools : IManageTools
    {
        /// <summary>
        /// Autofac上下文
        /// </summary>
        private IComponentContext _componentContext;

        /// <summary>
        /// 用户
        /// </summary>
        private IUserDAL _userDAL
        {
            get
            {
                return this._componentContext.Resolve<IUserDAL>();
            }
        }

        /// <summary>
        /// 角色
        /// </summary>
        private IRoleDAL _roleDAL
        {
            get
            {
                return this._componentContext.Resolve<IRoleDAL>();
            }
        }

        /// <summary>
        /// 用户角色
        /// </summary>
        private IUserRoleDAL _userRoleDAL
        {
            get
            {
                return this._componentContext.Resolve<IUserRoleDAL>();
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="componentContext">Autofac上下文</param>
        public ManageTools(IComponentContext componentContext)
        {
            this._componentContext = componentContext;
        }

        #region 实现接口

        /// <summary>
        /// 备份consul配置
        /// </summary>
        /// <returns>备份配置完整文件名</returns>
        public string BackupConsulConfig()
        {
            var fullFileName = DistributedConfigurator.Ins.BackupConfig<ConsulConfig>();
            return fullFileName;
        }

        /// <summary>
        /// 还原consul配置
        /// </summary>
        /// <param name="file">配置文件（.json文件）</param>
        public void RestoreConsulConfig(IFormFile file)
        {
            using (var ms = file.OpenReadStream())
            {
                var buf = new byte[ms.Length];
                ms.Read(buf, 0, (int)ms.Length);

                var jsonTxt = Encoding.Default.GetString(buf);
                var config = JsonConvert.DeserializeObject<ConsulConfig>(jsonTxt);

                DistributedConfigurator.Ins.RestoreConfig<ConsulConfig>(config);
            }
        }

        /// <summary>
        /// 根据数据库表结构生成实体类
        /// </summary>
        public void CreateDBModels()
        {
            // 连接数据库
            var db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = AccConfig.Consul.DBConnStr,
                DbType = DbType.MySql,
                InitKeyType = InitKeyType.Attribute,//从特性读取主键和自增列信息
                IsAutoCloseConnection = true//开启自动释放模式
            });

            //表映射
            db.MappingTables.Add("LoginLog", "login_log");
            db.MappingTables.Add("User", "user");
            db.MappingTables.Add("Role", "role");
            db.MappingTables.Add("UserRole", "user_role");

            // 生成路径
            var classDirectory = @"..\..\..\..\tdb.account.model";
            // 实体命名空间
            string classNamespace = "tdb.account.model";
            // 生成实体类
            db.DbFirst.IsCreateAttribute().CreateClassFile(classDirectory, classNamespace);
        }

        /// <summary>
        /// 初始化数据库
        /// </summary>
        /// <returns>是否成功</returns>
        public bool InitDB()
        {
            try
            {
                //开始事物
                this._userDAL.AsTenant().BeginTran();

                #region 添加超级管理员账号

                var userSuperAdmin = new User();
                userSuperAdmin.UserCode = CstRole.SuperAdmin;
                userSuperAdmin.UserName = "超级管理员";
                userSuperAdmin.Password = EncryptHelper.Md5("tdb123");
                userSuperAdmin.Gender = (int)EnumGender.Unknown;
                userSuperAdmin.Birthday = null;
                userSuperAdmin.MobilePhone = "";
                userSuperAdmin.Email = "";
                userSuperAdmin.Enable = true;
                userSuperAdmin.Creater = "0";
                userSuperAdmin.CreateTime = DateTime.Now;
                userSuperAdmin.Updater = "0";
                userSuperAdmin.UpdateTime = DateTime.Now;

                if (this._userDAL.ExistUser(userSuperAdmin.UserCode) == false)
                {
                    this._userDAL.AddUser(userSuperAdmin);
                }

                #endregion

                #region 添加超级管理员角色

                var roleSuperAdmin = new Role();
                roleSuperAdmin.RoleCode = CstRole.SuperAdmin;
                roleSuperAdmin.RoleName = "超级管理员";
                roleSuperAdmin.Enable = true;
                roleSuperAdmin.Creater = "0";
                roleSuperAdmin.CreateTime = DateTime.Now;
                roleSuperAdmin.Updater = "0";
                roleSuperAdmin.UpdateTime = DateTime.Now;

                if (this._roleDAL.ExistRole(roleSuperAdmin.RoleCode) == false)
                {
                    this._roleDAL.AddRole(roleSuperAdmin);
                }

                #endregion

                #region 添加超级管理员账号角色

                var userRoleSuperAdmin = new UserRole();
                userRoleSuperAdmin.UserCode = userSuperAdmin.UserCode;
                userRoleSuperAdmin.RoleCode = roleSuperAdmin.RoleCode;
                userRoleSuperAdmin.Creater = "0";
                userRoleSuperAdmin.CreateTime = DateTime.Now;

                this._userRoleDAL.SetUserRole(userRoleSuperAdmin.UserCode, new List<UserRole>() { userRoleSuperAdmin });

                #endregion

                //提交事物
                this._userDAL.AsTenant().CommitTran();

                return true;
            }
            catch (Exception ex)
            {
                Logger.Ins.Error(ex, "初始化数据库异常");

                //回滚事物
                this._userDAL.AsTenant().RollbackTran();

                return false;
            }
        }

        /// <summary>
        /// 测试
        /// </summary>
        public void Test()
        {
            this._userDAL.AsTenant().BeginTran();

            var user1 = this._userDAL.GetUser("admin");
            user1.Email = "12345";
            this._userDAL.UpdateUser(user1);
            var user2 = this._userDAL.GetUser("admin");

            this._userDAL.AsTenant().RollbackTran();
        }

        #endregion
    }
}
