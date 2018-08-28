using System;
using System.Configuration;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class board_revise : Webbase
{
    public string temp, idx, title, name, content, writedate, pwd;

    protected void Page_Load(object sender, EventArgs e)
    {
        revise_complete.ServerClick += new EventHandler(revise_complete_ServerClick);

        HttpContext context = HttpContext.Current;
        idx = Request.QueryString["idx"];

        temp = Session["idx"] == null ? "/" : (string)Session["idx"];
    // postBack이 아닌 경우에만 읽어와서 뿌림
    // button click 이벤트 실행 전 page load 하기 때문
            Database db = new Database();
            DataTable dt = new DataTable();

            dt = db.BoardDetail(idx);

            title = (dt.Rows[0])["title"].ToString();
            name = (dt.Rows[0])["name"].ToString();
            content = (dt.Rows[0])["contents"].ToString();
            writedate = (dt.Rows[0])["writedate"].ToString();

            //boardTitle.Value = title;
            //contents.Value = content;

            db.DBdispose();

    }
    protected void revise_cancel_ServerClick(object source, EventArgs e)
    {
        alertGo("게시글 수정을 취소하였습니다.", "board_detail.aspx?idx=" + idx);
    }
    
    protected void revise_complete_ServerClick(object sender, EventArgs e)
    {
        string idx = Request.QueryString["idx"] == null ? "" : Request.QueryString["idx"].ToString();
        string title = (Request.Form["boardTitle"] == null) ? "" : Request.Form["boardTitle"].ToString();
        string content = (Request.Form["contents"] == null) ? "" : Request.Form["contents"].ToString();
        string pwd = (Request.Form["password"] == null) ? "" : Request.Form["password"].ToString();

        bool checkBlankResult = true;

        if (title == "")
        {
            string message = "제목을 입력하세요.";
            Response.Write("<script>alert(\"" + message + "\");</script>");
            //boardTitle.Focus();
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
           // password.Focus();
            checkBlankResult = false;
            return;
        }

        if (checkBlankResult)
        {
            // DB에 작성한 내용 저장
            Database db = new Database();

            db.ReviseBoard(idx, stringToQuery(title), stringToQuery(content), stringToQuery(SHA256(pwd)));

            //Session.Remove("idx");
            alertGo("게시글 수정을 완료하였습니다.", "board_detail.aspx?idx=" + idx);
        }
    }
}