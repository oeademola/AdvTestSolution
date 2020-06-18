using System.ComponentModel.DataAnnotations;

namespace Advansio.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required(ErrorMessage="Email is Required")]
        public string Email { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 6, ErrorMessage="You must specify a password between 6 and 15 characters")]
        public string Password { get; set; }
        [RegularExpression(@"^(\+?[0-9]+)$",ErrorMessage="Invalid Mobile Number")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage="FirstName is Required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage="LastName is Required")]
        public string LastName { get; set; }
    }
}