using System;
using System.Collections.Generic;
using System.Text;
using tdb.account.dto.Common;
using tdb.common;

namespace tdb.account.bll
{
    /// <summary>
    /// 业务逻辑层基类
    /// </summary>
    public class BaseBLL
    {
        #region 设置字段

        /// <summary>
        /// 创建者字段名
        /// </summary>
        protected const string FieldName_Creater = "Creater";

        /// <summary>
        /// 创建时间字段名
        /// </summary>
        protected const string FieldName_CreateTime = "CreateTime";

        /// <summary>
        /// 更新者字段名
        /// </summary>
        protected const string FieldName_Updater = "Updater";

        /// <summary>
        /// 更新时间字段名
        /// </summary>
        protected const string FieldName_UpdateTime = "UpdateTime";

        /// <summary>
        /// 设置创建者/更新者字段值
        /// </summary>
        /// <param name="model">表对象</param>
        /// <param name="oper">操作者信息</param>
        protected void SetCreateUpdateFields<T>(T model, OperatorInfo oper) where T : class
        {
            //设置创建者字段值
            this.SetCreateFields(model, oper);

            //设置更新者字段值
            this.SetUpdateFields(model, oper);
        }

        /// <summary>
        /// 设置创建者字段值
        /// </summary>
        /// <param name="model">表对象</param>
        /// <param name="oper">操作者信息</param>
        protected void SetCreateFields<T>(T model, OperatorInfo oper) where T : class
        {
            //创建者
            if (CommHelper.IsExistProperty(model, FieldName_Creater))
            {
                CommHelper.EmitSet(model, FieldName_Creater, oper.LoginName);
            }

            //创建时间
            if (CommHelper.IsExistProperty(model, FieldName_CreateTime))
            {
                CommHelper.EmitSet(model, FieldName_CreateTime, DateTime.Now);
            }
        }

        /// <summary>
        /// 设置更新者字段值
        /// </summary>
        /// <param name="model">表对象</param>
        /// <param name="oper">操作者信息</param>
        protected void SetUpdateFields<T>(T model, OperatorInfo oper) where T : class
        {
            //更新者
            if (CommHelper.IsExistProperty(model, FieldName_Updater))
            {
                CommHelper.EmitSet(model, FieldName_Updater, oper.LoginName);
            }

            //更新时间
            if (CommHelper.IsExistProperty(model, FieldName_UpdateTime))
            {
                CommHelper.EmitSet(model, FieldName_UpdateTime, DateTime.Now);
            }
        }

        #endregion

    }
}
