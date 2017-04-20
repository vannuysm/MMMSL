using Microsoft.AspNetCore.Mvc.Rendering;
using mmmsl.Models;
using System.Collections.Generic;

namespace mmmsl.Areas.Manage.Models
{
    public class EditGameResultModel
    {
        public Game Game { get; set; }
        public Goal Goal { get; set; } = new Goal();
        public Penalty Penalty { get; set; } = new Penalty();
        public List<SelectListItem> HomeTeamPlayers { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> AwayTeamPlayers { get; set; } = new List<SelectListItem>();
    }
}
