using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mmmsl.Models
{
    public class Division
    {
        public string Id { get; set; }
        
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
        public List<Team> Teams { get; set; }
        public List<Game> Games { get; set; }
    }
}