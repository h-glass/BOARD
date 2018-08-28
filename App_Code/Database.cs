using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

/// <summary>
/// Database의 요약 설명입니다.
/// </summary>
public class Database
{
    protected SqlConnection db_conn = null;

    public Database(string connectionStr)
    {
        db_conn = new SqlConnection(connectionStr);
        db_conn.Open();
    }

    public Database()
    {
        string ConnString = System.Configuration.ConfigurationSettings.AppSettings["DSN"];
        db_conn = new SqlConnection(ConnString);
        db_conn.Open();
    }

   public void DBdispose()
    {
        db_conn.Close();
        db_conn.Dispose();
        db_conn = null;
    }

    ~Database()
    {
        if (db_conn != null)
        {
            db_conn.Close();
            db_conn.Dispose();
        }
    }

    public void ExecuteQuery(string query)
    {
        // 1개의 string 형을 받고 그것을 실행
        SqlCommand cmd = new SqlCommand(query, db_conn);
        cmd.ExecuteNonQuery();

        DBdispose();
    }

    public object ExecuteQueryResult(string query)
    {
        // query문을 실행하는데 1개의 결과를 return
        object return_val = null;
        SqlCommand cmd = new SqlCommand(query, db_conn);

        return_val = cmd.ExecuteScalar();
        DBdispose();
        return return_val;
    }

    public int GetQueryInt(string query)
    {
        int result = 0;

        SqlCommand comd = new SqlCommand(query, db_conn);
        SqlDataReader dr = comd.ExecuteReader();
        comd.Dispose();

        if (dr.Read())
        {
            string sresult = dr.GetSqlValue(0).ToString();
            result = Convert.ToInt32(sresult);
        }

        dr.Close();

        return result;
    }

    public string GetQueryString(string query)
    {
        string result = "";

        SqlCommand comd = new SqlCommand(query, db_conn);
        SqlDataReader dr = comd.ExecuteReader();
        comd.Dispose();

        if (dr.Read())
        {
            result = dr.GetSqlValue(0).ToString();
        }

        dr.Close();

        return result;
    }


    public SqlDataReader GetQueryResult(string query)
    {
        
        SqlCommand comd = new SqlCommand(query, db_conn);
        comd.CommandTimeout = 60 * 10;
        SqlDataReader dr = comd.ExecuteReader();
        comd.Dispose();

        return dr;
      
    }

    public DataTable ExecuteQueryDataTable(string query)
    {
        // query문의 결과를 여러개 받을 때 DataTable로 받아옴
        DataSet ds = new DataSet();

        SqlDataAdapter da = new SqlDataAdapter(query, db_conn);
        da.Fill(ds, "tempTable");
        return ds.Tables[0];
    }

    // 게시판 글쓰기
    public void BoardWrite(string title, string name, string content, string password)
    {
        string query = String.Format("INSERT INTO BOARD_LIST (title, name, contents, password) VALUES('{0}', '{1}', '{2}', '{3}')",
                                       title, name, content, password);
       
       ExecuteQuery(query);

    }

    // 글 갯수 받아오기 검색 중이라면 검색 조건 반영
    public int BoardListCount(string searchBase, string searchValue)
    {
        string query = "SELECT COUNT(*) FROM BOARD_LIST";
        if (!String.IsNullOrEmpty(searchBase) && !String.IsNullOrEmpty(searchValue))
        {
            query += " WHERE " + searchBase + " LIKE '%" + searchValue + "%'";
        }
        return GetQueryInt(query);
    }

    // 게시판 목록
    public DataTable BoardList()
    {
        return ExecuteQueryDataTable("SELECT * FROM BOARD_LIST ORDER BY writedate DESC");
    }

    // 검색 중인 목록
    public DataTable BoardList(string searchBase, string searchValue)
    {
        return ExecuteQueryDataTable(
           "SELECT * FROM BOARD_LIST WHERE " + searchBase + " LIKE '%" + searchValue + "%' ORDER BY writedate DESC"
        );
    }

    // 페이징 처리
    public DataTable BoardList(int pageSize, int currentPage, string searchBase, string searchValue)
    {
        int startNo, endNo;

        startNo = (currentPage - 1) * pageSize;
        endNo = (currentPage * pageSize) + 1;

        string condition = String.Format(" WHERE row_num > {0} AND row_num < {1}", startNo, endNo);

        string query = "SELECT * FROM"
            + " ( " + "SELECT ROW_NUMBER() OVER(ORDER BY idx DESC) AS row_num, * FROM BOARD_LIST  temp";
        if (!String.IsNullOrEmpty(searchBase) && !String.IsNullOrEmpty(searchValue))
        {
            query += " WHERE " + searchBase + " LIKE '%" + searchValue + "%'";
        }
        query += " ) " + " AS BOARD_NUMBERED " + condition;

        return ExecuteQueryDataTable(query.ToString());
    }

    // board_detail로 보여주기
    public DataTable BoardDetail(string idx)
    {
        string query = String.Format("SELECT * FROM BOARD_LIST WHERE idx={0}",  idx);
        return ExecuteQueryDataTable(query);

    }

    // comment 작성
    public void CommentWrite(string board_id, string name, string content, string password)
    {
        string query = String.Format("INSERT INTO COMMENT_LIST(board_id, name, contents, password) VALUES({0}, '{1}', '{2}', '{3}')",
                            board_id, name, content, password);

        ExecuteQuery(query);
    }

    // comment revise
    public void ReviseComment(string idx, string content, string password)
    {
        string query = String.Format("UPDATE COMMENT_LIST SET contents='{0}', password='{1}', revisedate=getdate() WHERE idx={2}",
                            content, password, idx);

        ExecuteQuery(query);
    }

    // 댓글 리스트
    public DataTable CommentList(string board_id)
    {
        string query = "SELECT * FROM COMMENT_LIST WHERE board_id=" + board_id;
        return ExecuteQueryDataTable(query);
    }

    public void DeleteComment(string idx)
    {
        string query = String.Format("DELETE FROM COMMENT_LIST WHERE idx={0}", idx);

        SqlCommand cmd = new SqlCommand(query, db_conn);
        cmd.ExecuteNonQuery();

        DBdispose();
    }

    // 게시글 수정
    public void ReviseBoard(string idx, string title, string content, string password)
    {
        string query = String.Format("UPDATE BOARD_LIST SET title='{0}', contents='{1}', password='{2}', revisedate=getdate() WHERE idx={3}",
                                    title, content, password, idx);
        ExecuteQuery(query);
    }

    // 게시판 글 삭제
    public void DeleteBoard(string idx)
    {
        string query = String.Format("DELETE FROM BOARD_LIST WHERE idx={0}", idx);
        ExecuteQuery(query);
    }

    public string GetBoardPassword(string idx)
    {
        string query = String.Format("SELECT password FROM BOARD_LIST WHERE idx ={0}",
                                       idx);
        string result = GetQueryString(query);
        return result;
    }

    public string GetCommentPassword(string idx)
    {
        string query = String.Format("SELECT password FROM COMMENT_LIST WHERE idx ={0}", idx);
        string result = GetQueryString(query);
        return result;
    }
}