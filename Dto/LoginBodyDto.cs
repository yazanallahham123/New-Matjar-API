using API.Utils.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dto
{
    public class LoginBodyDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = AuthErrorMessages.PASSWORD_REQUIRED)]
        public string password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = AuthErrorMessages.MOBILE_NUMBER_REQUIRED)]
        public string mobileNumber { get; set; }
        public int language { get; set; }
    }
}
