using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo_App.Interfaces;
using ToDo_App.Models;

namespace ToDo_App.Repositories
{
    public class MSSQLUserRepository : IRepository<User>
    {
        private readonly ToDoContext _context;

        public MSSQLUserRepository(ToDoContext context)
        {
            _context = context;
        }

        public void Create(User item)
        {
            _context.Users.Add(item);
        }

        public User GetItem(int id)
        {
            return _context.Users.Find(id);
        }

        public void Delete(int id)
        {
            User user = _context.Users.Find(id);
            if (user != null)
                _context.Users.Remove(user);
        }

        public void Update(User item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        public IEnumerable<User> GetItemList()
        {
            return _context.Users;
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
