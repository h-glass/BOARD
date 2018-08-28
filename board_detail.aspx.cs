using System;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.Services;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Collections;


public partial class board_detail : Webbase
{
    public string title, name, writedate, revisedate, content;
    public string idx = "";
    public string searchBase, searchValue;

    public int currentPage;

    public ArrayList dataList;

    //System.Windows.Forms.WebBrowser webBrowser;


    public string commentDetailIdx, commentReviseIdx, commentDeleteIdx;

    protected void Page_Load(object sender, EventArgs e)
    {
        idx = Request.QueryString["idx"] == null ? "" : CheckNumber(Request.QueryString["idx"].ToString());
        searchBase = Request.QueryString["searchBase"] == null ? "" : Request.QueryString["searchBase"].ToString();
        searchValue = Request.QueryString["searchValue"] == null ? "" : Request.QueryString["searchValue"].ToString();


        if (!String.IsNullOrEmpty(Request.QueryString["page"]))
        {
            currentPage = Int32.Parse(Request.QueryString["page"]);
        }
        else
        {
            currentPage = 1;
        }
        
            Database db = new Database();
            DataTable dt = new DataTable();

            dt = db.BoardDetail(idx);

            if (dt.Rows.Count == 0)
            {
                alertGo("잘못된 접근입니다.", "board_list_view.aspx");
            }

            title = (dt.Rows[0])["title"].ToString();
            name = (dt.Rows[0])["name"].ToString();
            content = (dt.Rows[0])["contents"].ToString();
            writedate = (dt.Rows[0])["writedate"].ToString();
            revisedate = (dt.Rows[0])["revisedate"].ToString();

  
            dt = db.CommentList(idx);

            commentListRepeater.DataSource = dt;
            commentListRepeater.DataBind();

            db.DBdispose();

    }

    protected void goToListView_ServerClick(object sender, EventArgs e)
    {
        justGo("board_list_view.aspx?page=" + currentPage + "&searchBase=" + searchBase + "&searchValue=" + searchValue);
    }



    protected void revise_ServerClick(object sender, EventArgs e)
    {
        string url = "board_password_check.aspx?idx=" + idx + "&target=board&function=revise";
        justGo(url);
    }

    protected void delete_ServerClick(object sender, EventArgs e)
    {
        string url = "board_password_check.aspx?idx=" + idx + "&target=board&function=delete";
        justGo(url);
    }

    protected void commentWriteComplete_ServerClick(object sender, EventArgs e)
    {
        bool checkBlankResult = true;
        string comWriter = commentWriter.Value;
        string comContents = commentContents.Value;
        string comPasswword = commentPassword.Value;

        if (comWriter == "")
        {
            string message = "작성자명을 입력하세요.";
            Response.Write("<script>alert(\"" + message + "\");</script>");
            commentWriter.Focus();
            checkBlankResult = false;
            return;
        }

        if (comContents == "")
        {
            string message = "내용을 입력하세요.";
            Response.Write("<script>alert(\"" + message + "\");</script>");
            commentContents.Focus();
            checkBlankResult = false;
            return;
        }

        if (comPasswword == "")
        {
            string message = "비밀번호를 입력하세요.";
            Response.Write("<script>alert(\"" + message + "\");</script>");
            commentPassword.Focus();
            checkBlankResult = false;
            return;
        }

        if (checkBlankResult)
        {
            // DB에 작성한 내용 저장
            Database db = new Database();
            db.CommentWrite(stringToQuery(idx), stringToQuery(comWriter), stringToQuery(comContents), stringToQuery(SHA256(comPasswword)));

            string message = "댓글 작성을 완료하였습니다.";
            Response.Write("<script>alert(\"" + message + "\"); </script>");
            Response.Write("<script>location.href = location.href; </script>");

        }
    }

    protected void commentReviseComplete_ServerClick(object sender, EventArgs e)
    {
        
    }

    protected void commentListRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {

    }

    // 댓글 비밀 번호 확인
    [WebMethod]
    public static bool checkPassword(string idx, string inputPassword)
    {
        bool check = false;
        string temp;

        Database db = new Database();

        temp = db.GetCommentPassword(idx);

        inputPassword = SHA256(inputPassword);

        if (temp == inputPassword)
        {
            check = true;
            //db.DeleteComment(idx);

        }
        else
        {
            check = false;
        }
        return check;
    }


    // 댓글 비밀 번호 확인
    [WebMethod]
    public static string _checkPassword(string idx, string inputPassword)
    {
        bool check = false;
        string temp;
        string _check;

        Database db = new Database();

        temp = db.GetCommentPassword(idx);

        inputPassword = SHA256(inputPassword);

        if (temp == inputPassword)
        {
            check = true;
            db.DeleteComment(idx);
            _check = "삭제 성공";
        }
        else
        {
            check = false;
            _check = "삭제 실패";
        }
        return _check;
    }

    [WebMethod]
    public static bool reviseComment(string idx, string content, string password)
    {
        
        Database db = new Database();

        db.ReviseComment(idx, content, SHA256(password));

        return true;
    }

    // 댓글 삭제
    [WebMethod]
    public static bool deleteComment(string idx)
    {
        Database db = new Database();

        db.DeleteComment(idx);



        return true;
    }

}