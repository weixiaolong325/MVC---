using MVC留言板.Models;
using MVC留言板.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC留言板.Controllers
{
    public class person
    {
        public string name { get; set; }
        public int age { get; set; }

    }
    public class CommentController : Controller
    { 
        //
        // GET: /Comment/
        public ActionResult Index()
        {
            CommentView commentview = new CommentView();
            HttpCookie cookie = Request.Cookies.Get("Uid");
            //判断用户是否已经登录
            if (Session["Uid"] != null)
            {
                commentview.LoginState = "yes";
                commentview.Uid = Session["Uid"].ToString();
            }
            else if (cookie != null)
            {
                Session["Uid"] = cookie.Value;
                commentview.LoginState = "yes";
                commentview.Uid = Session["Uid"].ToString();
            }
            else
            {
                commentview.LoginState = "no";
                commentview.Uid = "未登陆";
            }
            return View(commentview);
        }
        //提交评论
        [HttpPost]
        public ActionResult Addcomment(FormCollection form)
        {
            try
            {
                CommentView cv = new CommentView();
                string jsonData=null;
                //1.先判断是否已经登陆
                if (Session["Uid"] == null)
                {
                    cv.LoginState = "no";
                    //序列化为jsonString
                     jsonData = jsonHelper.JsonSerializer<CommentView>(cv);
                    return Content(jsonData);
                }

                //获取ajax传过来的数据
                Comment c = new Comment();
                c.CommentContent = Request.Form["comment"];
                c.CommentID = Convert.ToInt32(Request.Form["commentID"]);
                c.CreateTime = DateTime.Now;
                c.UpTime = DateTime.Now;
                c.UserId = Session["Uid"].ToString();
                c.IsInitial = Convert.ToInt32(Request["isInitial"]);
                c.ReplyUserId =Convert.ToInt32(Request.Form["replyUserId"]);
                c.ReplyUserName = c.ReplyUserId.ToString();
                //2.评论内容是否为空
                if (string.IsNullOrEmpty(c.CommentContent.Trim()))
                {
                    cv.msg = "*评论内容不能为空";
                    //序列化
                     jsonData = jsonHelper.JsonSerializer<CommentView>(cv);
                    return Content(jsonData);
                }
                //3.评论内容是否超过200字
                if(c.CommentContent.Length>200)
                {
                    cv.msg = "*评论内容不能超过200字";
                    //序列化
                    jsonData = jsonHelper.JsonSerializer<CommentView>(cv);
                    return Content(jsonData);
                }

                //3.将评论内容插入数据库
                 CommentLayer.CommentAdd(c);
                cv.comment = c;
                //序列化
                jsonData = jsonHelper.JsonSerializer<CommentView>(cv);
                return Content(jsonData);
            }
            catch
            {
                return RedirectToAction("Index");
            }
            
        }

    }
}
