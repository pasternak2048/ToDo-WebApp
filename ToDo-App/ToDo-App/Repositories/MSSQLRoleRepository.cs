using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo_App.Interfaces;
using ToDo_App.Models;

namespace ToDo_App.Repositories
{
    public class MSSQLRoleRepository : IRepository<Role>
    {
        private readonly ToDoContext _context;

        public MSSQLRoleRepository(ToDoContext context)
        {
            _context = context;
        }

        public IEnumerable<Role> GetAll()
        {
            return _context.Roles;
        }

        public Role Get(int id)
        {
            return _context.Roles.Find(id);
        }


        public void Create(Role item)
        {
            _context.Roles.Add(item);
        }

        public void Update(Role item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }


        public void Delete(int id)
        {
            Role role = _context.Roles.Find(id);
            if (role != null)
                _context.Roles.Remove(role);
        }
    }
}
