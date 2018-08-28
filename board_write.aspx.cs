using System;
using System.Configuration;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class board_write : Webbase
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void write_complete_Click(object source, EventArgs e)
    {
        bool checkBlankResult=true;
        string title = (boardTitle.Value).Trim();
        string name = (boardWriter.Value).Trim();
        string content = (contents.Value).Trim();
        string pwd = (password.Value).Trim();

        if (title == "")
        {
            string message = "제목을 입력하세요.";
            Response.Write("<script>alert(\"" + message + "\");</script>");
            boardTitle.Focus();
            checkBlankResult = false;
            return;
        }




        if (name == "")
        {
            string message = "작성자명을 입력하세요.";
            Response.Write("<script>alert(\"" + message + "\");</script>");
            boardWriter.Focus();
            checkBlankResult = false;
            return;
        }

        if (content == "")
        {
            string message = "내용을 입력하세요.";
            Response.Write("<script>alert(\"" + message + "\");</script>");
            checkBlankResult = false;
            return;
        }

        if (pwd == "")
        {
            string message = "비밀번호를 입력하세요.";
            Response.Write("<script>alert(\"" + message + "\");</script>");
            password.Focus();
            checkBlankResult = false;
            return;
        }

        if (checkBlankResult)
        {
            // DB에 작성한 내용 저장
           Database db = new Database();
            string sha256Pwd = SHA256(pwd);

            
            db.BoardWrite(stringToQuery(title), stringToQuery(name), stringToQuery(content), stringToQuery(sha256Pwd));

            alertGo("게시글 작성을 완료하였습니다.", "board_list_view.aspx");
        }

    }
   
    protected void write_cancel_Click(object source, EventArgs e)
    {
        alertGo("게시글 작성을 취소하였습니다.", "board_list_view.aspx");
    }

}