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

        
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void LogInEventMethod(object sender, EventArgs e)  

        {
            SecurePasswordHandler SecurePassword = new SecurePasswordHandler();
            // Check the result
            if (SecurePassword.VerifyHashPassword(userNameTextBox.Text, passWordTextBox.Text))
            {
                Session["uname"] = userNameTextBox.Text;
                Response.BufferOutput = true;
                Response.Redirect("Logged_in.aspx", false);
            }
            else
            {
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