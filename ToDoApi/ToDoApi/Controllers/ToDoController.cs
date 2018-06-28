using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ToDoApi.Data;
using ToDoApi.Models;

namespace ToDoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly ToDoDbContext _context;

        public ToDoController(ToDoDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<ToDoItem>> GetAll()
        {
            return _context.ToDoItems.ToList();
        }

        [HttpGet("{id}", Name = "GetToDo")]
        public ActionResult<ToDoItem> GetById([FromRoute]long id)
        {
            var item = _context.ToDoItems.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        [HttpPost]
        public IActionResult Create(ToDoItem item)
        {
            _context.ToDoItems.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetToDo", new { id = item.ID }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute]long id, [FromBody]ToDoItem item)
        {
            var todo = await _context.ToDoItems.FindAsync(id);

            if (todo == null)
            {
                return RedirectToAction("Create", item);
            }

            todo.IsDone = item.IsDone;
            todo.Name = item.Name;
            todo.ListID = item.ListID;

            _context.ToDoItems.Update(todo);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]long id)
        {
            var todo = await _context.ToDoItems.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }
            _context.ToDoItems.Remove(todo);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
