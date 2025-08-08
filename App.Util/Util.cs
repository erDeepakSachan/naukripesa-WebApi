using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace App.Util
{
    public static class Util
    {
        public static (bool isSucess, string otp) GenerateOTPAndSendMail(string tomail)
        {
            try
            {
                var fromAddress = new MailAddress("deepaksachandotcom@gmail.com", "Naukri-Pesa");
                var toAddress = new MailAddress(tomail);
                const string fromPassword = "dyhx xpnp qtej ibsm";
                const string subject = "Registration OTP";
                var otp = GetOTP().ToString();

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587, // TLS Port
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                    Timeout = 20000
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = otp
                })
                {
                    smtp.Send(message);
                    return (isSucess: true, otp: otp);
                }
            }
            catch (Exception ex)
            {
                return (isSucess: false, otp: "Something went wrong");
            }

        }

        private static int GetOTP() {
            int otp = 0;
            Random random = new Random();
            otp = random.Next(100000, 1000000); // Range: 100000 to 999999 (inclusive lower bound, exclusive upper)
            return otp;
        }
    }
}
