using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;


public partial class board_list_view : Webbase
{
    public int pageSize = 10; // 1page - 10board
    public int currentPage = 1;
    public int pageCount, totalCount;

    public string page;

    public ArrayList dataList;

    // 검색 관련 변수
    public string searchBase, searchValue;
    public bool isSearch = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        string sql = "";
        string like = "";

        page = (Request.QueryString["page"] == null) ? "1" : Request.QueryString["page"].ToString();
        searchBase = (Request.QueryString["searchBase"] == null) ? "" : Request.QueryString["searchBase"].ToString();
        searchValue = (Request.QueryString["searchValue"] == null) ? "" : Request.QueryString["searchValue"].ToString();

        if (!IsPostBack)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["page"]))
            {
                currentPage = Int32.Parse(Request.QueryString["page"]);
            }

            if (searchBase != "" && searchValue != "")
            {
                // 검색 진행 중
                isSearch = true;
            }

            Database db = new Database();
            DataTable dt = null;

            if (isSearch)
            {
                // 검색 진행 중
                searchStop.Visible = true;
                dropdown.SelectedValue = searchBase;
                searchText.Value = searchValue;
            }

            dt = db.BoardList(pageSize, currentPage, searchBase, searchValue);

            totalCount = db.BoardListCount(searchBase, searchValue); // 전체 글 수

            if (totalCount != 0)
            {
                pageCount = totalCount / pageSize;

                if (totalCount % pageSize != 0)
                {
                    // 한 페이지 추가
                    pageCount++;
                }
            }

            if( Int32.Parse(page) > pageCount  && totalCount !=0)
            {
                alertGo("잘못된 접근입니다.", "board_list_view.aspx");
            }

            if(Int32.Parse(page) <= 0)
            {
                alertGo("page <= 0 ", "board_list_view.aspx");
            }

                if (searchBase != "" && searchBase != "title" && searchBase != "name" && searchBase != "writedate")
            {
                alertGo("잘못된 검색 접근입니다.", "board_list_view.aspx");
            }

            displayPage.Text = String.Format("<b> {0} 개의 글 [ {1}  /{2} Page ]</b>", totalCount, currentPage, pageCount);

            if (totalCount != 0)
            {
                pageMove.Text = pageGen(currentPage, pageCount, "board_list_view.aspx", String.Format("searchBase={0}&searchValue={1}", searchBase, searchValue), "page", 10);
            }
            else
            {
                pageMove.Text = "<b> (페이지 없음) <b>";
            }

            boardListRepeater.DataSource = dt;
            boardListRepeater.DataBind();

            db.DBdispose();
        }

    }

    private string pageGen(int currentPage, int pageCount, string pageName, string searchParam, string pageParam, int blockSize)
    {
        string temp = " ";
        string linkStr = pageName + "?" + searchParam + "&" + pageParam + "=#PG#"; // 기본형 #PG#로 페이지 이동 조절

        // 총 필요한 블럭의 수
        int totalBlock = pageCount / blockSize;
        if (pageCount % blockSize != 0)
        {
            totalBlock++;
        }

        // 현재 위치한 블럭
        int currentBlock = currentPage / blockSize;
        if (currentPage % blockSize != 0)
        {
            currentBlock++;
        }

        if (currentPage != 1)
        {
            temp += String.Format("<a href='{0}'>[1] </a>..", linkStr.Replace("#PG#", "1"));
        }

        if (currentBlock == 1)
        {
            // 첫번째 블럭이면 링크 필요 없음
            //temp += String.Format("[이전 {0}개] .. ", blockSize);
        }
        else
        {
            // 내 앞 블럭의 첫째 페이지로 이동
            temp += String.Format("<a href='{0}'>[이전 {1}개]</a> .. ", linkStr.Replace("#PG#", ((currentBlock - 2) * blockSize + 1.ToString())), blockSize);
        }

        for (int i = 1; i < blockSize; i++)
        {
            int blockStartPageNumber = (currentBlock - 1) * blockSize;
            if (blockStartPageNumber + i == currentPage)
            {
                temp += String.Format("<font color='red'><b> {0} </b></font>", currentPage);
            }
            else
            {
                temp += String.Format("<a href='{0}'>{1} </a>", linkStr.Replace("#PG#", (blockStartPageNumber + i).ToString()), (currentBlock - 1) * blockSize + i);
            }

            if (blockStartPageNumber + i == pageCount)
            {
                break;
            }
        }

        if (currentBlock == totalBlock)
        {
            //temp += String.Format(" .. [다음 {0}개] ", blockSize);
        }
        else
        {
            temp += String.Format(" .. <a href='{0}'>[다음 {1}개]</a> ",
              linkStr.Replace("#PG#", ((currentBlock * blockSize) + 1).ToString()), blockSize);
        }

        // 마지막 페이지 이동링크
        if (currentPage != 1)
            temp += String.Format(" .. <a href='{0}'>[{1}]</a>",
                   linkStr.Replace("#PG#", pageCount.ToString()), pageCount);

        // 만든 페이징 리턴
        return temp;
    }

      protected void search_ServerClick(object sender, EventArgs e)
    {
        bool IsChecked = true;

        if (searchText.Value == "")
        {
            string message = "검색 내용을 입력하세요.";
            Response.Write("<script>alert(\"" + message + "\");</script>");
            IsChecked = false;
            return;
        }
        else
        {
            if (IsChecked)
            {
                Response.Redirect(
                    String.Format("board_list_view.aspx?searchBase={0}&searchValue={1}",
                                   dropdown.SelectedValue,
                                   searchText.Value.Trim()
                                )
                    );
            }
        }

    }

    protected void boardListRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        //int VIRTUAL_NUM = pageSize - e.Item.ItemIndex;
        int VIRTUAL_NUM = totalCount - e.Item.ItemIndex - ((currentPage - 1) * pageSize);
        ((Label)e.Item.FindControl("num")).Text = VIRTUAL_NUM.ToString();
    }

    protected void boardWrite_ServerClick(object sender, EventArgs e)
    {
        justGo("board_write.aspx");
    }

    protected void searchStop_ServerClick(object sender, EventArgs e)
    {
        justGo("board_list_view.aspx");
    }
}

