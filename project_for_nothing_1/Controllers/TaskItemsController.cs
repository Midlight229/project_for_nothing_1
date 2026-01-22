using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using project_for_nothing_1.CodeData;
using project_for_nothing_1.Models;

namespace project_for_nothing_1.Controllers
{
    public class TaskItemsController : Controller
    {
        private readonly AppDbContext _db;
        public TaskItemsController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Create(int columnId)
        {
         var model=new TaskItem { ColumnId=columnId };
            
            ViewBag.Assignees=await _db.Assignees
                .OrderBy(a=>a.FullName)
                .ToListAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskItem model)
        {
            var column = await _db.BoardColumns
                .Include(c=>c.Board)
                .FirstOrDefaultAsync(c => c.Id == model.ColumnId);

            if (column == null)
                return BadRequest($"Column {model.ColumnId} not found.");


            // save column
            /*var task = new TaskItem
            {
                Title = title,
                ColumnId = column.Id,
                BoardId = column.BoardId,
                CreatedAt = DateTime.UtcNow
            };*/

            
            model.BoardId = column.BoardId;
            model.CreatedAt = DateTime.UtcNow;

            _db.TaskItems.Add(model);
            await _db.SaveChangesAsync();

            return RedirectToAction("Details", "Boards",
                new { id = column.BoardId });
        }


        public async Task<IActionResult> Edit(int id)
        {
            var task = await _db.TaskItems
                .Include(t => t.Column)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (task == null) return NotFound();

            ViewBag.Assignees = await _db.Assignees
                .OrderBy(a => a.FullName)
                .ToListAsync();
            

            return View(task);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TaskItem model)
        {
            if (id != model.Id)
                return BadRequest();



            var existing = await _db.TaskItems.FindAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            existing.Title = model.Title;
            existing.Description = model.Description;
            existing.Priority = model.Priority;
            existing.DueDate = model.DueDate;
            existing.AssigneeId=model.AssigneeId;
            if (!ModelState.IsValid)
            {
                ViewBag.Assignees=await _db.Assignees
                    .OrderBy(a => a.FullName)
                    .ToListAsync();
                return View(model);
            }



            await _db.SaveChangesAsync();

            return RedirectToAction("Details", "Boards",
                new { id = existing.BoardId });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var task = await _db.TaskItems
                .Include(t => t.Column)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null) return NotFound();

            return View(task);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _db.TaskItems.FindAsync(id);
            if (task == null) return NotFound();

            int boardId = task.BoardId;

            _db.TaskItems.Remove(task);
            await _db.SaveChangesAsync();

            return RedirectToAction("Details", "Boards", new { id = boardId });
        }


        public async Task<IActionResult> Details(int id)
        {
            var task = await _db.TaskItems
                .Include(b => b.Column)
                .Include(b => b.Assignee)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (task == null)
                return NotFound();

            return View(task);

        }
    }
}
