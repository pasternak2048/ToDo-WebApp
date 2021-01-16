using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo_App.Models;
using ToDo_App.Models.Abstractions;
using ToDo_App.Repositories;

namespace ToDo_App.Controllers
{
    public class ToDoController : Controller
    {
        UnitOfWork unitOfWork;
        private const int _pageSize = 8;

        public ToDoController(ToDoContext context)
        {
            unitOfWork = new UnitOfWork(context);
        }

        [Authorize(Roles = "admin, user")]
        public IActionResult Index(int page = 1)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;

            if (currentUser.IsInRole("user"))
            {
                var count = unitOfWork.ToDos.GetAll().Where(i => i.User.Email == User.Identity.Name).Count();
                var items = unitOfWork.ToDos.GetAll().Where(i => i.User.Email == User.Identity.Name)
                    .Skip((page - 1) * _pageSize).Take(_pageSize).ToList();

                var pageViewModel = new PageViewModel(count, page, _pageSize);
                var viewModel = new IndexViewModel<ToDo>
                {
                    PageViewModel = pageViewModel,
                    Items = items
                };

                
                return View(viewModel);
            }

            else
            {
                var count = unitOfWork.ToDos.GetAll().Count();
                var items = unitOfWork.ToDos.GetAll()
                    .Skip((page - 1) * _pageSize).Take(_pageSize).ToList();

                var pageViewModel = new PageViewModel(count, page, _pageSize);
                var viewModel = new IndexViewModel<ToDo>
                {
                    PageViewModel = pageViewModel,
                    Items = items
                };

                return View(viewModel);
            }
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = unitOfWork.ToDos.Get(id);
                
            if (todo == null || todo.UserId != unitOfWork.Users.GetAll().FirstOrDefault(x => x.Email == User.Identity.Name).Id
                && !User.IsInRole("admin"))
            {
                return NotFound();
            }

            return View(todo);
        }



        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [Authorize(Roles = "admin, user")]
        public IActionResult Create([Bind("Id,TaskName,TaskDescription,Deadline,UserId")] ToDo todo)
        {
            if (ModelState.IsValid)
            {
                todo.UserId = unitOfWork.Users.GetAll().FirstOrDefault(x => x.Email == User.Identity.Name).Id;
                unitOfWork.ToDos.Create(todo);
                unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }

            return View(todo);
        }



        [Authorize(Roles = "admin, user")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo =  unitOfWork.ToDos.Get(id);
            if (todo == null || todo.UserId != unitOfWork.Users.GetAll().FirstOrDefault(x => x.Email == User.Identity.Name).Id
                && !User.IsInRole("admin"))
            {
                return NotFound();
            }
            
            return View(todo);
        }

       
        [HttpPost]
        [Authorize(Roles = "admin, user")]
        public IActionResult Edit(int id, [Bind("Id,TaskName,TaskDescription,Deadline,IsCompleted,UserId")] ToDo todo)
        {
            if (id != todo.Id || todo.UserId != unitOfWork.Users.GetAll().FirstOrDefault(x => x.Email == User.Identity.Name).Id
                && !User.IsInRole("admin"))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    unitOfWork.ToDos.Update(todo);
                    unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToDoExists(todo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            
            return View(todo);
        }



        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = unitOfWork.ToDos.Get(id);
            if (todo == null || todo.UserId != unitOfWork.Users.GetAll().FirstOrDefault(x => x.Email == User.Identity.Name).Id
                && !User.IsInRole("admin"))
            {
                return NotFound();
            }

            return View(todo);
        }
 

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "admin, user")]
        public IActionResult DeleteConfirmed(int id)
        {
            unitOfWork.ToDos.Delete(id);
            unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }



        [Authorize(Roles = "admin, user")]
        public IActionResult MarkAsComplete(int? id)
        {
            ToDo todo = unitOfWork.ToDos.Get(id);

            if (ModelState.IsValid && (todo.UserId == unitOfWork.Users.GetAll().FirstOrDefault(x => x.Email == User.Identity.Name).Id
                || User.IsInRole("admin")))
            {
                try
                {
                    todo.IsCompleted = !todo.IsCompleted;
                    unitOfWork.ToDos.Update(todo);
                    unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToDoExists(todo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }
        private bool ToDoExists(int id)
        {
            return unitOfWork.ToDos.GetAll().Any();
        }
    }
}
