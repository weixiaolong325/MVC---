$(document).ready(function () {
    //提交评论
    $("#submitComment").click(function () {
        var jsonData = {
            comment: $("#txtarea").val(),
            commentID: 0,
            isInitial: 1,
            replyUserId: 0,
        }
        $.post("Comment/Addcomment", jsonData, function (data, status) {
            //把json字符串转化为json对象
            var obj = eval('(' + data + ')')
            //未登录就跳转到登录页面
            if (obj.LoginState == 'no')
                window.location = "Users/Login?redirectUrl=" + window.location.href;
                //遇到错误消息
            else if (obj.msg != null) {
                $("#msg").text(obj.msg);
                $("#msg").attr("class", "msgerror");
            }
            else {
                $("#txtarea").val("");
                $("#msg").text("评论成功");
                $("#msg").attr("class", "msgsucceed");
            }

        })
    })

})
var isclick = false;
//点击回复弹出回复框
function Reply(e) {
    var p = $(e).parent().parent();
    var pClass = p.attr("class");
    //回复评论块
    var $reply = $(" <div id='ReplyContent'><div><textarea id='txtReplyContent'></textarea></div><div> <input type='button' id='replybtn' class='btnDefult' value='提交评论' /></div></div>");
    //先删除
    $reply.remove();
    //回复父评论
    if (pClass == "contentParent")
    {
        p.parent().append($reply);
    }
     //回复子评论
    else if (pClass == "contentchild")
    {
        p.append($reply);
    }
    //得到焦点
    $("#txtReplyContent").focus();

    //内容失去焦点就清除回复评论块
    $("#txtReplyContent").blur(function (ev) {
       // alert(isclick);
        $reply.remove();
    })
    //提交回复
   sbReply();
}
//提交回复

function sbReply() {
    $("#replybtn").click(function () {
        isclick = true;
        var jsonData = { comment: $("#txtReplyContent").val() };
        if (jsonData.comment == "") {
            alert('请输入回复内容');
        }
        else {
            alert(jsonData.comment);
        }
    })
}



