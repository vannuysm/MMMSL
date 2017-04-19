using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mmmsl.Models;

namespace mmmsl.Controllers
{
    public class TeamsController : Controller
    {
        private readonly MmmslDatabase database;

        public TeamsController(MmmslDatabase database)
        {
            this.database = database;
        }

        public async Task<IActionResult> Index(string divisionId)
        {
            var division = await database.Divisions
                .SingleOrDefaultAsync(d => d.Id == divisionId);

            if (division == null) {
                return NotFound();
            }

            var teams = await database.Teams
                .Where(team => team.DivisionId == divisionId)
                .Include(team => team.Manager)
                .Include(team => team.Roster)
                .OrderBy(team => team.Name)
                .ToListAsync();

            var chunkedTeams = new List<List<Team>>();

            for (var i = 0; i < teams.Count; i += 4) { 
                chunkedTeams.Add(teams.GetRange(i, Math.Min(4, teams.Count - i))); 
            }

            return View(new TeamsIndexModel {
                Division = division,
                Teams = chunkedTeams
            });
        }
    }
}