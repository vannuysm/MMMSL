using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using mmmsl.Models;

namespace mmmsl.Areas.Manage.Models
{
    public class EditTeamModel
    {
        public Team Team { get; set; }
        public List<SelectListItem> Divisions { get; set; }
        public List<SelectListItem> Profiles { get; set; }
    }
}