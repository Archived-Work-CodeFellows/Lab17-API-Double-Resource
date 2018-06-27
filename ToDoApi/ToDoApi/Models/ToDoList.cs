using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoApi.Models
{
    public class ToDoList
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public IEnumerable<ToDoItem> ToDoItems { get; set; }
        public bool IsDone { get; set; }
    }
}
