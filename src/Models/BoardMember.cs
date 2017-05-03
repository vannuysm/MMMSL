namespace mmmsl.Models
{
    public class BoardMember
    {
        public int Id { get; set; }

        public int ProfileId { get; set; }
        public Profile Profile { get; set; }

        public string Title { get; set; }
    }
}
