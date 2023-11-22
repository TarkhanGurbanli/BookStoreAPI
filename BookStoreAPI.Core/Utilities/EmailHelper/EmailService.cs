using BookStoreAPI.Core.Configuration;
using BookStoreAPI.Core.Utilities.EmailHelper;
using BookStoreAPI.Entities.Concrete;
using BookStoreAPI.Entities.Dtos.EmailDtos;
using MongoDB.Driver;
using System.Net;
using System.Net.Mail;

namespace AspNetCoreIdentityApp.Web.Service
{
    public class EmailService : IEmailService
    {
        private readonly IMongoCollection<AppUser> _userCollection;

        public EmailService(IMongoCollection<AppUser> userCollection)
        {
            _userCollection = userCollection;
        }

        public async Task<bool> SendEmailConfirm(string mailAddress, string token)
        {

            var emailAddress = EmailConfiguration.Email;
            var emailPassword = EmailConfiguration.Password;
            string senderEmail = emailAddress;
            string senderPassword = emailPassword;

            //Create the SMTP client
            SmtpClient smtpClient = new(EmailConfiguration.Host, EmailConfiguration.Port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(senderEmail, senderPassword)
            };

            try
            {
                // Create the email message
                MailMessage mailMessage = new()
                {
                    From = new MailAddress(senderEmail)
                };
                mailMessage.To.Add(mailAddress);
                mailMessage.Subject = $"Message from - {EmailConfiguration.Email}";
                mailMessage.Body = $@"
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Email Confirmation Link</title>
              <style>
                   body {{
                       font-family: 'Arial', sans-serif;
                        background-color: #f4f4f4;
                        margin: 0;
                        padding: 0;
                    }}
                    .container {{
                       max-width: 600px;
                       margin: 20px auto;
                        background-color: #fff;
                        padding: 20px;
                        border-radius: 8px;
                        box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                    }}
                    h4 {{
                        color: #333;
                   }}
                    p {{
                        color: #555;
                   }}
                    a {{
                      color: #007bff;
                        text-decoration: none;
                    }}
                    .btn-link {{
                        display: inline-block;
                        padding: 8px 12px;
                        background-color: #000000;
                       color: #fff;
                       text-decoration: none;
                        border-radius: 4px;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <h4>Click on the link to confirm your email:</h4>
                    <p><a class='btn-link' href='https://localhost:7020/api/Users/verifypassword?email={mailAddress}&token={token}'>Email Confirmation Link</a></p>
                </div>
            </body>
            </html>
        ";
                // Specify that the body contains HTML
                mailMessage.IsBodyHtml = true;
                // Send the email
                smtpClient.Send(mailMessage);
                Console.WriteLine("Email sent successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> SendEmailToAllUsers(EmailContentDto emailContent)
        {
            var emailAddress = EmailConfiguration.Email;
            var emailPassword = EmailConfiguration.Password;
            string senderEmail = emailAddress;
            string senderPassword = emailPassword;

            // Create the SMTP client
            SmtpClient smtpClient = new SmtpClient(EmailConfiguration.Host, EmailConfiguration.Port);
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);

            try
            {
                var allUsers = await _userCollection.Find(_ => true).ToListAsync();

                foreach (var user in allUsers)
                {
                    // Create the email message
                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress(senderEmail);
                    mailMessage.To.Add(user.Email);
                    mailMessage.Subject = emailContent.Subject;

                    // Use the HTML template and replace the placeholder with Swagger content
                    mailMessage.Body = $@"
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>{emailContent.Subject}</title>
                <style>
                    body {{
                        font-family: 'Arial', sans-serif;
                        background-color: #f4f4f4;
                        margin: 0;
                        padding: 0;
                    }}
                    .container {{
                        max-width: 600px;
                        margin: 20px auto;
                        background-color: #fff;
                        padding: 20px;
                        border-radius: 8px;
                        box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                    }}
                    h4 {{
                        color: #333;
                    }}
                    p {{
                        color: #555;
                    }}
                    a {{
                        color: #007bff;
                        text-decoration: none;
                    }}
                    .btn-link {{
                        display: inline-block;
                        padding: 8px 12px;
                        background-color: #000000;
                        color: #fff;
                        text-decoration: none;
                        border-radius: 4px;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <h4>{emailContent.Subject}</h4>
                    <p>{emailContent.Body}</p>
                </div>
            </body>
            </html>
        ";

                    // Specify that the body contains HTML
                    mailMessage.IsBodyHtml = true;

                    // Send the email
                    await smtpClient.SendMailAsync(mailMessage);

                    Console.WriteLine($"Email sent to {user.Email} successfully.");
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending emails: " + ex.Message);
                return false;
            }
            finally
            {
                // Dispose of the SMTP client to release resources
                smtpClient.Dispose();
            }

        }

    }
}
