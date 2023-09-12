using Experimental.System.Messaging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CommonLayer.Model
{
    public class MsmqModel
    {
        MessageQueue messageQ = new MessageQueue(); // Created Object(instance) of MessageQueue

        public void MessageSender(string Token) // Method for sending MSMQ message
        {
            messageQ.Path = @".\private$\MyPrivateQueue"; // Set the path of the private queue
            if (!MessageQueue.Exists(messageQ.Path))
            {
                MessageQueue.Create(messageQ.Path);
            }

            messageQ.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) }); // Set message Formator
            messageQ.ReceiveCompleted += MessageQ_ReceiveCompleted;
            
            messageQ.Send(Token); // Send the MSMQ message 
            messageQ.BeginReceive(); // Begin Receiving message asynchronously 
            messageQ.Close(); // Close the message queue after sending
        }

        private void MessageQ_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            try
            {
                var msg = messageQ.EndReceive(e.AsyncResult); // Complete the asynchronous receive operation
                string subject = "Fundoo Notes Reset Password JWT token";
                string body = msg.Body.ToString() ; // Extract the message body from the MSMQ message 


                // Configure the SMTP(Simple Mail Transfer Protocol) client for sending emails
                var SMTP = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("usahu898@gmail.com", "qahsjtcjpfvfxajl"),
                    EnableSsl = true
                };

                // Send the email using the SMTP client
                SMTP.Send("usahu898@gmail.com", "upendrasahu1199@gmail.com", subject, body); // We can only send one person in msmq

                messageQ.BeginReceive();// Begin
            }
            catch (Exception)
            {

                throw ;
            }
        }
    }
}
