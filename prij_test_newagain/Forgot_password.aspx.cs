﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Text;


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
            string hashedInputCode = HashSHA1(verifyTextBox.Text);

            // Compare the entered code with the stored verification code
            if (hashedInputCode == HashSHA1(verification.ToString()))
            {
                Response.BufferOutput = true;
                Response.Redirect("Reset_password.aspx", false); // Redirect if code is correct
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

                // Hash the verification code using SHA-1 before sending it
                string hashedVerificationCode = HashSHA1(verification.ToString());


                // Configure and send the email with the verification code
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("matansit04@gmail.com");
                    mail.To.Add(recipientEmail);
                    mail.Subject = "Password Reset";
                    mail.Body = "Your verification code is: " + verification + "\n\nHash: " + hashedVerificationCode;
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


                ButtonPlaceHolder.Controls.Clear(); // Remove all controls from the PlaceHolder
                TextBoxPlaceHolder.Controls.Clear();
                // Dynamically add the verifyButton in the same place
                ButtonPlaceHolder.Controls.Add(verifyButton);
                TextBoxPlaceHolder.Controls.Add(verifyTextBox);

                forgot_Button.Visible = false;  // Hide forgotButton
                verifyButton.Visible = true; // Make verifyButton visible
                mailTextBox.Visible = false;
                verifyTextBox.Visible = true;
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
        private string HashSHA1(string input)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] data = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in data)
                {
                    sb.Append(b.ToString("x2"));
                }
                // Truncate the hash to 8 characters (you can choose any length here)
                string truncatedHash = sb.ToString().Substring(0, 8);
                return truncatedHash;
            }
            
        }
    }
}


