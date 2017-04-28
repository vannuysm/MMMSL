using System.Collections.Generic;

namespace mmmsl.Areas.Manage.Models
{
    public class EditTeamPostModel
    {
        public List<int> Managers { get; set; }
        public List<int> Roster { get; set; }
    }
}
