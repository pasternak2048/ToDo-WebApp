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

        public void Create(ToDo item)
        {
            _context.ToDos.Add(item);
        }

        public ToDo GetItem(int id)
        {
            return _context.ToDos.Find(id);
        }

        public void Delete(int id)
        {
            ToDo todo = _context.ToDos.Find(id);
            if (todo != null)
                _context.ToDos.Remove(todo);
        }

        public void Update(ToDo item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        public IEnumerable<ToDo> GetItemList()
        {
            return _context.ToDos;
        }

        public void Save()
        {
            _context.SaveChanges();
        }


        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
