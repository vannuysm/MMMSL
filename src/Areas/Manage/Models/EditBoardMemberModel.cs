using Microsoft.AspNetCore.Mvc.Rendering;
using mmmsl.Models;
using System.Collections.Generic;

namespace mmmsl.Areas.Manage.Models
{
    public class EditBoardMemberModel
    {
        public BoardMember BoardMember { get; set; }
        public List<SelectListItem> Profiles { get; set; } = new List<SelectListItem>();
    }
}
