using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required, StringLength(50, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 6) ]
        public string username { get; set; }
        [Required, StringLength(20, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 4) ]
        public string password { get; set; }
    }
}