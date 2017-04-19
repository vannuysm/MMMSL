using System.Collections.Generic;

namespace mmmsl.Models
{
    public class TeamPenaltiesModel
    {
        public Team Team { get; set; }
        public List<Penalty> Penalties { get; set; }
    }
}
