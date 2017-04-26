namespace mmmsl.Models
{
    public class Field
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description => Location == null ? Name : $"{Location.Name} : {Name}";

        public int LocationId { get; set; }
        public Location Location { get; set; }
    }
}
