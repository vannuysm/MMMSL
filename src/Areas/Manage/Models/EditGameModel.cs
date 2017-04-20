using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using mmmsl.Models;

namespace mmmsl.Areas.Manage.Models
{
    public class EditGameModel
    {
        public Game Game { get; set; }
        public List<SelectListItem> Divisions { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Teams { get; set; } = new List<SelectListItem>();
    }
}