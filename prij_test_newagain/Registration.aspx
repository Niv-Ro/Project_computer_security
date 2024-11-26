<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="prij_test_newagain.Registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>registration</title>
</head>
<body style="background-color:#32323D; font-family:verdana">
    <form id="form1" runat="server">

        <div style =" width:500px; height:500px; margin-top:100px;margin:auto; background-color:gray; text-align:center; background-color:#ffffff; border:medium; border-radius: 25px;">
            <div style="position:relative;top:80px">
                <h3 style="font-size:30px;color:#32323D">Register</h3>
                <asp:TextBox ID="firstNameTextBox" runat="server" placeholder="Enter your name " Style="border: 1px solid #c8c8c8; border-radius: 10px; padding: 5px; font-family:verdana; border-color:#32323D" />
                <br /><br/>
                <asp:TextBox ID="lastNameTextBox" runat="server" placeholder="Enter your last name " Style="border: 1px solid #c8c8c8; border-radius: 10px; padding: 5px; font-family:verdana; border-color:#32323D" />
                <br /><br/>
                <asp:TextBox ID="userNameTextBox" runat="server" placeholder="Enter new username " Style="border: 1px solid #c8c8c8; border-radius: 10px; padding: 5px; font-family:verdana; border-color:#32323D" />
                <br /><br/>
                <asp:TextBox ID="passWordTextBox" runat="server" placeholder="Enter new password " Style="border: 1px solid #c8c8c8; border-radius: 10px; padding: 5px; font-family:verdana; border-color:#32323D" />
                <br /><br/>
                <asp:TextBox ID="emailTextBox" runat="server" placeholder="Enter your email " Style="border: 1px solid #c8c8c8; border-radius: 10px; padding: 5px; font-family:verdana; border-color:#32323D" />
                <br /><br/>
                <asp:Button ID="registerButton" Text="Submit" runat="server" OnClick="RegisterEventMethod" Style="border: 1px solid #c8c8c8; border-radius: 10px; padding: 10px;width:125px; background-color:#32323D; color:white; font-family:verdana" />
                <br />
                <asp:Button ID="logInButton" Text="Log In" runat="server" OnClick="BackToLogInEventMethod" Style="font-size:11px; border:unset;background-color:white;color:#32323D " />
                  
                </div>
                                

                
       
      </div>
    </form>
</body>
</html>
