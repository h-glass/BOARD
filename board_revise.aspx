<%@ Page Language="C#" AutoEventWireup="true" CodeFile="board_revise.aspx.cs" Inherits="board_revise" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style>
        .container {
            position: absolute;
            left: 35%;
            width: 740px;
            height: 500px;
            min-width: 740px;
            min-height: 500px;
            margin: 150px 0 0 -50px;
        }

        .btn {
            cursor: pointer;
            border: none;
            background-color: white;
            padding: 0;
        }

        #detail {
            font-size: 30px;
            font-weight: bold;
            text-align: center;
        }

        #buttonGroup {
            position: absolute;
            left: 40%;
            right: 30%;
        }
    </style>
</head>
<body>

    <div class="container">
        <div id="detail">
            게시글 수정
        </div>
        <br />
        <form id="form1" runat="server">
            <!-- 리스트 -->
            <div class="reviseTable">
                <table border="3" style="border-collapse: collapse;">
                    <colgroup>
                        <col style="width: 15%;" />
                    </colgroup>
                    <tbody>
                        <tr>
                            <th scope="row">제목</th>
                            <td>
                                <input type="text" name="boardTitle" id="boardTitle" value="<%=title %>" />
                            </td>
                        </tr>
                        <tr>
                            <th scope="row">작성자
                        <td>
                            <%= name %>
                        </td>
                        </tr>
                        <tr>
                            <th scope="row">내용</th>
                            <td>
                                <textarea cols="300" rows="20" style="width: 98%; resize: vertical;" name="contents" id="contents"><%=content %></textarea>
                            </td>
                        </tr>
                        <tr>
                            <th scope="row">비밀번호</th>
                            <td>
                                <input type="password" name="password" id="password" />
                                (게시글 삭제 및 수정 용도)
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <br />
            <div id="buttonGroup">
                <button class="btn" id="revise_complete" runat="server">
                    <img class="btn-img" src="asset/btn_revise_complete.png" alt="수정완료"></button>
                <button class="btn" id="revise_cancel" runat="server" onserverclick="revise_cancel_ServerClick">
                    <img class="btn-img" src="asset/btn_cancel.png" alt="취소"></button>
            </div>
        </form>
    </div>
    <script>
        window.onload = load();
        function load() {
            var idx = "<%= idx %>";
            if (idx == "<%=temp%>") {
                <% Session.Remove("idx"); %>
            } else {
                alert("잘못된 접근입니다.");
                window.location.href = "board_list_view.aspx";
            }
        }
    </script>
</body>
</html>
