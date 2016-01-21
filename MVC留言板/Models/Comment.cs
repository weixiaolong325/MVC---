using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace MVC留言板.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpTime { get; set; }

        public int IsDel { get; set; }

        public string CommentContent { get; set; }

        public int CommentID { get; set; }

        public string UserId { get; set; }

        public int IsInitial { get; set; }

        public int ReplyUserId { get; set; }

        public string ReplyUserName { get; set; }

    }
}