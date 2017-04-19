using System.Collections.Generic;

namespace mmmsl.Models
{
    public class TeamGoalsModel
    {
        public Team Team { get; set; }
        public List<Goal> Goals { get; set; }
    }
}
