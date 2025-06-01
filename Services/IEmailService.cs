using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string confirmationLink);
        Task VerifyEmailAddressAsync(string toEmail);
        Task SendEmailAsync(string toEmail, string subject, string htmlBody, string textBody);
        Task<bool> IsEmailVerifiedAsync(string email);
    }
}
