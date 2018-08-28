using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;

using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Security.Cryptography;

/// <summary>
/// Webbase의 요약 설명입니다.
/// </summary>
public class Webbase : System.Web.UI.Page
{
    protected void alertGo(string message, string url)
    {
        Response.Write("<script>alert(\"" + message + "\");window.location.href='" + url + "';</script>");
        Response.End();
    }

    protected void alertCommentBack(string message)
    {
        Response.Write("<script>alert(\"" + message + "\");window.location.history.go(-3);</script>");
        Response.End();
    }

    protected void justGo(string url)
    {
        Response.Write("<script>window.location.href='" + url + "';</script>");
        Response.End();
    }

    // 쿼리문에 사용할 수 있게 값을 변경한다.
    protected string stringToQuery(string pOrgStr)
    {
        if (pOrgStr == null)
            return "";

        return pOrgStr.Replace("'", "''").Replace("\0", "");
    }

    // 비밀번호 암호화
    public static string SHA256(string password)
    {
        StringBuilder Sb = new StringBuilder();

        using (SHA256 hash = SHA256Managed.Create())
        {
            Encoding enc = Encoding.UTF8;
            Byte[] result = hash.ComputeHash(enc.GetBytes(password));

            foreach (Byte b in result)
                Sb.Append(b.ToString("x2"));
        }

        return Sb.ToString();
    }

    // sql injection 처리
    protected string CheckNumber(string pString)
    {
        if (pString == null)
            return null;

        string number = "0123456789";

        for (int i = 0; i < pString.Length; i++)
        {
            if (number.IndexOf(pString.Substring(i, 1)) == -1)
            {
                Response.Write("잘못된 인자입니다.");
                Response.End();
            }
        }

        return pString;
    }

}