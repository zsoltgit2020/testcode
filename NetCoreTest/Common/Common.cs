using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace NetCoreTestProject.Common
{
    public static class Common
    {
        /// <summary>
        /// email address hashed
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string CalculateHash(string input)
        {
            using (var algorithm = SHA512.Create()) //or MD5 SHA256 etc.
            {
                var hashedBytes = algorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        /// <summary>
        /// 7. Once all the processing is completed and the email gets 10 attributes attached to it. Send an email out 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="attributes"></param>
        public static void SendMail(string email, string[] attributes)
        {
            try
            {
                SendGridClient sendGridClient = new SendGridClient(Startup.SendGridAPIKey);
                SendGridMessage sendGridMessage = new SendGridMessage();
                sendGridMessage.SetFrom("zsolt@zsolt.net");
                sendGridMessage.AddTo("email");
                sendGridMessage.Subject = "Mail to 10 attributes";

                string attributesContent = string.Empty;
                foreach (var item in attributes)
                {
                    attributesContent += item + ", ";
                }
                attributesContent = attributesContent.TrimEnd(' ').TrimEnd(',');
                sendGridMessage.AddContent(MimeType.Html, $"<b>Dear XY</b> <br /><br /> Attributes sent: {attributesContent}");
                sendGridClient.SendEmailAsync(sendGridMessage);
            }
            catch (Exception)
            {
            }
        }
    }
}
