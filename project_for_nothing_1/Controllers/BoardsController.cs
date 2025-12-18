using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using project_for_nothing_1.CodeData;
using project_for_nothing_1.Models;

namespace project_for_nothing_1.Controllers
{
    public class BoardsController : Controller // a built in class for controllers that support view
    {
        private readonly AppDbContext _db;
        public BoardsController(AppDbContext db) => _db = db;

        // LIST OF BOARDS
        public async Task<IActionResult> Index()
        {
            var data = await _db.Boards
                .Include(b => b.Project)
                .OrderBy(b => b.Name)
                .ToListAsync();

            return View(data);
        }


        //DETAILS (KANBAN BOARD VIEW)
        public async Task<IActionResult> Details(int id)
        {
            var board = await _db.Boards
                .Include(b => b.Project)
                .Include(b => b.Columns.OrderBy(c => c.Order))
                .ThenInclude(c => c.Tasks.OrderBy(t => t.Order))
                .FirstOrDefaultAsync(b => b.Id == id);

            if (board == null)
                return NotFound();

            //для кнопок "Move to..." и "Assign"
            ViewBag.Columns = board.Columns.ToList();
            ViewBag.Assignees = await _db.Assignees.OrderBy(a=>a.FullName).ToListAsync();


            return View(board);

        }

        [HttpGet]
        //CREATE (GET)
        public async Task<IActionResult> Create()
        {
            await FillProjectsSelect();
            return View();
        }


        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Board model)
        {
            if (!ModelState.IsValid)
            {
                await FillProjectsSelect(model.ProjectId);
                return View(model);
            }

            _db.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // HELPER - заполнение SelectList проектами
        public async Task FillProjectsSelect(int? selectId = null)
        {
            var items = await _db.Projects
                .OrderBy(p => p.Name)
                .ToListAsync();

            ViewBag.ProjectId = new SelectList(items,nameof(Project.Id),
                nameof(Project.Name),selectId);
        }


        public async Task<IActionResult> Edit(int id)
        {
            var item = await _db.Boards.FindAsync(id);
            if (item == null) return NotFound();

            await FillProjectsSelect(item.ProjectId);
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Board model)
        {
            if (id != model.Id) return BadRequest();
            if(!ModelState.IsValid)
            {
                await FillProjectsSelect(model.ProjectId);
                return View(model);
            }

            _db.Update(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}