using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace App.Web.Models.Vm
{
    public class ResetPasswordRequest
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "The Email field is not a valid e-mail address.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "OTP is required.")]
        public string OTP { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Mobile No is required.")]
        public string MobileNo { get; set; }
    }
}