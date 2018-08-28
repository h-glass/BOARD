<%@ Page Language="C#" AutoEventWireup="true" CodeFile="board_password_check.aspx.cs" Inherits="board_password_check" %>
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
        

     
    </style>
</head>
<body>
    <div class="container">
        <form runat="server">
            <div style=" font-size:30px; font-weight:bold; text-align:center;""> 비밀번호를 입력하세요. </div>
            <br />
        <div id="input"align="center">
            <asp:TextBox TextMode="Password" runat="server" Rows="2" Columns="20" ID="inputPassword" OnTextChanged="InputPassword_TextChanged"/>
        </div>
        </form>
    </div>
</body>
</html>
