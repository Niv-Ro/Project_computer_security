using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using MySql.Data.MySqlClient;

namespace prij_test_newagain
{
    public partial class Forgot_password : System.Web.UI.Page
    {
        MySql.Data.MySqlClient.MySqlConnection conn;
        MySql.Data.MySqlClient.MySqlCommand cmd;
        MySql.Data.MySqlClient.MySqlDataReader reader;
        string queryStr;
        private static Random rand = new Random();

        // Public static field for the verification code
        public static int verification = rand.Next(111111, 999999);

        // Optional: Public static property for controlled access to the verification code
        public static int VerificationCode
        {
            get { return verification; }
            set { verification = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Page load logic (e.g., showing any initial data or handling session)
        }

        protected void BackToLoginEventMethod(object sender, EventArgs e)
        {
            Response.BufferOutput = true;
            Response.Redirect("Default.aspx", false);
        }

        protected void Verify_EventMethod(object sender, EventArgs e)
        {
            // Verify the entered code against the generated one
            if (verifyTextBox.Text == verification.ToString())
            {
                Response.BufferOutput = true;
                Response.Redirect("Default.aspx", false); // Redirect if code is correct
            }
            else
            {
                Message.Text = "Code is incorrect"; // Show an error message if code is wrong
                Message.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void Forgot_EventMethod(object sender, EventArgs e)
        {
            // Database connection string
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["WebAppConnString"].ToString();

            // Initialize database connection
            conn = new MySql.Data.MySqlClient.MySqlConnection(connString);
            conn.Open();

            // Generate a new verification code
            verification = rand.Next(111111, 999999);

            try
            {
                // Read email from the TextBox
                string recipientEmail = mailTextBox.Text;

                // Validate email input and check if email exists in the database
                queryStr = "SELECT * FROM webapp.new_tableuserregistration WHERE email = @Email";
                cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);

                // Use parameterized query to prevent SQL injection
                cmd.Parameters.AddWithValue("@Email", recipientEmail);

                // Execute the query
                reader = cmd.ExecuteReader();

                // If email is empty or does not exist in the database
                if (string.IsNullOrWhiteSpace(recipientEmail) || !reader.HasRows)
                {
                    Message.Text = "Please enter a valid email address.";
                    Message.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                // Configure and send the email with the verification code
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("matansit04@gmail.com");
                    mail.To.Add(recipientEmail);
                    mail.Subject = "Password Reset";
                    mail.Body = "Your verification code is: " + verification;
                    mail.IsBodyHtml = true;

                    // Configure SMTP client
                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential("matansit04@gmail.com", "jrmp jnxe dpdb cfve");
                        smtp.EnableSsl = true;
                        smtp.Send(mail); // Send the email
                        Message.Text = "Verification email sent successfully!";
                        Message.ForeColor = System.Drawing.Color.Green;
                    }
                }

                // Show the verification textbox and button
                verifyTextBox.Visible = true;
                forgot_Button.Enabled = false;  // Disable the forgot button after email is sent
                verifyButton.Visible = true;   // Show the verification button

            }
            catch (Exception ex)
            {
                Message.Text = "Error sending email: " + ex.Message; // Handle errors
                Message.ForeColor = System.Drawing.Color.Red;
            }
            finally
            {
                // Close the connection and reader
                reader.Close();
                conn.Close();
            }
        }
    }
}


