using MVC留言板.Models;
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
        //[Authorize]
        public ActionResult Index()
        {
            CommentView commentview = new CommentView();
            //判断用户是否已经登录
            if (Session["Uid"] != null)
            {
                commentview.LoginState = Session["Uid"].ToString();
            }
            else commentview.LoginState = "未登录";
            return View(commentview);
        }
        //提交评论
       // 
        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            //先判断是否已经登陆
            if (Session["Uid"] == null)
            {
                //记录下跳转前的页面
                string url = Request.Url.ToString();
                Session["redirectUrl"] = url;

                return Content("请先登陆");
            }
            Comment comment = new Comment();
            //获取ajax传过来的数据
            comment.CommentContent = Request.Form["comment"];
            comment.CommentID = Convert.ToInt32(Request.Form["commentID"]);
            comment.UserId = Session["Uid"].ToString();
            if (string.IsNullOrEmpty(comment.CommentContent))
            {
                return RedirectToAction("Index");
            }
            var age = Request.Form["age"];
            if (comment.CommentContent != null)
            {
                return Content("成功");
            }
            else 
            return RedirectToAction("Index");
        }

    }
}
