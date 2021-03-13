using System;
using System.Collections.Generic;
using System.Text;
using tdb.account.common.Config;
using tdb.framework.webapi.DTO;

namespace tdb.account.common
{
    /// <summary>
    /// 通用帮助类
    /// </summary>
    public class AccHelper
    {
        #region 回报消息

        /// <summary>
        /// 失败消息
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="data">结果</param>
        /// <returns></returns>
        public static BaseItemRes<T> FailItemRes<T>(MsgInfo msg, T data)
        {
            var res = new BaseItemRes<T>(false, msg.MsgID, msg.Msg, data);
            return res;
        }

        /// <summary>
        /// 成功消息
        /// （IsOK=true,MsgID="OK",Msg="成功"）
        /// </summary>
        /// <param name="data">结果</param>
        /// <returns></returns>
        public static BaseItemRes<T> OkItemRes<T>(T data)
        {
            var res = BaseItemRes<T>.Ok(data);
            return res;
        }

        /// <summary>
        /// 成功消息
        /// （IsOK=true,MsgID="OK",Msg="成功"）
        /// </summary>
        /// <param name="data">结果</param>
        /// <param name="totalRecord">总数</param>
        /// <returns></returns>
        public static BasePageRes<T> OkPageRes<T>(List<T> data, int totalRecord)
        {
            var res = BasePageRes<T>.Ok(data, totalRecord);
            return res;
        }

        #endregion
    }
}
