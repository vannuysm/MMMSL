using System.ComponentModel.DataAnnotations;

namespace mmmsl.Models
{
    public class Goal
    {
        public int Id { get; set; }
        
        [Required]
        public int GameId { get; set; }
        public Game Game { get; set; }
        
        [Required]
        public int TeamId { get; set; }
        public Team Team { get; set; }
        
        [Required]
        public int PlayerId { get; set; }
        public Profile Player { get; set; }

        [Required]
        public int Count { get; set; }
    }
}