using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace mmmsl.Models
{
    public class Game
    {
        public int Id { get; set; }
        
        [Required]
        public DateTimeOffset DateAndTime { get; set; }

        [Required]
        public int HomeTeamId { get; set; }
        public Team HomeTeam { get; set; }

        [Required]
        public int AwayTeamId { get; set; }
        public Team AwayTeam { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string DivisionId { get; set; }
        public Division Division { get; set; }
        public List<Goal> Goals { get; set; }

        public int TeamGoalTotal(int teamId) => Goals?
            .Where(goal => goal.TeamId == teamId)
            .Sum(goal => goal.Count) ?? 0;
    }
}