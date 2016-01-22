using MVC留言板.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC留言板.ViewModels
{
    public class CommentView
    {
        //登陆状态
        public string LoginState { get; set; }
        //用户ID
        public string Uid { get; set; }
        //评论内容
        public Comment comment { get; set; }
        //消息
        public string msg { get; set; }
    }
}