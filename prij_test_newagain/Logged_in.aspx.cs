using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

namespace prij_test_newagain
{
    public partial class Logged_in : System.Web.UI.Page
    {
        string connString = System.Configuration.ConfigurationManager.ConnectionStrings["WebAppConnString"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            string name;
            string uemail;
            string passemail;
            name = (string)(Session["uname"]);
            uemail = (string)(Session["uemail"]);
            passemail = uemail;
            Session["passemail"] = uemail;
            if (!IsPostBack)
            {
                PopulateCustomerTable();
            }
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

        private void PopulateCustomerTable()
        {
            string query = "SELECT customer_ID, name, email, phone, address, package_type, package_price FROM webapp.customers";
            try
            {
                using (var conn = new MySql.Data.MySqlClient.MySqlConnection(connString))
                {
                    conn.Open();
                    using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(query, conn))
                    {
                        using (var adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            CustomerGridView.DataSource = dt;
                            CustomerGridView.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or display the error
                ScriptManager.RegisterStartupScript(this, this.GetType(), "error", $"alert('Error: {ex.Message}');", true);
            }
        }

        protected void LogOutEventMethod(object sender, EventArgs e)
        {
            Session["uname"] = null;
            Session.Abandon();
            Response.BufferOutput = true;
            Response.Redirect("Default.aspx",false);
        }

        protected void ChangePasswordEventMethod(object sender, EventArgs e)
        {
            Response.BufferOutput = true;
            Response.Redirect("Change_Password.aspx", false);
        }

        protected void SaveCustomerEventMethod(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAddCustomerID.Text) ||
                string.IsNullOrWhiteSpace(txtAddCustomerName.Text) ||
                string.IsNullOrWhiteSpace(txtAddCustomerEmail.Text) ||
                string.IsNullOrWhiteSpace(txtAddCustomerPhone.Text) ||
                string.IsNullOrWhiteSpace(txtAddCustomerAddress.Text) ||
                string.IsNullOrWhiteSpace(txtAddPackageType.Text) ||
                string.IsNullOrWhiteSpace(txtAddPackagePrice.Text))
            {
                addMessage.Text = "All fields are required";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "keepModalOpen",
                    "$('#addCustomerModal').modal('show');", true);
                return;
            }

            string saveQuery = "INSERT INTO webapp.customers (customer_ID, name, email, phone, address, package_type, package_price) VALUES (@CustomerID, @Name, @Email, @Phone, @Address, @PackageType, @PackagePrice)";

            try
            {
                using (var conn = new MySql.Data.MySqlClient.MySqlConnection(connString))
                {
                    conn.Open();
                    using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(saveQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustomerID", txtAddCustomerID.Text);
                        cmd.Parameters.AddWithValue("@Name", txtAddCustomerName.Text);
                        cmd.Parameters.AddWithValue("@Email", txtAddCustomerEmail.Text);
                        cmd.Parameters.AddWithValue("@Phone", txtAddCustomerPhone.Text);
                        cmd.Parameters.AddWithValue("@Address", txtAddCustomerAddress.Text);
                        cmd.Parameters.AddWithValue("@PackageType", txtAddPackageType.Text);
                        cmd.Parameters.AddWithValue("@PackagePrice", txtAddPackagePrice.Text);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            PopulateCustomerTable();
                            addMessage.Text = "";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                                "alert('Customer added successfully!'); $('#addCustomerModal').modal('hide');", true);
                        }
                        else
                        {
                            addMessage.Text = "Failed to add customer";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "keepModalOpen",
                                "$('#addCustomerModal').modal('show');", true);
                        }
                    }
                }
            }
            catch
            {
                addMessage.Text = "Error adding customer, customer with this id is already exists";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "keepModalOpen",
                    "$('#addCustomerModal').modal('show');", true);
            }
        }

        //using System.Web;
        //string input = "<script>alert('XSS');
        //</script>";
        //string encodedInput = HttpUtility.HtmlEncode(input);
    protected void UpdateCustomerEventMethod(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUpdateCustomerID.Text) ||
                string.IsNullOrWhiteSpace(txtUpdateCustomerName.Text) ||
                string.IsNullOrWhiteSpace(txtUpdateCustomerEmail.Text) ||
                string.IsNullOrWhiteSpace(txtUpdateCustomerPhone.Text) ||
                string.IsNullOrWhiteSpace(txtUpdateCustomerAddress.Text) ||
                string.IsNullOrWhiteSpace(txtUpdatePackageType.Text) ||
                string.IsNullOrWhiteSpace(txtUpdatePackagePrice.Text))
            {
                updateMessage.Text = "All fields are required";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "keepModalOpen",
                    "$('#updateCustomerModal').modal('show');", true);
                return;
            }

            string updateQuery = @"UPDATE webapp.customers SET name = @Name, email = @Email, phone = @Phone, 
                           address = @Address, package_type = @PackageType, package_price = @PackagePrice 
                           WHERE customer_ID = @CustomerID";
            try
            {
                using (var conn = new MySql.Data.MySqlClient.MySqlConnection(connString))
                {
                    conn.Open();
                    using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustomerID", txtUpdateCustomerID.Text);
                        cmd.Parameters.AddWithValue("@Name", txtUpdateCustomerName.Text);
                        cmd.Parameters.AddWithValue("@Email", txtUpdateCustomerEmail.Text);
                        cmd.Parameters.AddWithValue("@Phone", txtUpdateCustomerPhone.Text);
                        cmd.Parameters.AddWithValue("@Address", txtUpdateCustomerAddress.Text);
                        cmd.Parameters.AddWithValue("@PackageType", txtUpdatePackageType.Text);
                        cmd.Parameters.AddWithValue("@PackagePrice", txtUpdatePackagePrice.Text);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            PopulateCustomerTable();
                            updateMessage.Text = "";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                                "alert('Customer updated successfully!'); $('#updateCustomerModal').modal('hide');", true);
                        }
                        else
                        {
                            updateMessage.Text = "No customer found with this ID";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "keepModalOpen",
                                "$('#updateCustomerModal').modal('show');", true);
                        }
                    }
                }
            }
            catch 
            {
                updateMessage.Text = "Error updating customer";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "keepModalOpen",
                    "$('#updateCustomerModal').modal('show');", true);
            }
        }
        protected void DeleteCustomerEventMethod(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDeleteCustomerID.Text))
            {
                deleteMessage.Text = "Customer ID is required";
                // Prevent the modal from closing
                ScriptManager.RegisterStartupScript(this, this.GetType(), "keepModalOpen",
                    "$('#deleteCustomerModal').modal('show');", true);
                return;
            }

            string delquery = "DELETE FROM webapp.customers WHERE customer_ID = @CustomerID";
            try
            {
                using (var conn = new MySql.Data.MySqlClient.MySqlConnection(connString))
                {
                    conn.Open();
                    using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(delquery, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustomerID", txtDeleteCustomerID.Text);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            PopulateCustomerTable();
                            deleteMessage.Text = "";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                                "alert('Customer deleted successfully!'); $('#deleteCustomerModal').modal('hide');", true);
                        }
                        else
                        {
                            deleteMessage.Text = "No customer found with this ID";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "keepModalOpen",
                                "$('#deleteCustomerModal').modal('show');", true);
                        }
                    }
                }
            }
            catch
            {
                deleteMessage.Text = "Error deleting customer";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "keepModalOpen",
                    "$('#deleteCustomerModal').modal('show');", true);
            }
        }

       

    }
}