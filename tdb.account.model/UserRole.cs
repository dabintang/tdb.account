using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace tdb.account.model
{
    ///<summary>
    ///用户角色表
    ///</summary>
    [SugarTable("user_role")]
    public partial class UserRole
    {
           public UserRole(){


           }
           /// <summary>
           /// Desc:创建者
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Creater {get;set;}

           /// <summary>
           /// Desc:创建时间
           /// Default:
           /// Nullable:False
           /// </summary>           
           public DateTime CreateTime {get;set;}

           /// <summary>
           /// Desc:登录名
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public string LoginName {get;set;}

           /// <summary>
           /// Desc:角色编码
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public string RoleCode {get;set;}

    }
}
