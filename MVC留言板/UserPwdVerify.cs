using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC留言板
{
    public class UserPwdVerify : ValidationAttribute
    {
        //自定义验证
        protected  override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            foreach (char c in value.ToString())
            {
                if ((int)c > 127)
                    return new ValidationResult("*密码含有汉字或非法字符");
            }
            return ValidationResult.Success;
        }
    }
}