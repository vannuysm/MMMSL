using mmmsl.Models;

namespace mmmsl.Areas.Manage.Models
{
    public class EditProfileModel
    {
        public Profile Profile { get; set; }
        public bool MakeAdministrator { get; set; }
    }
}
