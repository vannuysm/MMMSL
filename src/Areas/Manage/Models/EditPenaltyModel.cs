using Microsoft.AspNetCore.Mvc.Rendering;
using mmmsl.Models;
using System.Collections.Generic;

namespace mmmsl.Areas.Manage.Models
{
    public class EditPenaltyModel
    {

        public Game Game { get; set; }
        public int TeamId { get; set; }
        public int PlayerId { get; set; }
        public string MisconductCode { get; set; }
        public List<SelectListItem> Players { get; set; }
        public List<SelectListItem> PenaltyCards { get; set; }
    }
}
