using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace tdb.account.model
{
    ///<summary>
    ///角色权限表
    ///</summary>
    [SugarTable("role_authority")]
    public partial class RoleAuthority
    {
           public RoleAuthority(){


           }
           /// <summary>
           /// Desc:权限编码
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public string AuthorityCode {get;set;}

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
           /// Desc:角色编码
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public string RoleCode {get;set;}

    }
}
