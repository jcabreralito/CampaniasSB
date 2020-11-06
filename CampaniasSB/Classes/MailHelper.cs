using CampaniasSB.Properties;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CampaniasSB.Classes
{
    public class MailHelper
    {
        public static async Task SendMail(string to, string cco, string subject, string body)
        {
            var message = new MailMessage();
            message.To.Add(new MailAddress(to));
            message.Bcc.Add(new MailAddress(cco));
            message.From = new MailAddress(Resources.emailInfo.ToString());
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = Resources.emailInfo.ToString(),
                    Password = Resources.passwordInfo.ToString()
                };

                smtp.Credentials = credential;
                smtp.Host = Resources.SMTPName;
                smtp.Port = int.Parse(Resources.SMTPPort);
                smtp.EnableSsl = Convert.ToBoolean(Resources.SSL);
                await smtp.SendMailAsync(message);
            }
        }

        public static async Task SendMail(List<string> mails, string subject, string body)
        {
            var message = new MailMessage();

            foreach (var to in mails)
            {
                message.To.Add(new MailAddress(to));
            }

            message.From = new MailAddress(Resources.emailInfo.ToString());
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = Resources.emailInfo.ToString(),
                    Password = Resources.passwordInfo.ToString()
                };

                smtp.Credentials = credential;
                smtp.Host = Resources.SMTPName;
                smtp.Port = int.Parse(Resources.SMTPPort);
                smtp.EnableSsl = Convert.ToBoolean(Resources.SSL);
                await smtp.SendMailAsync(message);
            }
        }

        internal static Task SendMail(object nombreUsuario, string subject, string body)
        {
            throw new NotImplementedException();
        }
    }
}