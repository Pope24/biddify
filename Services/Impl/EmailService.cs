using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.Extensions.Configuration;

namespace Service.Impl
{
    public class EmailService : IEmailService
    {
        string subjectTemplate = "Confirm your email";
        string htmlBodyTemplate = "<p>Please confirm your email by clicking this link: <a href='{0}'>{1}</a></p>";
        string textBodyTemplate = "Please confirm your email by visiting: {0}";


        private readonly IAmazonSimpleEmailService _sesClient;
        private readonly string _senderEmail;

        public EmailService(IConfiguration config)
        {
            var awsAccessKey = config["AWS:AccessKey"];
            var awsSecretKey = config["AWS:SecretKey"];
            var region = RegionEndpoint.GetBySystemName(config["AWS:Region"]);

            _senderEmail = "levanchinh2422003@gmail.com";

            _sesClient = new AmazonSimpleEmailServiceClient(awsAccessKey, awsSecretKey, region);
        }
        public async Task SendEmailAsync(string toEmail, string confirmationLink)
        {
            string htmlBody = string.Format(htmlBodyTemplate, confirmationLink, confirmationLink);
            string textBody = string.Format(textBodyTemplate, confirmationLink);
            var sendRequest = new SendEmailRequest
            {
                Source = _senderEmail,
                Destination = new Destination
                {
                    ToAddresses = new List<string> { toEmail }
                },
                Message = new Message
                {
                    Subject = new Content(subjectTemplate),
                    Body = new Body
                    {
                        Html = new Content { Charset = "UTF-8", Data = htmlBody },
                        Text = new Content { Charset = "UTF-8", Data = textBody }
                    }
                }
            };

            await _sesClient.SendEmailAsync(sendRequest);
        }
        public async Task SendEmailAsync(string toEmail, string subject, string htmlBody, string textBody)
        {
            var sendRequest = new SendEmailRequest
            {
                Source = _senderEmail,
                Destination = new Destination
                {
                    ToAddresses = new List<string> { toEmail }
                },
                Message = new Message
                {
                    Subject = new Content(subject),
                    Body = new Body
                    {
                        Html = new Content { Charset = "UTF-8", Data = htmlBody },
                        Text = new Content { Charset = "UTF-8", Data = textBody }
                    }
                }
            };

            await _sesClient.SendEmailAsync(sendRequest);
        }

        public async Task VerifyEmailAddressAsync(string toEmail)
        {
            await _sesClient.VerifyEmailAddressAsync(new VerifyEmailAddressRequest()
            {
                EmailAddress = toEmail
            });
        }
        public async Task<bool> IsEmailVerifiedAsync(string email)
        {

            var request = new GetIdentityVerificationAttributesRequest
            {
                Identities = new List<string> { email }
            };

            var response = await _sesClient.GetIdentityVerificationAttributesAsync(request);

            if (response.VerificationAttributes != null && response.VerificationAttributes.ContainsKey(email))
            {
                var status = response.VerificationAttributes[email].VerificationStatus;
                return status == VerificationStatus.Success;
            }
            return false;
        }
    }
}
