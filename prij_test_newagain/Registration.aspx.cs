using System;
using System.Collections.Generic;
using System.IO; 
using System.Linq;
using System.Text.RegularExpressions; 
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;

namespace prij_test_newagain
{
    public partial class Registration : System.Web.UI.Page
    {
        //MySql.Data.MySqlClient.MySqlConnection conn;
        //MySql.Data.MySqlClient.MySqlCommand cmd;
        //string queryStr;
        private List<string> validationErrors = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
           
        }
        protected void RegisterEventMethod(object sender, EventArgs e)
        {
            
            
            /// change to  hash and salt in registration
            SecurePasswordHandler  SecurePassword= new SecurePasswordHandler();

            if (ValidatePassword(passWordTextBox.Text)) // Validate password before registering
            {
                if (EmailIsValid(emailTextBox.Text))
                {
                    RegisterUser();
                    Session.Abandon();
                    Response.BufferOutput = true;
                    Response.Redirect("Default.aspx", false);
                }
                else {
                    errorLabel.Text = "Email is not valid";
                    errorLabel.Visible = true;
                    errorLabel.ForeColor = System.Drawing.Color.Red;
                }
                
            }
            else
            {
                // Show error message to the user
                errorLabel.Text = string.Join("<br/>", validationErrors);
                errorLabel.Visible = true;
                errorLabel.ForeColor = System.Drawing.Color.Red;
            }

        }


        /*private void RegisterUser()
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

        }*/

        public bool EmailIsValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        private void RegisterUser()
        {

            /*
            */

            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["WebAppConnString"].ToString();
            SecurePasswordHandler SecurePassword = new SecurePasswordHandler();
            var (hashedSaltPassword, salt) = SecurePassword.CreatePasswordHash(passWordTextBox.Text);

            // Use a using block to ensure proper disposal of the connection
            using (var conn = new MySql.Data.MySqlClient.MySqlConnection(connString))
            {
                conn.Open();

                string queryStr = "INSERT INTO webapp.new_tableuserregistration (firstname, lastname, username, password, email, password_hash, salt) " +
                                  "VALUES (@FirstName, @LastName, @UserName, @Password, @Email, @password_Hash, @salt)";


                using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn))
                {

                    // Use parameters to securely add user input
                    cmd.Parameters.AddWithValue("@FirstName", firstNameTextBox.Text);
                    cmd.Parameters.AddWithValue("@LastName", lastNameTextBox.Text);
                    cmd.Parameters.AddWithValue("@UserName", userNameTextBox.Text);
                    cmd.Parameters.AddWithValue("@Password", passWordTextBox.Text); // Note: Consider hashing the password before storing it
                    cmd.Parameters.AddWithValue("@Email", emailTextBox.Text);
                    cmd.Parameters.AddWithValue("@password_Hash", hashedSaltPassword);
                    cmd.Parameters.AddWithValue("@Salt", salt);
                    

                    // Execute the command
                    cmd.ExecuteNonQuery();
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
                else
                {
                    // Handle unknown rules (log or skip)
                    LogUnrecognizedRule(rule);
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

        protected void BackToLogInEventMethod(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.BufferOutput = true;
            Response.Redirect("Default.aspx", false);
        }
    }
}