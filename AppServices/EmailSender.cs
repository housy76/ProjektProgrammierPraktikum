using AppData;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AppServices
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string emailAddress, string subject, string message)
        {
            // Command line argument must the the SMTP host.
            SmtpClient client = new SmtpClient("smtp-2.hof-university.de");

            client.Credentials = new NetworkCredential("vi-shummel", "#VV20151006");

            // Specify the e-mail sender.
            // Create a mailing address that includes a UTF8 character
            // in the display name.
            MailAddress from = new MailAddress("simon.hummel@hof-university.de",
                "Simon " + (char)0xD8 + " Hummel",
                System.Text.Encoding.UTF8);
            // Set destinations for the e-mail message.
            MailAddress to = new MailAddress(emailAddress);
            // Specify the message content.
            MailMessage mail = new MailMessage(from, to)
            {
                Body = message
            };
            // Include some non-ASCII characters in body and subject.
            string someArrows = new string(new char[] { '\u2190', '\u2191', '\u2192', '\u2193' });
            mail.Body += Environment.NewLine + someArrows;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.Subject = subject + someArrows;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            // The userState can be any object that allows your callback 
            // method to identify this send operation.
            // For this example, the userToken is a string constant.
            client.SendAsync(mail, null);

            return Task.CompletedTask;
        }
    }
}
