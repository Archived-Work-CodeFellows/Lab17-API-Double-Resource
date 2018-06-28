using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoApi.Models
{
    public class ToDoItem
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public bool IsDone { get; set; }
        public long ListID { get; set; }
        public string ToDoList { get; set; }
    }
}
