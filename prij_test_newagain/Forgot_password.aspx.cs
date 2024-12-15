using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;


namespace prij_test_newagain
{
    public partial class Forgot_password : System.Web.UI.Page
    {


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BackToLoginEventMethod(object sender, EventArgs e)
        {
            Response.BufferOutput = true;
            Response.Redirect("Default.aspx", false);
        }


        protected void Forgot_EventMethod(object sender, EventArgs e)
        {
            try
            {
                // Read email from TextBox
                string recipientEmail = mailTextBox.Text;

                if (string.IsNullOrWhiteSpace(recipientEmail))
                {
                    Message.Text = "Please enter a valid email address.";
                    Message.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                // Configure email
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("project.comp.sec.test@gmail.com");
                    mail.To.Add(recipientEmail);
                    mail.Subject = "Password Reset";
                    mail.Body = "Reset your password";
                    mail.IsBodyHtml = true;

                    // Configure SMTP
                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 465))
                    {
                        smtp.Credentials = new System.Net.NetworkCredential("project.comp.sec.test@gmail.com", "Projecttemp1234!");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                        Message.Text = "Email sent successfully!";
                        Message.ForeColor = System.Drawing.Color.Green;
                    }
                }
            }

            catch (Exception ex)
            {
                Message.Text = "Error sending email: " + ex.Message;
                Message.ForeColor = System.Drawing.Color.Red;
            }

        }
    }
}



 