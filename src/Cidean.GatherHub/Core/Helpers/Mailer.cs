
using Microsoft.AspNetCore.Hosting;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cidean.GatherHub.Core.Helpers
{
    public class Mailer
    {

        private readonly IHostingEnvironment _environment;

        public Mailer(IHostingEnvironment environment)
        {
            this._environment = environment;
        }


        public async Task SendMail(string fromEmail, string fromName, string subject, string toEmail, string toName,
            string textContent, string htmlContent)
        {
            string apiKey = "";
            if (_environment.IsDevelopment())
                 apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY", EnvironmentVariableTarget.User);
            else
                apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");

            var client = new SendGridClient(apiKey);
            var message = MailHelper.CreateSingleEmail(
                new EmailAddress(fromEmail, fromName), 
                new EmailAddress(toEmail, toName), 
                subject, textContent, htmlContent);
            var response = await client.SendEmailAsync(message);
        }

        public  async Task SendMail(string emailId, string toEmail, string toName, Dictionary<string,string> tags)
        {
            
            XDocument doc = XDocument.Load(Path.Combine(_environment.ContentRootPath, "Data/mailer.xml"));
            

            XElement email = doc.Descendants("Email").Where(el => el.Attribute("id").Value == emailId).First();

            

            var htmlContent = email.Element("HtmlContent").Value.FormatWithTags(tags);
            var textContent = email.Element("TextContent").Value.FormatWithTags(tags);


            await SendMail(email.Element("From").Element("Address").Value,
                email.Element("From").Element("Name").Value,
                email.Element("Subject").Value,
                toEmail,toName, textContent, htmlContent);

            
        }




    }
}
