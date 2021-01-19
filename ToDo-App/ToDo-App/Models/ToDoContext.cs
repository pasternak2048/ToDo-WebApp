using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDo_App.Models
{
    public class ToDoContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ToDo> ToDos { get; set; }
        public ToDoContext(DbContextOptions<ToDoContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string adminRoleName = "admin";
            string userRoleName = "user";

            string adminEmail = "admin@gmail.com";
            string adminPassword = "123456";
            string adminFirstName = "Admin";
            string adminLastName = "One";
            string adminAddress = "Lviv";
            DateTimeOffset adminDateOfRegistration = DateTimeOffset.Now;


            
            Role adminRole = new Role { Id = 1, Name = adminRoleName };
            Role userRole = new Role { Id = 2, Name = userRoleName };
            User adminUser = new User
            {
                Id = 1,
                Email = adminEmail,
                Password = adminPassword,
                RoleId = adminRole.Id,
                FirstName = adminFirstName,
                LastName = adminLastName,
                Address = adminAddress,
                DateOfRegistration = adminDateOfRegistration
            };

            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole });
            modelBuilder.Entity<User>().HasData(new User[] { adminUser });
            base.OnModelCreating(modelBuilder);
        }

    }
}
