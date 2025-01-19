<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logged_in.aspx.cs" Inherits="prij_test_newagain.Logged_in" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>logged_in</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 0;
            background-color: #f4f4f9;
        }

        .navbar {
            background-color: #007BFF;
            color: white;
            padding: 1rem;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        .navbar .title {
            font-size: 1.5rem;
            font-weight: bold;
        }

        .navbar button {
            background-color: white;
            color: #007BFF;
            border: none;
            padding: 0.5rem 1rem;
            border-radius: 5px;
            cursor: pointer;
        }

        .navbar button:hover {
            background-color: #0056b3;
            color: white;
        }

        .container {
            max-width: 1200px;
            margin: 2rem auto;
            padding: 1rem;
            background-color: white;
            border-radius: 8px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        }

        .actions {
            display: flex;
            justify-content: space-between;
            margin-bottom: 1.5rem;
        }

        .actions button {
            background-color: #007BFF;
            color: white;
            border: none;
            padding: 0.7rem 1.5rem;
            border-radius: 5px;
            font-size: 1rem;
            cursor: pointer;
            transition: background-color 0.3s;
        }

        .actions button:hover {
            background-color: #0056b3;
        }

        table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 1rem;
        }

        table, th, td {
            border: 1px solid #ddd;
        }

        th, td {
            padding: 0.8rem;
            text-align: left;
        }

        th {
            background-color: #007BFF;
            color: white;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="navbar">
            <div class="title">Admin Dashboard</div>
            <asp:Button ID="logOutButton" Text="Log out" runat="server" OnClick="LogOutEventMethod" />
        </div>

        <div class="container">
            <div>
                <label for="userlabel">Hello </label>
                <asp:label ID="userlabel" Text="no user" runat="server" />
            </div>

            <div class="actions">
                <asp:Button ID="btnAddCustomer" Text="Add Customer" runat="server" />
                <asp:Button ID="btnUpdateCustomer" Text="Update Customer" runat="server" />
                <asp:Button ID="btnRemoveCustomer" Text="Remove Customer" runat="server" />
                <asp:Button ID="btnViewCustomers" Text="View All Customers" runat="server" />
                <asp:Button ID="btnChangePassword" Text="ChangePassword" runat="server" OnClick="ChangePasswordEventMethod" />

            </div>

            <table>
                <thead>
                    <tr>
                        <th>Customer ID</th>
                        <th>Name</th>
                        <th>Email</th>
                        <th>Phone</th>
                    </tr>
                </thead>
                <tbody>
                    <!-- Data will be populated dynamically -->
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>
