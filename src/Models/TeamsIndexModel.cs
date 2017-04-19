using System.Collections.Generic;

namespace mmmsl.Models
{
    public class TeamsIndexModel
    {
        public Division Division { get; set; }
        public List<List<Team>> Teams { get; set; }
    }
}