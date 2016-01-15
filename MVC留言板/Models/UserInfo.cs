using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace MVC留言板.Models
{
    public class UserInfo
    {
        //用户ID
        public int ID { get; set; }
        //用户名
        // [UserPwdVerify]
        [Required(ErrorMessage="*用户名不能为空")]
        [StringLength(10,ErrorMessage="*用户名不能超过10位")]
        public string UserName { get; set; }
        //用户密码
        [Required(ErrorMessage="*密码不能为空")]
        [StringLength(15,ErrorMessage="*密码不能超过15位")]
        [UserPwdVerify]  //自定义验证
        public string UserPwd { get; set; }
        //用户创建时间
        public DateTime CreateTime { get; set; }
    }
}