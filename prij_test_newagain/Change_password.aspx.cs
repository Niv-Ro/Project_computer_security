using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;





namespace prij_test_newagain
{
    public partial class Change_password : System.Web.UI.Page

    {
        protected System.Web.UI.WebControls.TextBox lastPassword;
        protected System.Web.UI.WebControls.TextBox newPassword;
        protected System.Web.UI.WebControls.TextBox newPasswordAgain;
        protected System.Web.UI.WebControls.Button submitPassword;
        protected System.Web.UI.WebControls.Label Message;
        protected System.Web.UI.WebControls.PlaceHolder resetPasswordPlaceHolder;

        private List<string> validationErrors = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BackToLoginEventMethod(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.BufferOutput = true;
            Response.Redirect("Default.aspx", false);
        }


        protected void Submit_New_Password_EventMethod(object sender, EventArgs e)
        {
            string userEmail = Session["uname"]?.ToString();//!!!!!!!!need to change the session and usernameBox to email! or username to unique! DONE AHI

            if ((newPassword.Text == newPasswordAgain.Text) && ValidatePassword(newPassword.Text) && Check_email_password(userEmail, lastPassword.Text))
            {
                resetPasswordPlaceHolder.Controls.Clear();
                lastPassword.Visible = false;
                newPassword.Visible = false;
                newPasswordAgain.Visible = false;
                submitPassword.Visible = false;

                Message.Text = "Password reset complete!";
                Message.ForeColor = System.Drawing.Color.Green;
                Message.Visible = true;

                resetPasswordPlaceHolder.Controls.Add(Message);
                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["WebAppConnString"].ToString();
                SecurePasswordHandler SecurePassword = new SecurePasswordHandler();
                var (hashedSaltPassword, salt) = SecurePassword.CreatePasswordHash(newPassword.Text);

                using (var conn = new MySql.Data.MySqlClient.MySqlConnection(connString))
                {

                    conn.Open();

                    // Single update query that handles everything
                    string updateQuery = @"UPDATE webapp.new_tableuserregistration SET password = @Password,password_hash = @Password_Hash,salt = @Salt WHERE email = @Email";
                    

                    //try
                    //{
                    using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Password", newPassword.Text);
                        cmd.Parameters.AddWithValue("@Email", userEmail);
                        cmd.Parameters.AddWithValue("@Password_Hash", hashedSaltPassword);
                        cmd.Parameters.AddWithValue("@Salt", salt);
                        cmd.ExecuteNonQuery();
                    }
                  


                    string script = "setTimeout(function() { window.location.href = 'Default.aspx'; }, 2000);";
                    ClientScript.RegisterStartupScript(this.GetType(), "RedirectAfterDelay", script, true);

                    //}
                    //catch (Exception ex)
                    //{
                    //    Message.Text = "An error occurred while resetting your password. Please try again.";
                    //    Message.ForeColor = System.Drawing.Color.Red;
                    //    Message.Visible = true;
                    //}
                }
            }

        }


        private bool ValidatePassword(string password)
        {
            // Reset the validation errors list
            validationErrors.Clear();

            // Load the password validation rules
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PasswordValidationRules.txt");
            var rules = File.ReadAllLines(filePath);

            foreach (string rule in rules)
            {
                if (rule.Contains("Minimum Length"))
                {
                    int minLength = ExtractNumber(rule);
                    if (password.Length < minLength)
                        validationErrors.Add($"Password must be at least {minLength} characters long.");
                }
                else if (rule.Contains("Uppercase"))
                {
                    if (!password.Any(char.IsUpper))
                        validationErrors.Add("Password must contain at least one uppercase letter.");
                }
                else if (rule.Contains("Lowercase"))
                {
                    if (!password.Any(char.IsLower))
                        validationErrors.Add("Password must contain at least one lowercase letter.");
                }
                else if (rule.Contains("Digit"))
                {
                    int mindig = ExtractNumber(rule);
                    if (!password.Any(char.IsDigit))
                        validationErrors.Add($"Password must contain at least {mindig} digits.");
                }
                else if (rule.Contains("Special Character"))
                {
                    int minspec = ExtractNumber(rule);
                    char[] specialChars = "!@#$%^&*()_-+=[]{}|:;\"'<>,.?/`~".ToCharArray();
                    if (!password.Any(c => specialChars.Contains(c)))
                        validationErrors.Add($"Password must contain at least {minspec} special characters.");
                }
                else if (rule.Contains("Unique Characters"))
                {
                    int minUnique = ExtractNumber(rule);
                    if (password.Distinct().Count() < minUnique)
                        validationErrors.Add($"Password must contain at least {minUnique} unique characters.");
                }
                else if (rule.Contains("No Common Words"))
                {
                    var commonWords = new[] { "password", "123456", "qwerty" }; // Expandable list of common words
                    foreach (var word in commonWords)
                    {
                        if (password.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0)
                            validationErrors.Add("Password contains a common word (e.g., 'password', '123456'). Please choose a stronger password.");
                    }
                }
              
            }

            // If any errors were found, return false and display them
            return validationErrors.Count == 0;
        }

        private int ExtractNumber(string rule)
        {
            var match = Regex.Match(rule, @"\d+");
            return match.Success ? int.Parse(match.Value) : 0;
        }
        private void LogUnrecognizedRule(string rule)
        {
            string logPath = Server.MapPath("~/UnrecognizedRules.txt"); // Path to log unrecognized rules
            using (var writer = new StreamWriter(logPath, true))
            {
                writer.WriteLine($"Unrecognized Rule: {rule} - {DateTime.Now}");
            }
        }
        private bool Check_email_password(string email, string password)
        {
            MySql.Data.MySqlClient.MySqlConnection conn;
            MySql.Data.MySqlClient.MySqlCommand cmd;
            MySql.Data.MySqlClient.MySqlDataReader reader;

            // Retrieve connection string
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["WebAppConnString"].ToString();
            conn = new MySql.Data.MySqlClient.MySqlConnection(connString);

            try
            {
                conn.Open();

                // SQL query to check email and password
                string queryStr = "SELECT * FROM webapp.new_tableuserregistration WHERE email = @Email AND password = @Password";
                cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);

                // Use parameterized query
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                reader = cmd.ExecuteReader();

                // Return true if a match is found
                return reader.HasRows;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it
                Message.Text = "An error occurred: " + ex.Message;
                Message.ForeColor = System.Drawing.Color.Red;
                return false;
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }


    }
}