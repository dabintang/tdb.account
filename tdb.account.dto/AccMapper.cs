using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using tdb.account.dto.Role;
using tdb.account.dto.User;

namespace tdb.account.dto
{
    /// <summary>
    /// 类型映射
    /// </summary>
    public class AccMapper
    {
        /// <summary>
        /// 类型映射
        /// </summary>
        private static IMapper _mapper = null;

        /// <summary>
        /// 缓存实例
        /// </summary>
        public static IMapper Ins
        {
            get
            {
                if (_mapper == null)
                {
                    _mapper = Initialize();
                }

                return _mapper;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private static IMapper Initialize()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                #region 映射

                cfg.CreateMap<tdb.account.model.User, UserInfo>();
                cfg.CreateMap<UpdateMyUserInfoReq, UpdateUserReq>();
                cfg.CreateMap<tdb.account.model.Role, RoleInfo>();

                #endregion
            });

            // only during development, validate your mappings; remove it before release
            //configuration.AssertConfigurationIsValid();

            // use DI (http://docs.automapper.org/en/latest/Dependency-injection.html) or create the mapper yourself
            return configuration.CreateMapper();
        }
    }
}
