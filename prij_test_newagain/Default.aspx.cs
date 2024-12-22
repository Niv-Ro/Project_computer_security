using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace prij_test_newagain
{
    public partial class Default : System.Web.UI.Page
    {

        MySql.Data.MySqlClient.MySqlConnection conn;
        MySql.Data.MySqlClient.MySqlCommand cmd;
        MySql.Data.MySqlClient.MySqlDataReader reader;
        string name;
        string queryStr;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void LogInEventMethod(object sender, EventArgs e)   // need to use  the Validate password before registering!!!!!!!!!!!!!!!!!!!!!!!

        {
            /// change to  hash and salt in login!!!!!!!!!!!!!!!!!!!!!!!
            SecurePasswordHandler SecurePassword = new SecurePasswordHandler();
            SecurePasswordHandler.VerificationResult result = SecurePassword.VerifyHashPassword(userNameTextBox.Text, passWordTextBox.Text);
            // Check the result
            if (result.IsValid)
            {
                Session["uname"] = result.Message;
                Response.BufferOutput = true;
                Response.Redirect("Logged_in.aspx", false);
            }
            else
            {
                passWordTextBox.Text = "Invalid user";
                Message.Text = "Invalid user";
            }
        }






protected void ForgotPasswordEventMethod(object sender, EventArgs e)
        {
            Response.BufferOutput = true;
            Response.Redirect("Forgot_password.aspx", false);
        }

        protected void RegisterEventMethod(object sender, EventArgs e)
        {
            Response.BufferOutput = true;
            Response.Redirect("Registration.aspx", false);
        }
    }
}