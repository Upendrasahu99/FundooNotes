using CommonLayer.Model;
using MassTransit;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SendMail.Mail
{
    public class MailService : IConsumer<CreateCollabModel>
    {


        public async Task Consume(ConsumeContext<CreateCollabModel> context)
        {
            var data = context.Message; // containg data which is consume
            string email = data.Email;
            string subject = "FundooNotes Collab";
            string body = "Got Mail for FundooNote Collab";
            var SMTP = new SmtpClient("smtp.gmail.com") // SMTP Configur gmail Smtp server setting
            {
                Port = 587,
                Credentials = new NetworkCredential("usahu898@gmail.com", "qahsjtcjpfvfxajl"), // gmail account username and password provide as credential
                EnableSsl = true,
            };
            SMTP.Send("usahu898@gmail.com", email, subject, body);
        }
    }
}
