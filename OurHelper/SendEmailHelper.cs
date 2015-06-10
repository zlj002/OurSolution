using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace OurHelper
{
    public class SendEmailHelper
    { 
        public ResultResponse SendEmail(int emailPort, string emailHost, EmailObject emailObj)
        {
            ResultResponse response = new ResultResponse();
            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            msg.From = new MailAddress(emailObj.Address, emailObj.DisplayName, System.Text.Encoding.UTF8);
            msg.To.Add(emailObj.EmailTo);
            msg.Subject = emailObj.Subject;//邮件标题 
            msg.SubjectEncoding = System.Text.Encoding.UTF8;//邮件标题编码 
            msg.Body = emailObj.EmailBody;//邮件内容 
            msg.BodyEncoding = System.Text.Encoding.UTF8;//邮件
            msg.IsBodyHtml = true;
            msg.Priority = MailPriority.Normal;
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential(emailObj.Address, emailObj.Password);
            client.Port = emailPort;//使用的端口 
            client.Host = emailHost;
            client.EnableSsl = true;//经过ssl加密 
            object userState = msg;
            //client.Send(msg);
            try
            {
                //client.SendAsync(msg, userState);
                //client.SendCompleted += new SendCompletedEventHandler(client_SendCompleted);
                //client.Send(msg);
                client.SendAsync(msg, "");
                response.State = true;
                response.Msg = "Send Email Success!";
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                response.State = false;
                response.Msg = ex.Message;
            }
            return response;
        }
        //void client_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        //{
        //  //  frm.UpdateProcessBar();
        //}
    }
    public class EmailObject
    {
        public string Address { get; set; }
        public string DisplayName { get; set; }
        public string EmailTo { get; set; }
        public string Subject { get; set; }
        public string EmailBody { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class ResultResponse
    { 
        public bool State { get; set; }
        public string Msg { get; set; }
    }
}
