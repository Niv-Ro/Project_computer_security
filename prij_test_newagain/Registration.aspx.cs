using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace prij_test_newagain
{
    public partial class Registration : System.Web.UI.Page
    {
        MySql.Data.MySqlClient.MySqlConnection conn;
        MySql.Data.MySqlClient.MySqlCommand cmd;
        string queryStr;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void RegisterEventMethod(object sender, EventArgs e)
        {
            RegisterUser();
        }

        private void RegisterUser()
        {
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["WebAppConnString"].ToString();
            
            conn = new MySql.Data.MySqlClient.MySqlConnection(connString);
            conn.Open();
            queryStr = "";
            queryStr = "INSERT INTO webapp.new_tableuserregistration(firstname,lastname,username,password,email)" +
                "VALUES('" + firstNameTextBox.Text + "','" + lastNameTextBox.Text + "','" + userNameTextBox.Text + "','" + passWordTextBox.Text + "','" + emailTextBox.Text + "')";
            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.ExecuteReader();

            conn.Close();

        }

        protected void BackToLogInEventMethod(object sender, EventArgs e)
        { 
        }
    }
}