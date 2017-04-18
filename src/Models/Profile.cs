using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mmmsl.Models
{
    public class Profile
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        
        [DataType(DataType.EmailAddress)]
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