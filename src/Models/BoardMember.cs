using System.ComponentModel.DataAnnotations;

namespace mmmsl.Models
{
    public class BoardMember
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public int ProfileId { get; set; }
        public Profile Profile { get; set; }
        
        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }
    }
}
