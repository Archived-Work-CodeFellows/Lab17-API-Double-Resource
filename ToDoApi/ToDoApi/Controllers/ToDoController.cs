using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Data;
using ToDoApi.Models;

namespace ToDoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly ToDoDbContext _context;
        /// <summary>
        /// This allows us to use dependency injection
        /// </summary>
        /// <param name="context">Our dbcontext variable</param>
        public ToDoController(ToDoDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Action that allows us to view all todo items
        /// </summary>
        /// <returns>List of ToDos</returns>
        [HttpGet]
        public ActionResult<List<ToDoItem>> GetAll()
        {
            return _context.ToDoItems.ToList();
        }
        /// <summary>
        /// Action that allows us to get a ToDo item
        /// by its specific id
        /// </summary>
        /// <param name="id">Id of todo itme</param>
        /// <returns>Null if it doesnt exist or the item</returns>
        [HttpGet("{id}", Name = "GetToDo")]
        public ActionResult<ToDoItem> GetById([FromRoute]long id)
        {
            var item = _context.ToDoItems.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            ToDoList toDoList = _context.ToDoLists.First(l => l.ID == item.ListID);
            item.ToDoList = toDoList.Name;
            return item;
        }
        /// <summary>
        /// Action that allows us to create a new todo
        /// </summary>
        /// <param name="item">ToDoItem object of data from the body</param>
        /// <returns>CreatedAtRoute status and redirects to a get</returns>
        [HttpPost]
        public IActionResult Create([FromBody]ToDoItem item)
        {
            if(item.ListID == 0)
            {
                item.ListID = 1;
            }
            _context.ToDoItems.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetToDo", new { id = item.ID }, item);
        }
        /// <summary>
        /// Action that allows us to update a ToDo item
        /// </summary>
        /// <param name="id">specific todo item id</param>
        /// <param name="item">ToDoItem object with data from the body</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute]long id, [FromBody]ToDoItem item)
        {
            ToDoItem todo = await _context.ToDoItems.FindAsync(id);

            if (todo == null)
            {
                return RedirectToAction("Create", item);
            }
            _context.Entry(todo).State = EntityState.Detached;
            todo = item;

            _context.ToDoItems.Update(todo);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        /// <summary>
        /// Action that allows us to remove a specific todo item
        /// </summary>
        /// <param name="id">id f todoitem</param>
        /// <returns>NoContent if success, NotFound if null</returns>
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
