using System.ComponentModel.DataAnnotations.Schema;

namespace Advansio.API.Dtos
{
    public class UserForListDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AccountNumber { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal AccountBalance { get; set; }
    }
}