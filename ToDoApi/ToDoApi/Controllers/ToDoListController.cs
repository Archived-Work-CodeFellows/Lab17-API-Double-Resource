using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoApi.Data;
using ToDoApi.Models;

namespace ToDoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoListController : ControllerBase
    {
        private ToDoDbContext _context;

        public ToDoListController(ToDoDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<ToDoList>> GetAll()
        {
            return _context.ToDoLists.ToList();
        }

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

        [HttpPost]
        public async Task<IActionResult> Create(ToDoList toDoList)
        {
            await _context.ToDoLists.AddAsync(toDoList);
            await _context.SaveChangesAsync();

            return CreatedAtRoute("GetList", new { id = toDoList.ID }, toDoList);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute]int id, [FromBody]ToDoList list)
        {
            var toDoList = await _context.ToDoLists.FindAsync(id);
            if(toDoList == null)
            {
                return RedirectToAction("Create", list);
            }

            toDoList.Name = list.Name;
            toDoList.IsDone = list.IsDone;
            _context.ToDoLists.Update(toDoList);
            await _context.SaveChangesAsync();
            return NoContent();
        }

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