using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ToDoApi.Data;
using ToDoApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        public ActionResult<ToDoItem> GetById(long id)
        {
            var item = _context.ToDoItems.Find(id);
            if(item == null)
            {
                return NotFound();
            }
            return item;
        }
    }
}
