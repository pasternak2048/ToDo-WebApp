using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo_App.Models;

namespace ToDo_App.Repositories
{
    public class UnitOfWork : IDisposable
    {
        private readonly ToDoContext _context;
        private MSSQLToDoRepository toDoRepository;
        private MSSQLUserRepository userRepository;


        public UnitOfWork(ToDoContext context)
        {
            _context = context;
        }


        public MSSQLToDoRepository ToDos
        {
            get
            {
                if (toDoRepository == null)
                    toDoRepository = new MSSQLToDoRepository(_context);
                return toDoRepository;
            }
        }

        public MSSQLUserRepository Users
        {
            get
            {
                if (userRepository == null)
                    userRepository = new MSSQLUserRepository(_context);
                return userRepository;
            }
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
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
