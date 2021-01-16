using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo_App.Interfaces;
using ToDo_App.Models;

namespace ToDo_App.Repositories
{
    public class MSSQLToDoRepository : IRepository<ToDo>
    {
        private readonly ToDoContext _context;

        public MSSQLToDoRepository(ToDoContext context)
        {
            _context = context;
        }

        public IEnumerable<ToDo> GetAll()
        {
            return _context.ToDos.Include(x=>x.User);
        }

        public ToDo Get(int? id)
        {
            return _context.ToDos.Include(x=>x.User)
                .FirstOrDefault(y=>y.Id == id);
        }


        public void Create(ToDo item)
        {
            _context.ToDos.Add(item);
        }

        public void Update(ToDo item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }


        public void Delete(int id)
        {
            ToDo todo = _context.ToDos.Find(id);
            if (todo != null)
                _context.ToDos.Remove(todo);
        }
    }
}
