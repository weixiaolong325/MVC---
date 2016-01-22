using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace MVC留言板.Models
{
    public class CommentLayer
    {
        public static bool CommentAdd(Comment c)
        {
            bool result = false;
            string sql = "insert into Comment(CommentContent,CommentID,UserId,IsInitial,ReplyUserId,ReplyUserName)values(@CommentContent,@CommentID,@UserId,@IsInitial,@ReplyUserId,@ReplyUserName) ";
            SqlParameter[] param ={new SqlParameter("@CommentContent",c.CommentContent),
                                      new SqlParameter("@CommentID",c.CommentID),
                                      new SqlParameter("@UserId",c.UserId),
                                      new SqlParameter("@IsInitial",c.IsInitial),
                                      new SqlParameter("@ReplyUserId",c.ReplyUserId),
                                      new SqlParameter("@ReplyUserName",c.ReplyUserName)};
            if (SqlHelper.ExecuteNonQuery(sql, CommandType.Text, param) > 0)
            {
                result = true;
            }
            return result;
        }
    }
}