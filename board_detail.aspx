<%@ Page Language="C#" AutoEventWireup="true" CodeFile="board_detail.aspx.cs" Inherits="board_detail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>상세 내용 페이지</title>
    <style>
        .container {
            position: absolute;
            top: 40%;
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

        #detail {
            font-size: 30px;
            font-weight: bold;
            text-align: center;
        }
    </style>

</head>
<body>
    <form runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="True">
        </asp:ScriptManager>
        <div class="container">
            <div id="detail">
                게시글 상세 내용
            </div>
            <br />
            <div class="buttonGroup" align="right">
                <button class="btn" id="goToListView" runat="server" onserverclick="goToListView_ServerClick">
                    <img class="btn-img" src="asset/btn_list.png" alt="목록으로"></button>
                <button class="btn" id="revise" runat="server" onserverclick="revise_ServerClick">
                    <img class="btn-img" src="asset/btn_revise.png" alt="수정"></button>
                <button class="btn" id="delete" runat="server" onserverclick="delete_ServerClick">
                    <img class="btn-img" src="asset/btn_delete.png" alt="삭제"></button>
            </div>
            <!-- 리스트 -->
            <table border="3" style="border-collapse: collapse;">
                <colgroup>
                    <col style="width: 15%;" />
                </colgroup>
                <tbody>
                    <tr>
                        <th scope="row">글 번호</th>
                        <td style="text-align: center;">
                            <%= idx %>
                        </td>
                        <th scope="row" style="width: 100px">작성자</th>
                        <td style="text-align: center;">
                            <%= name %>
                        </td>
                    </tr>
                    <tr>
                        <th scope="row">제목</th>
                        <td colspan="3">&nbsp;<%= title %>
                        </td>
                    </tr>
                    <tr>
                        <th scope="row">작성 일시</th>
                        <td style="text-align: center; width: 200px;">
                            <%= writedate %>
                        </td>
                        <th scope="row">수정 일시</th>
                        <td style="text-align: center; width: 200px;">
                            <%= revisedate %>
                        </td>
                    </tr>
                    <tr>
                        <th scope="row">내용</th>
                        <td colspan="3">
                            <textarea cols="300" rows="20" style="width: 98%; resize: vertical;" readonly="readonly" id="contents"><%= content %></textarea>
                        </td>
                    </tr>
                </tbody>
            </table>
            <table border="3" style="border-collapse: collapse; width: 740px; margin-top: 10px; margin-bottom: 10px; table-layout: fixed; word-break: break-all;"">
                <colgroup>
                    <col style="width: 15%;" />
                    <col style="width: 75%;" />
                    <col style="width: 10%;" />
                </colgroup>
                <tbody>
                    <!-- 댓글 리스트 -->
                    <tr>
                        <th>작성자</th>
                        <th colspan="2">내용</th>
                    </tr>
                    <asp:Repeater runat="server" ID="commentListRepeater" OnItemDataBound="commentListRepeater_ItemDataBound">
                        <ItemTemplate>
                            <tr id="<%# String.Format("comment{0}", Eval("idx")) %>">
                                <td align="center" id="<%# String.Format("commentWriter{0}", Eval("idx")) %>">
                                    <%# Eval("name") %>
                                </td>
                                <td id="<%# Eval("idx") %>">
                                    <%# Eval("contents") %> 
                                </td>
                                <td align="center">
                                    <button class="btn" id="<%# String.Format("commentRevise{0}",Eval("idx")) %>" onclick="reviseCommentClick(this.id)">
                                        <img class="btn-img" src="asset/btn_revise.png" alt="댓글수정" style="width: 55px">
                                    </button>
                                    <button class="btn" id="<%#  String.Format("commentDelete{0}",Eval("idx"))%>" onclick="deleteCommentClick(this.id)">
                                        <img class="btn-img" src="asset/btn_delete.png" alt="댓글삭제" style="width: 55px">
                                    </button>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
            <table border="none" style="border-collapse: collapse;">
                <tbody>
                    <tr>
                        <th scope="row">작성자</th>
                        <td>&nbsp<input type="text" id="commentWriter" runat="server" />
                        </td>
                        <th scope="row">비밀번호</th>
                        <td>&nbsp<input type="password" id="commentPassword" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="align:center" colspan="4">
                            <textarea id="commentContents" cols="250" rows="3" runat="server" style="width: 90%; resize: none; "></textarea>
                            <button class="btn" id="commentWriteComplete" stryle="width:10%; tablecell"  runat="server" onserverclick="commentWriteComplete_ServerClick">
                                <img class="btn-img" src="asset/btn_comment_complete.png" alt="댓글 등록"></button>
                        </td>
                    </tr>
                </tbody>
            </table>
            
    </form>
    </div>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
    <script type="text/javascript"> 
        var eventSrcID
        var inputPassword;
        var checkPassword;
        
        function reviseCommentClick(id) {
            eventSrcID = id.substr(13, id.length - 13); // 이벤트가 발생한 버튼 id
            inputPassword = prompt("비밀번호를 입력하세요.", "");
            if (inputPassword == null) {
                    alert("댓글 수정을 취소하였습니다.");
                    return;
                }
            checkPassword = false;
            PageMethods.checkPassword(eventSrcID, inputPassword, onReviseCheckPwdSuccess, onCallBackFail);
        }

        function deleteCommentClick(id) {
            eventSrcID = id.substr(13, id.length - 13); // 이벤트가 발생한 버튼 id
           
            inputPassword = prompt("비밀번호를 입력하세요.", "");
            if (inputPassword == null) {
                    alert("댓글 삭제를 취소하였습니다.");
                    return;
                }
            checkPassword = false;
            //_deleteComment();
            PageMethods.checkPassword(eventSrcID, inputPassword, onDeleteCheckPwdSuccess, onCallBackFail);
        }

        function onReviseCheckPwdSuccess(result) {
            if (!result) {
                // 비밀번호 틀림
                alert("비밀번호가 일치하지 않습니다.");
            } else {
                // 비밀번호 일치, 수정 진행
                reviseComment();
            }
        }

        function onDeleteCheckPwdSuccess(result) {
            if (!result) {
                // 비밀번호 틀림
                alert("비밀번호가 일치하지 않습니다.");
            } else {
                // 비밀번호 일치, 삭제 진행
                deleteComment();
            }
        }

        function _deleteComment() {   
            var sReturn = "";
            $.ajax({
            type: "POST",
                url: "board_detail.aspx/deleteComment",
                data: "{ idx: '" + eventSrcID + "', inputPassword: '" + inputPassword+"' }",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: "false",
                cache: "false",
                success: function (msg) {
                    // On success           
                    // alert(msg);
    
                    window.location.reload();
                },
                Error: function (x, e) {
                    // On Error
                }
            });
            
        }  

        function reviseComment() {

            var originalContent = document.getElementById(eventSrcID).innerHTML;

            originalContent = originalContent.replace(/(^\s*) | (\s*$)/g, '');

            var reviseContent;

            while (true) {
                reviseContent = prompt("수정할 내용을 입력하세요.", originalContent);
                if (reviseContent == null) {
                    alert("댓글 수정을 취소하였습니다.");
                    return;
                }
                if (reviseContent != "") {
                    break;
                }
            }

            var revisePassword;
            while (true) {
                revisePassword = prompt("수정할 비밀번호를 입력하세요.");
                if (revisePassword == null) {
                    alert("댓글 수정을 취소하였습니다.");
                    return;
                }
                if (revisePassword != "") {
                    break;
                }
            }

            PageMethods.reviseComment(eventSrcID, reviseContent, revisePassword, onCommentSuccess, onCallBackFail);
            window.location.reload();
        }

        function deleteComment() {
            //PageMethods.deleteComment(eventSrcID, onDeleteSuccess, onCallBackFail);
            _deleteComment();
            window.location.reload();
        }

        function onDeleteSuccess(result) {
            if (result) {
                alert("댓글을 삭제하였습니다.");
            } else {
                
            }
             window.refresh();
        }


        function onCommentSuccess(result) {
            if (result) {
                alert("댓글을 수정하였습니다.");
            } else {
                
            }

        }

        function onCallBackFail(result) {
            alert("CallBack Failed");
        }

    </script>
</body>
</html>
