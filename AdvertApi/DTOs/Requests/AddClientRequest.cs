using System.ComponentModel.DataAnnotations;

namespace AdvertApi.DTOs.Requests
{
    public class AddClientRequest
    {   
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Login { get; set; }
        [MinLength(8)]
        [Required]
        public string Password { get; set; }
    }
}
