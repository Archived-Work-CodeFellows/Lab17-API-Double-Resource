using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Data;
using ToDoApi.Models;

namespace ToDoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoListController : ControllerBase
    {
        private ToDoDbContext _context;
        /// <summary>
        /// This allows us to use dependency injection
        /// </summary>
        /// <param name="context">our dbcontext variable</param>
        public ToDoListController(ToDoDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Action that returns all available lists
        /// </summary>
        /// <returns>List of ToDoLists</returns>
        [HttpGet]
        public ActionResult<List<ToDoList>> GetAll()
        {
            return _context.ToDoLists.ToList();           
        }
        /// <summary>
        /// Action that returns a specific list and what tasks
        /// are associated with that list
        /// </summary>
        /// <param name="id">id of list</param>
        /// <returns>ToDoList or NotFound</returns>
        [HttpGet("{id}", Name = "GetList")]
        public async Task<ActionResult<ToDoList>> GetById([FromRoute]int id)
        {
            ToDoList toDoList = await _context.ToDoLists.FindAsync(id);
            if (toDoList == null)
            {
                return NotFound();
            }
            var todos = _context.ToDoItems.Where(i => i.ListID == id).ToList();
            toDoList.ToDoItems = todos;

            return Ok(toDoList);
        }
        /// <summary>
        /// Action that creates a ToDoList
        /// </summary>
        /// <param name="toDoList">Newly created toDoList from the body</param>
        /// <returns>CreatedAtRoute status and redirects to a get</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]ToDoList toDoList)
        {
            await _context.ToDoLists.AddAsync(toDoList);
            await _context.SaveChangesAsync();

            return CreatedAtRoute("GetList", new { id = toDoList.ID }, toDoList);
        }
        /// <summary>
        /// Action that updates the give list
        /// </summary>
        /// <param name="id">Id of list</param>
        /// <param name="list">ToDoList object with data from the body</param>
        /// <returns>NoContent if succeeded</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute]int id, [FromBody]ToDoList list)
        {
            ToDoList toDoList = await _context.ToDoLists.FindAsync(id);
            if(toDoList == null)
            {
                return RedirectToAction("Create", list);
            }
            list.ID = id;
            _context.Entry(toDoList).State = EntityState.Detached;

            toDoList = list;
            _context.ToDoLists.Update(toDoList);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        /// <summary>
        /// Action that will remove specific list and all todos
        /// associated with that list
        /// </summary>
        /// <param name="id">Id of the list</param>
        /// <returns>NotFound if null or NoContent if success</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            var toDoList = await _context.ToDoLists.FindAsync(id);
            if(toDoList == null || id == 1)
            {
                return NotFound();
            }

            var toDoItems = _context.ToDoItems.Where(i => i.ListID == id).ToList();

            foreach(var item in toDoItems)
            {
                _context.ToDoItems.Remove(item);
            }
            _context.ToDoLists.Remove(toDoList);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}