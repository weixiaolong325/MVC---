using MVC留言板.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC留言板.Controllers
{
    public class CommentController : Controller
    {
        //
        // GET: /Comment/

        public ActionResult Index()
        {
            CommentView commentview = new CommentView();
            //判断用户是否已经登陆
            if (Session["UserName"] != null)
            {
                commentview.LoginState = Session["UserName"].ToString();
            }
            else commentview.LoginState = "未登陆";
            return View(commentview);
        }

    }
}
