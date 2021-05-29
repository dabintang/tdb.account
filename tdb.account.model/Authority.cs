using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace tdb.account.model
{
    ///<summary>
    ///权限表
    ///</summary>
    [SugarTable("authority")]
    public partial class Authority
    {
           public Authority(){


           }
           /// <summary>
           /// Desc:权限编码
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public string AuthorityCode {get;set;}

           /// <summary>
           /// Desc:权限名称
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string AuthorityName {get;set;}

           /// <summary>
           /// Desc:是否启用
           /// Default:
           /// Nullable:False
           /// </summary>           
           public bool Enable {get;set;}

           /// <summary>
           /// Desc:备注
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Remark {get;set;}

    }
}
