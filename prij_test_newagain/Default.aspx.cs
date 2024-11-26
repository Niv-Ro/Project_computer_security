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
        protected void LogInEventMethod(object sender, EventArgs e)
        {
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["WebAppConnString"].ToString();//retrive connection string and stores it in connstring
            conn = new MySql.Data.MySqlClient.MySqlConnection(connString);
            conn.Open();
            queryStr = "";
            queryStr = "SELECT * FROM webapp.new_tableuserregistration WHERE username='"+ userNameTextBox.Text +"' AND password='" + passWordTextBox.Text +"' ";
            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            reader = cmd.ExecuteReader();
            name = "";
            while (reader.HasRows && reader.Read())
            {
                name = reader.GetString(reader.GetOrdinal("firstname")) + " " + reader.GetString(reader.GetOrdinal("lastname"));
            }


            if (reader.HasRows)
            {
                Session["uname"] = name;
                Response.BufferOutput = true;
                Response.Redirect("Logged_in.aspx", false);
            }
            else
            {
               // passWordTextBox.Text = "Invalid user";
                Message.Text= "Invalid user";
            }
            reader.Close();
            conn.Close();


        }

        protected void ForgotPasswordEventMethod(object sender, EventArgs e)
        { 
        }

        protected void RegisterEventMethod(object sender, EventArgs e)
        { 
        }
    }
}