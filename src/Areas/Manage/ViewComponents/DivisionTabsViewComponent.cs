using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mmmsl.Models;

namespace mmmsl.ViewComponents
{
    public class DivisionTabsViewComponent : ViewComponent
    {
        private readonly MmmslDatabase database;

        public DivisionTabsViewComponent(MmmslDatabase database)
        {
            this.database = database;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var divisions = await database.Divisions.ToListAsync();
            return View(divisions);
        }
    }
}