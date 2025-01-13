<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logged_in.aspx.cs" Inherits="prij_test_newagain.Logged_in" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>logged_in</title>
</head>
<body>
    <p>logged in page</p>
    <form id="form1" runat="server">
        <div>
            <br /><br />
            <label for="userlabel">Hello </label>
            <!--<p>Hello</p>-->           
            <asp:label ID="userlabel" Text="no user" runat="server" />
                       <asp:Button ID="logOutButton" Text="Log out" runat="server" OnClick="LogOutEventMethod" />
            <br /><br />
                       <asp:Button ID="ChangePassword" Text="Change Password?" runat="server" OnClick="ChangePasswordMethod" />

        </div>
    </form>
</body>
</html>
