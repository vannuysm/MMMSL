using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mmmsl.Models
{
    public class Profile
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a first name.")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a last name.")]
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";

        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }
        public List<Role> Roles { get; set; }

        public bool HasRole(string roleName)
        {
            return Roles?.HasRole(roleName) ?? false;
        }

        public Role GetRole(string roleName)
        {
            return Roles?.GetRole(roleName);
        }
    }
}