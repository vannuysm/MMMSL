using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mmmsl.Models
{
    public class Team
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "You must specify a team name.")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "You must specify a division.")]
        public string DivisionId { get; set; }
        public Division Division { get; set; }
        
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "You must specify a team manager.")]
        public int ManagerId { get; set; }
        public Profile Manager { get; set; }
        public List<RosterPlayer> Roster { get; set; }
    }
}