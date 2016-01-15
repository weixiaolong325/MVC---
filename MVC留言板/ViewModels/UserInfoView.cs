using MVC留言板.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace MVC留言板.ViewModels
{
    public class UserInfoView
    {
        public UserInfo userinfo { get; set; }
        [Required(ErrorMessage="*验证码不能为空")]
        public string verifyCode { get; set; }
    }
}