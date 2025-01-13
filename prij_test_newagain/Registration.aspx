<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="prij_test_newagain.Registration" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Registration</title>
    <style>
        body {
            background-color: #f4f4f9;
            font-family: 'Arial', sans-serif;
            margin: 0;
            padding: 0;
        }

        .form-container {
            max-width: 400px;
            margin: 100px auto;
            padding: 20px;
            background-color: #ffffff;
            border-radius: 10px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
            text-align: center;
        }

        .form-container h3 {
            font-size: 24px;
            color: #333333;
            margin-bottom: 20px;
        }

        .form-container input, .form-container button {
            width: calc(100% - 20px);
            margin: 10px;
            padding: 10px;
            border-radius: 5px;
            border: 1px solid #cccccc;
            font-size: 14px;
        }

        .form-container input:focus {
            border-color: #007BFF;
            outline: none;
        }

        .form-container button {
            background-color: #007BFF;
            color: white;
            border: none;
            cursor: pointer;
            transition: background-color 0.3s;
        }

        .form-container button:hover {
            background-color: #0056b3;
        }

        .form-container .link-button {
            background: none;
            border: none;
            color: #007BFF;
            cursor: pointer;
            text-decoration: underline;
            font-size: 12px;
        }

        .form-container .link-button:hover {
            color: #0056b3;
        }

        .form-container .error-label {
            color: red;
            font-size: 12px;
            margin-top: 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="form-container">
            <h3>Register</h3>

            <asp:TextBox ID="firstNameTextBox" runat="server" placeholder="Enter your name"></asp:TextBox>
            <asp:TextBox ID="lastNameTextBox" runat="server" placeholder="Enter your last name"></asp:TextBox>
            <asp:TextBox ID="userNameTextBox" runat="server" placeholder="Enter new username"></asp:TextBox>
            <asp:TextBox ID="passWordTextBox" runat="server" placeholder="Enter new password" TextMode="Password"></asp:TextBox>
            <asp:TextBox ID="rePasswordTextBox" runat="server" placeholder="Re-enter password" TextMode="Password"></asp:TextBox>
            <asp:TextBox ID="emailTextBox" runat="server" placeholder="Enter your email" TextMode="SingleLine"></asp:TextBox>

            <asp:Button ID="registerButton" Text="Submit" runat="server" OnClick="RegisterEventMethod"></asp:Button>
            <asp:Button ID="logInButton" Text="Log In" runat="server" OnClick="BackToLogInEventMethod" CssClass="link-button"></asp:Button>

            <asp:Label ID="errorLabel" Text="" runat="server" Visible="false" CssClass="error-label"></asp:Label>
        </div>
    </form>
</body>
</html>
