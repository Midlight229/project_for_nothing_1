using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using project_for_nothing_1.CodeData;
using project_for_nothing_1.Models;

namespace project_for_nothing_1.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly AppDbContext _db;
        public ProjectsController(AppDbContext db) => _db = db; // the => is called expression-bodied member, just a shortcut basically


        public async Task<IActionResult> Index() =>
            View(await _db.Projects.OrderBy(p => p.CreatedAt).ToListAsync());



        public async Task<IActionResult> Details(int id)
        {
            var project = await _db.Projects
                .Include(p => p.Boards)
                .FirstOrDefaultAsync(p => p.Id == id);
            return project == null ? NotFound() : View(project);
        }

        // if we don't type in [HttpPost] or get, it will initially be set to [HttpGet]
        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Project model)
        {
            if (!ModelState.IsValid) return View(model);
            _db.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Edit(int id)
        {
            var item = await _db.Projects.FindAsync(id);
            return item == null ? NotFound() : View(item);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Project model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);
            _db.Update(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.Projects.FindAsync(id);
            return item == null ? NotFound() : View(item);
        }


        [HttpPost,ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _db.Projects.FindAsync(id);
            if (item != null)
            {
                _db.Projects.Remove(item);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

    }
        

}