<%@ Page Language="C#" AutoEventWireup="true" CodeFile="board_list_view.aspx.cs" Inherits="board_list_view" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style>
        a {
            text-decoration: none;
            color: #000000
        }

        .container {
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            width: 740px;
            height: 500px;
        }


        .btn {
            cursor: pointer;
            border: none;
            background-color: white;
            padding: 0;
        }

        #boardList {
            font-size: 30px;
            font-weight: bold;
            text-align: center;
        }
    </style>
</head>
<body>
    <form runat="server">
        <div class="container">

            <div id="boardList">
                게시글 목록
            </div>


            <div id="writeButton" align="right" style="margin-top: 10px;">
                <button class="btn" visible="false" id="searchStop" style="vertical-align: middle" runat="server" onserverclick="searchStop_ServerClick">
                    <img class="btn-img" src="asset/btn_search_stop.png" alt="검색 해제"></button>
                <button class="btn" id="boardWrite" style="vertical-align: middle" runat="server" onserverclick="boardWrite_ServerClick">
                    <img class="btn-img" src="asset/btn_write.png" alt="글쓰기"></button>
            </div>

            <!-- 리스트 -->
            <table cellpadding="0" cellspacing="0" border="1" width="740">
                <col width="40">
                <col width="300">
                <col width="100">
                <col width="150">
                <tr>
                    <td colspan="4" align="right">
                        <asp:Label ID="displayPage" runat="server" Text="spspsp"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th>번호</th>
                    <th>제목</th>
                    <th>작성자</th>
                    <th>작성 일시</th>
                </tr>

                <asp:Repeater ID="boardListRepeater" runat="server" OnItemDataBound="boardListRepeater_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td align="center">

                                <asp:Label ID="num" runat="server" />

                            </td>
                            <td style="text-align: left; padding-left: 10px;">
                                <a href="board_detail.aspx?idx=<%# Eval("idx") %>&page=<%# currentPage %>&searchBase=<%# searchBase %>&searchValue=<%# searchValue %>"><%# Eval("title") %>
                                </a>
                            </td>
                            <td align="center">
                                <%# Eval("name")%>
                            </td>
                            <td align="center">
                                <%# Eval("writedate").ToString().Substring(0,10)%> 
                            </td>

                        </tr>
                    </ItemTemplate>
                </asp:Repeater>

            </table>
            <br />

            <table width="740px">
                <tr>
                    <td width="50%" align="center">
                        <asp:Label ID="pageMove" runat="server">[이전 10] <b>1</b> 2 3 4 5 6 7 8 9 10 [다음 10] </asp:Label>
                    </td>
                </tr>
            </table>

            <div id="searchDropdown" style="margin-top: 30px; vertical-align: middle; display: inline-block">
                <asp:DropDownList ID="dropdown" runat="server" Height="30px">
                    <asp:ListItem Value="title" Text="제목" />
                    <asp:ListItem Value="name" Text="작성자" />
                    <asp:ListItem Value="writedate" Text="작성일시" />
                </asp:DropDownList>

                <input type="text" style="font-size: large; width: 550px" id="searchText" name="searchValue" runat="server" />
                <button class="btn" id="search" style="vertical-align: middle" runat="server" onserverclick="search_ServerClick">
                    <img class="btn-img" src="asset/btn_search.png" alt="검색"></button>
            </div>
    </form>


    </div>
</body>
<script>
    // enter 방지
    document.addEventListener('keydown', function (event) {
        if (event.keyCode === 13) {
            event.preventDefault();
        }
    }, true);
</script>
</html>
