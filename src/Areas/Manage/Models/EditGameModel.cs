using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using mmmsl.Models;

namespace mmmsl.Areas.Manage.Models
{
    public class EditGameModel
    {
        public Game Game { get; set; }
        public Goal Goal { get; set; } = new Goal();
        public Penalty Penalty { get; set; } = new Penalty();
        public List<SelectListItem> Divisions { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Teams { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> HomeTeamPlayers { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> AwayTeamPlayers { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> PenaltyCards { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Fields { get; set; } = new List<SelectListItem>();
    }
}