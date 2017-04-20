using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using mmmsl.Models;

namespace mmmsl.Areas.Manage.Models
{
    public class EditGoalModel
    {
        public Game Game { get; set; }
        public int TeamId { get; set; }
        public int PlayerId { get; set; }
        public int Count { get; set; } = 1;
        public List<SelectListItem> Players { get; set; }
    }
}
