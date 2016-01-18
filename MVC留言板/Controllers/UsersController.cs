using MVC留言板.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Imaging;
using MVC留言板.ViewModels;

namespace MVC留言板.Controllers
{
    public class UsersController : Controller
    {
        //
        // GET: /Users/

        //注册
        public ActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignIn(UserInfoView userView)
        {
            //model是否通过验证
            if (ModelState.IsValid)
            {
                //验证码是否正确
                if (userView.verifyCode.Trim().ToUpper() != Session["verifyCode"].ToString().ToUpper())
                {
                    ModelState.AddModelError("verifyCodeError", "*验证码错误");
                    return View();
                }
                //用户是否已经存在
                UserInfo user = userView.userinfo;
                string sql_isExist = "select count(1) from Users where UserName=@UserName";
                SqlParameter[] param_isExist = { new SqlParameter("@UserName", user.UserName) };
                if (Convert.ToInt32(SqlHelper.ExecuteScalar(sql_isExist, CommandType.Text, param_isExist)) > 0)
                {
                    ModelState.AddModelError("msgUserName", "*用户名已被使用");
                    return View();
                }

                string sql = "insert into Users(UserName,UserPwd)values(@UserName,@UserPwd)";
                SqlParameter[] param ={new SqlParameter("@UserName",user.UserName),
                                         new SqlParameter("@UserPwd",user.UserPwd)};
                if (SqlHelper.ExecuteNonQuery(sql, CommandType.Text, param)<=0)
                {
                    ModelState.AddModelError("msg", "注册失败");
                    return View();
                }
                return RedirectToAction("SignInSuccess");
            }
            return View();
        }
        public ActionResult SignInSuccess()
        {
            return View();
        }
        //登录
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(UserInfo user)
        {
            //用户名密码model验证
            if (ModelState.IsValid)
            {
                //判断用户名密码是否正确
                string sql = "select count(1) from Users where UserName=@UserName and UserPwd=@UserPwd";
                SqlParameter[] param={new SqlParameter("@UserName",user.UserName),
                                        new SqlParameter("@UserPwd",user.UserPwd )};
                if(Convert.ToInt32(SqlHelper.ExecuteScalar(sql,CommandType.Text,param))!=1)
                {
                    ModelState.AddModelError("loginError", "*用户名或密码错误");
                }
                //用户名密码正确则保持用户登陆状态
                Session["UserName"] = user.UserName;
                return RedirectToAction("Index", "Comment");
            }
            return View();
        }
        //退出登录
        public ActionResult ExitLogin()
        {
            Session.Remove("UserName");
            return RedirectToAction("Index", "Comment");
        }
        //错误页
        public ActionResult Error()
        {
            return Content("访问内容不存在~");
        }
        //验证码
        public void VerifyCode()
        {
            //获得随机码
            string randCode = GetRandCode();
            Session["verifyCode"] = randCode;
            //生成验证码图像
            Bitmap bitmap = CreateVerifyCodeImage(randCode);
            //显示验证码
            if (bitmap != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bitmap.Save(ms, ImageFormat.Jpeg);
                    Response.ClearContent();
                    Response.ContentType = "image/Jpeg";
                    Response.BinaryWrite(ms.ToArray());
                    Response.Flush();
                    Response.End();
                }
            }
           
        }
        //生成随机码
        public string GetRandCode()
        {
            //生成随机数
            string RandCode = "";
            StringBuilder stringbd = new StringBuilder();
            //添加数字1-9
            for (int i = 1; i <= 9; i++)
            {
                stringbd.Append(i.ToString());
            }
            //添加大写字母A-Z,不包括O
            char temp = ' ';
            for (int i = 0; i < 26; i++)
            {
                temp = Convert.ToChar(i + 65);
                if (!temp.Equals('O'))
                {
                    stringbd.Append(temp);
                }
            }
            //添加小写字母a-z,不包括o
            for (int i = 0; i < 26; i++)
            {
                temp = Convert.ToChar(i + 97);
                if (!temp.Equals('o'))
                {
                    stringbd.Append(temp);
                }
            }
            Random rd = new Random();
            //返回4位的随机码
            for (int i = 0; i < 4; i++)
            {
                RandCode += stringbd[rd.Next(0, stringbd.Length)];
            }
            return RandCode;
        }
       //生成验证码图像
        public Bitmap CreateVerifyCodeImage(string randCode)
        {
            Random rd = new Random();
            //创建绘图,宽度根据验证码长度改变，高度为24
            Bitmap bitmap = new Bitmap(randCode.Length * 18, 24);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;//图象质量
                //清除整个绘图面并以指定背景色填充
                g.Clear(Color.AliceBlue);
                //创建画笔
                using (SolidBrush brush = new SolidBrush(Color.Blue))
                {
                    //添加前景噪点
                    for (int i = 0; i < bitmap.Width * 2; i++) //多少个噪点
                    {
                        bitmap.SetPixel(rd.Next(bitmap.Width), rd.Next(bitmap.Height), Color.Blue);
                    }
                    //添加背景噪点
                    using (Pen pen = new Pen(Color.Azure, 0))
                    {
                        for (int i = 0; i < bitmap.Width * 2; i++)
                        {
                            g.DrawRectangle(pen, rd.Next(bitmap.Width), rd.Next(bitmap.Height), 1, 1);
                        }
                    }
                    //文字居中
                    StringFormat format = new StringFormat(StringFormatFlags.NoClip);
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                    //字体样式
                    Font font = new Font("Times New Roman", rd.Next(15, 18), FontStyle.Regular);
                    //验证码旋转，防止机器识别
                    char[] chars = randCode.ToCharArray();
                    for (int i = 0; i < chars.Length; i++)
                    {
                        //转动度数
                        float angle = rd.Next(-40, 40);
                        g.TranslateTransform(12, 12);
                        g.RotateTransform(angle);
                        g.DrawString(chars[i].ToString(), font, brush, -2, 2, format);
                        g.RotateTransform(-angle);
                        g.TranslateTransform(2, -12);
                    }
                }
            }
            return bitmap;
        }
    
    }
}
