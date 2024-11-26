using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace prij_test_newagain
{
    public partial class Logged_in : System.Web.UI.Page
    {
        string name;
        protected void Page_Load(object sender, EventArgs e)
        {
            name = (string)(Session["uname"]);
            if (name == null)
            {
                Response.BufferOutput = true;
                Response.Redirect("Default.aspx", false);
            }
            else
            {
                userlabel.Text = name;
            }
           
        }

        protected void LogOutEventMethod(object sender, EventArgs e)
        {
            Session["uname"] = null;
            Session.Abandon();
            Response.BufferOutput = true;
            Response.Redirect("Default.aspx",false);
        }
    }
}