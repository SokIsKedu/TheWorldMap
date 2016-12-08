using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWorld.Services
{
    public class DebugMailService : IMailService
    {
       

        public void SendMail(string to, string from, string subject, string body)
        {
            Debug.WriteLine($"Sending Mail: To:{ to},From:{from},Subject:{subject}");


            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("The World", "erlandas.trumpickas@gmail.com"));
            message.To.Add(new MailboxAddress("",to));
            message.Subject = subject;
            message.Body = new TextPart("plain")
            {
                Text = body
            };
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 465, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                // Note: since we don't have an OAuth2 token, disable 	// the XOAUTH2 authentication mechanism.     client.Authenticate("anuraj.p@example.com", "password");
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}


// 129345506627-rsnekr80mp022tn4ueqopbehljpson2b.apps.googleusercontent.com
// 2Ego5bg_9JVHjWCmWiJwS-4Z