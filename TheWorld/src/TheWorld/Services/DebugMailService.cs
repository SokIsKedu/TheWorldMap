using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using MimeKit;


namespace TheWorld.Services
{
    public class DebugMailService : IMailService
    {
        
            public void SendMail(string to, string from, string subject, string body)
            {
            Chilkat.MailMan mailman = new Chilkat.MailMan();

            //  Any string argument automatically begins the 30-day trial.
            bool success = mailman.UnlockComponent("30-day trial");
            if (success != true)
            {
                Console.WriteLine(mailman.LastErrorText);
                return;
            }

            //  Set the SMTP server.
            mailman.SmtpHost = "smtp.gmail.com";

            mailman.SmtpUsername = "et88568";
            mailman.SmtpPassword = "templarORDIN3//";

            mailman.SmtpSsl = true;
            mailman.SmtpPort = 465;

            //  Create a new email object
            Chilkat.Email email = new Chilkat.Email();

            email.Subject = subject;
            email.Body = body;
            email.From = from;
            success = email.AddTo("Admin", to);

            success = mailman.SendEmail(email);
            Console.WriteLine(success);
            if (success != true)
            {
                Console.WriteLine(mailman.LastErrorText);
                return;
            }

            success = mailman.CloseSmtpConnection();
            if (success != true)
            {
                Console.WriteLine("Connection to SMTP server not closed cleanly.");
            }

            Console.WriteLine("Mail Sent!");






        }
    }
    }
