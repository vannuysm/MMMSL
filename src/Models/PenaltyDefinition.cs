namespace mmmsl.Models
{
    public enum PenaltyCardSeverity
    {
        Yellow = 1,
        Red = 2
    }

    public class PenaltyDefinition
    {
        public string MisconductCode { get; set; }
        public string Description { get; set; }
        public int Points { get; set; }
        public PenaltyCardSeverity Severity { get; set; }
        public string Title => $"{MisconductCode} : {Description}";
    }
}
