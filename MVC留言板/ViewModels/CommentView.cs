using MVC留言板.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC留言板.ViewModels
{
    public class CommentView
    {
        public string LoginState { get; set; }

        public Comment comment { get; set; }
    }
}