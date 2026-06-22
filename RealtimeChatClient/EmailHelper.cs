using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace RealtimeChatClient
{
    public static class EmailHelper
    {
        public static async Task SendVerificationCodeAsync(string toEmail, string code)
        {
            using (MailMessage mail = new MailMessage())
            {
                // Email hệ thống gửi đi
                mail.From = new MailAddress(AppConfig.SmtpEmail, AppConfig.SmtpDisplayName);

                // Email người dùng nhận mã
                mail.To.Add(toEmail);

                mail.Subject = "Mã xác nhận đăng ký Realtime Chat App";
                mail.Body =
$@"Xin chào,

Mã xác nhận đăng ký tài khoản của bạn là: {code}

Mã này có hiệu lực trong 5 phút.

Nếu bạn không thực hiện yêu cầu này, vui lòng bỏ qua email.";

                mail.IsBodyHtml = false;

                using (SmtpClient smtp = new SmtpClient(AppConfig.SmtpHost, AppConfig.SmtpPort))
                {
                    smtp.EnableSsl = true;
                    smtp.Credentials = new NetworkCredential(
                        AppConfig.SmtpEmail,
                        AppConfig.SmtpPassword
                    );

                    await smtp.SendMailAsync(mail);
                }
            }
        }
    }
}