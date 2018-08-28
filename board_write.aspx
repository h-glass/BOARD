<%@ Page Language="C#" AutoEventWireup="true" CodeFile="board_write.aspx.cs" Inherits="board_write" %>



<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>새로운 게시글 작성</title>
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

        #writeNew {
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
        <div id="writeNew">
            게시글 등록
        </div>
        <br />
        <form runat="server">
             <div class="writeTable">
            <table border="3" style="border-collapse: collapse;">
                <colgroup>
                    <col style="width: 15%;" />
                </colgroup>
                <tbody>
                    <tr>
                        <th scope="row">제목</th>
                        <td>
                            <input type="text" id="boardTitle" maxlength="15" runat="server" /> (최대 15자) </td>
                    </tr>
                    <tr>
                        <th scope="row">
                        작성자
                        <td>
                            <input type="text" id="boardWriter" runat="server" /></td>
                        </td>
                    </tr>
                    <tr>
                        <th scope="row">내용</th>
                        <td>
                            <textarea cols="300" rows="20" style="width: 98%; resize: vertical;" id="contents" runat="server"></textarea>
                        </td>
                    </tr>
                    <tr>
                        <th scope="row">비밀번호</th>
                        <td>
                            <input type="password" id="password" runat="server" />
                            (게시글 삭제 및 수정 용도)
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <br />
        <div id="buttonGroup">
            <button class="btn" id="write_complete" runat="server" onserverclick="write_complete_Click">
                <img class="btn-img" src="asset/btn_write_complete.png" alt="확인"></button>
            <button class="btn" id="write_cancel" runat="server" onserverclick="write_cancel_Click" >
                <img class="btn-img" src="asset/btn_cancel.png" alt="취소"></button>
        </div>
        </form>
       

    </div>

</body>
</html>
