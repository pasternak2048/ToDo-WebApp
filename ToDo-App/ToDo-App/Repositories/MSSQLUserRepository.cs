﻿using Microsoft.EntityFrameworkCore;
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

        public IEnumerable<User> GetAll()
        {
            return _context.Users.AsNoTracking().Include(x=>x.Role);
        }


        public User Get(int? id)
        {
            return _context.Users.AsNoTracking().Include(x => x.Role)
                .FirstOrDefault(y => y.Id == id);
        }


        public void Create(User item)
        {
            _context.Users.Add(item);
        }

        public void Update(User item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }


        public void Delete(int id)
        {
            User user = _context.Users.Find(id);
            if (user != null)
            {
                List<ToDo> toDoList = _context.ToDos.Where(x => x.UserId == id).ToList();
                _context.ToDos.RemoveRange(toDoList);
                _context.Users.Remove(user);
            }
        }
    }
}
