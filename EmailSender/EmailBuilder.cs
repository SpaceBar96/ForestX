using DAL.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace EmailSender
{
    public class EmailBuilder
    {
        //User for sending email to new user for account activation
        public static void BuildEmailTemplateToNewUser(Guid id)
        {
            UserRepo objUserRepo = new UserRepo();

            string emailBody = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailTemplate/") + "EmailTextUser" + ".cshtml");
            var userInfo = objUserRepo.GetUser(id);
            var url = "https://localhost:44355/" + "Login/Confirm?id=" + id;
            emailBody = emailBody.Replace("@ViewBag.NewUserName", userInfo.Name);
            emailBody = emailBody.Replace("@ViewBag.ConfirmationLink", url);
            emailBody = emailBody.ToString();

            BuildEmailTemplate("Your Account Is Successfully Activated", emailBody, userInfo.Email);
        }

        public static void BuildEmailTemplate(string subjectText, string bodyText, string sendTo)
        {
            string from, to, bcc, cc, subject, body;
            from = "elvis.adam.lycan@gmail.com";
            to = sendTo.Trim();
            bcc = "";
            cc = "";
            subject = subjectText;
            StringBuilder sb = new StringBuilder();
            sb.Append(bodyText);
            body = sb.ToString();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from);
            mail.To.Add(new MailAddress(to));
            if (!string.IsNullOrEmpty(bcc))
            {
                mail.Bcc.Add(new MailAddress(bcc));
            }
            if (!string.IsNullOrEmpty(cc))
            {
                mail.CC.Add(new MailAddress(cc));
            }
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SendEmail(mail);
        }

        private static void SendEmail(MailMessage mail)
        {
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new System.Net.NetworkCredential("elvis.adam.lycan@gmail.com", "960126421Lvis");
            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
