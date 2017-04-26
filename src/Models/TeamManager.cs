namespace mmmsl.Models
{
    public class TeamManager
    {
        public int ProfileId { get; set; }
        public Profile Profile { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
    }
}
