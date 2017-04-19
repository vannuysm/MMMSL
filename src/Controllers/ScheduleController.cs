using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mmmsl.Models;

namespace mmmsl.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly MmmslDatabase database;

        public ScheduleController(MmmslDatabase database)
        {
            this.database = database;
        }

        public async Task<IActionResult> Index(string divisionId)
        {
            if (string.IsNullOrEmpty(divisionId)) {
                return NotFound();
            }

            var games = await database.Games
                .Where(game => game.DivisionId == divisionId)
                .Include(game => game.AwayTeam)
                .Include(game => game.HomeTeam)
                .Include(game => game.Goals)
                .ThenInclude(goal => goal.Player)
                .Include(game => game.Penalties)
                .ThenInclude(penalty => penalty.Player)
                .OrderBy(game => game.DateAndTime)
                .ToListAsync();

            var schedule = new Schedule {
                Division = await database.Divisions.SingleAsync(d => d.Id == divisionId)
            };

            foreach (var dateGroup in games.GroupBy(game => game.DateAndTime.Date)) {
                schedule.Entries.Add(dateGroup.Key.Date, dateGroup.GroupBy(group => group.DateAndTime));
            }
            
            return View(schedule);
        }
    }
}