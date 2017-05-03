namespace mmmsl.Models
{
    public class BoardMember
    {
        public int ProfileId { get; set; }
        public Profile Profile { get; set; }

        public string Title { get; set; }
    }
}
