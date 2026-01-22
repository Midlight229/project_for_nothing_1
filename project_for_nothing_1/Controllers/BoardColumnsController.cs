using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using project_for_nothing_1.CodeData;
using project_for_nothing_1.Models;
using System.Threading.Tasks;

namespace project_for_nothing_1.Controllers
{
    public class BoardColumnsController : Controller
    {
        private readonly AppDbContext _db;

        public BoardColumnsController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Create(int boardId)
        {
            ViewBag.BoardId = boardId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(int boardId, string name)
        {
            // save column
            var column = new BoardColumn
            {
                BoardId = boardId,
                Name = name
            };

            _db.BoardColumns.Add(column);
            await _db.SaveChangesAsync();

            return RedirectToAction("Details", "Boards", new { id = boardId });

        }
    }
}
