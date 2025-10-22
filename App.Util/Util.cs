using System.Net;
using System.Net.Mail;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;


namespace App.Util
{
    public static class Utility
    {
        public static (bool isSucess, string otp) GenerateOTPAndSendMail(string tomail)
        {
            try
            {
                var regOtp = GetOTP().ToString();
                var isMailSent = SendEmailViaResend(tomail, "Registration OTP", regOtp).Result; // Await the Task<bool> result
                if (isMailSent)
                {
                    return (isSucess: true, otp: regOtp);
                }
                else
                {
                    return (isSucess: false, otp: "Failed to send OTP");
                }
            }
            catch (Exception ex)
            {
                return (isSucess: false, otp: "Something went wrong");
            }
        }

        public static bool SendMail(string tomail, string subject, string body)
        {
            try
            {
                var fromAddress = new MailAddress("support@naukripesa.com", "Naukri-Pesa");
                var toAddress = new MailAddress(tomail);
                const string fromPassword = "naukripesa@support";

                var smtp = new SmtpClient
                {
                    Host = "smtpout.secureserver.net",
                    Port = 465, //587, // TLS Port
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                    Timeout = 20000
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static async Task<bool> SendEmailViaResend(string toEmail, string sub, string body)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "re_PPnNqqL1_3vuxvKexqmZtCM2Hr9HMY3yP");

            var payload = new
            {
                from = "Naukri-Pesa <support@naukripesa.com>",
                to = toEmail,
                subject = sub,
                html = body
            };

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://api.resend.com/emails", content);

            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }


        private static int GetOTP() {
            int otp = 0;
            Random random = new Random();
            otp = random.Next(100000, 1000000); // Range: 100000 to 999999 (inclusive lower bound, exclusive upper)
            return otp;
        }
    }
}
