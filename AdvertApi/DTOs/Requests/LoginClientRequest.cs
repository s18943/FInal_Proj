using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertApi.DTOs.Requests
{
    public class LoginClientRequest
    {
        [Required]
        public string Login { get; set; }
        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }
}
