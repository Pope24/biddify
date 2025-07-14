using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailOtpSender
{
    private readonly string smtpHost;
    private readonly int smtpPort;
    private readonly string smtpUser;
    private readonly string smtpPass;
    private readonly bool enableSsl;

    public EmailOtpSender(string host, int port, string user, string pass, bool ssl = true)
    {
        smtpHost = host;
        smtpPort = port;
        smtpUser = user;
        smtpPass = pass;
        enableSsl = ssl;
    }

    public async Task<bool> SendOtpAsync(string toEmail, string otpCode)
    {
        try
        {
            using (var smtp = new SmtpClient(smtpHost, smtpPort))
            {
                smtp.Credentials = new NetworkCredential(smtpUser, smtpPass);
                smtp.EnableSsl = enableSsl;

                var mail = new MailMessage
                {
                    From = new MailAddress(smtpUser, "Biddify Security"),
                    Subject = "Your OTP Code",
                    Body = $"Your OTP code is: {otpCode}",
                    IsBodyHtml = false
                };

                mail.To.Add(toEmail);

                await smtp.SendMailAsync(mail);
            }

            return true;
        }
        catch
        {
            return false;
        }
    }
}
