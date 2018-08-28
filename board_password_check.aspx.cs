using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class board_password_check : Webbase
{
    string idx, target, function, board_id, input;

    protected void Page_Load(object sender, EventArgs e)
    {
        idx = Request.QueryString["idx"] == null ? "" : CheckNumber((Request.QueryString["idx"]).ToString());
        target = Request.QueryString["target"] == null ? "" : (Request.QueryString["target"]).ToString();
        function = Request.QueryString["function"] == null ? "" : (Request.QueryString["function"]).ToString();
        board_id = Request.QueryString["board_id"] == null ? "" : CheckNumber((Request.QueryString["board_id"]).ToString());

        if (!IsPostBack)
        {
            // postBack이 아닌 경우
            inputPassword.Focus();
        }
        
    }

    protected void InputPassword_TextChanged(object sender, EventArgs e)
    {

        input = inputPassword.Text;

        input = SHA256(input);
        //Response.Write("<script>alert('" + input + "');</script>");

        switch (target)
        {
            case "board":
                targetBoard();
                break;
            case "comment":
                targetComment();
                break;
        }
    }

    protected void targetBoard()
    {
        Database db = new Database();

        string result = db.GetBoardPassword(idx);

        if(result == input)
        {
            if(function == "revise")
            {
                // string url = "board_revise.aspx?idx=" + idx;
                // justGo(url);

                Session["idx"] = idx;
               
                justGo("board_revise.aspx?idx="+idx);
            } else if (function == "delete")
            {
                db.DeleteBoard(idx);
                alertGo("게시글이 삭제되었습니다.", "board_list_view.aspx");
            }
        } else
        {
            alertGo("비밀번호가 일치하지 않습니다.", "board_detail.aspx?idx=" + idx);
        }

    }

    protected void targetComment()
    {
        Database db = new Database();
        string result = db.GetCommentPassword(idx);

        if (result == input)
        {
            if (function == "revise")
            {
                Session["commentPwdChk"] = "true";
                Session["comentIdx"] = idx;
                //string url = "comment_revise.aspx?idx=" + idx+"&board_id="+board_id;
                string url = "board_detail.aspx?idx=" + board_id;
                justGo(url);
            }
            else if (function == "delete")
            {
                db.DeleteComment(idx);
                alertGo("댓글이 삭제되었습니다.", "board_detail.aspx?idx="+board_id);
            }
        }
        else
        {
            alertGo("비밀번호가 일치하지 않습니다.", "board_detail.aspx?idx=" + board_id);
        }
    }


}