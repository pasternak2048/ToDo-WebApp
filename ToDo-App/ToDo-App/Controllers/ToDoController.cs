using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo_App.Enums;
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
        public IActionResult Index(ToDoSortState sortOrder = ToDoSortState.DeadlineAsc, ToDoFilter filterOrder = ToDoFilter.OnlyOpenTasks, int page = 1)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var items = unitOfWork.ToDos.GetAll();

            ViewData["DeadlineSort"] = sortOrder == ToDoSortState.DeadlineAsc ? ToDoSortState.DeadlineDesc : ToDoSortState.DeadlineAsc;
            ViewData["FilterStatus"] = filterOrder == ToDoFilter.OnlyOpenTasks ? ToDoFilter.AllTasks : ToDoFilter.OnlyOpenTasks;

            items = GetFilteredByTaskStatus(items, filterOrder);

            if (currentUser.IsInRole("user"))
            {
                var count = items.Where(i => i.User.Id.ToString() == User.Identity.Name).Count();
                items = items.Where(i => i.User.Id.ToString() == User.Identity.Name)
                    .Skip((page - 1) * _pageSize).Take(_pageSize).ToList();

                items = GetSorted(items, sortOrder);

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
                var count = items.Count();
                items = items
                    .Skip((page - 1) * _pageSize).Take(_pageSize).ToList();

                items = GetSorted(items, sortOrder);

                var pageViewModel = new PageViewModel(count, page, _pageSize);
                var viewModel = new IndexViewModel<ToDo>
                {
                    PageViewModel = pageViewModel,
                    Items = items
                };

                return View(viewModel);
            }
        }


        public IActionResult Details(int? id, int page, ToDoFilter filterOrder)
        {
            ViewData["Page"] = page;
            ViewData["FilterStatus"] = filterOrder;
            if (id == null)
            {
                return NotFound();
            }

            var todo = unitOfWork.ToDos.Get(id);

            if (todo == null || todo.UserId.ToString() != User.Identity.Name
                && !User.IsInRole("admin"))
            {
                return NotFound();
            }

            return View(todo);
        }



        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public IActionResult Create(int page)
        {
            ViewData["Page"] = page;
            return View();
        }


        [HttpPost]
        [Authorize(Roles = "admin, user")]
        public IActionResult Create([Bind("Id,TaskName,TaskDescription,Deadline,UserId")] ToDo todo)
        {
            if (ModelState.IsValid)
            {
                todo.UserId = Convert.ToInt32(User.Identity.Name);
                unitOfWork.ToDos.Create(todo);
                unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }

            return View(todo);
        }



        [Authorize(Roles = "admin, user")]
        public IActionResult Edit(int? id, int page, ToDoFilter filterOrder)
        {
            ViewData["Page"] = page;
            ViewData["FilterStatus"] = filterOrder;

            if (id == null)
            {
                return NotFound();
            }

            var todo = unitOfWork.ToDos.Get(id);
            if (todo == null || todo.UserId.ToString() != User.Identity.Name
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
            if (id != todo.Id || todo.UserId.ToString() != User.Identity.Name
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
        public async Task<IActionResult> Delete(int? id, int page, ToDoFilter filterOrder)
        {
            ViewData["Page"] = page;
            ViewData["FilterStatus"] = filterOrder;

            if (id == null)
            {
                return NotFound();
            }

            var todo = unitOfWork.ToDos.Get(id);
            if (todo == null || todo.UserId.ToString() != User.Identity.Name
                && !User.IsInRole("admin"))
            {
                return NotFound();
            }

            return View(todo);
        }


        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "admin, user")]
        public IActionResult DeleteConfirmed(int id, int page)
        {
            ViewData["Page"] = page;

            unitOfWork.ToDos.Delete(id);
            unitOfWork.Save();
            return RedirectToAction("Index", page);
        }



        [Authorize(Roles = "admin, user")]
        public IActionResult MarkAsComplete(int? id, int page, ToDoSortState sortOrder, ToDoFilter filterOrder)
        {
            ToDo todo = unitOfWork.ToDos.Get(id);

            sortOrder = sortOrder == ToDoSortState.DeadlineAsc ? ToDoSortState.DeadlineDesc : ToDoSortState.DeadlineAsc;
            filterOrder =  filterOrder == ToDoFilter.OnlyOpenTasks ? ToDoFilter.OnlyOpenTasks : ToDoFilter.AllTasks;

            if (ModelState.IsValid && (todo.UserId.ToString() == User.Identity.Name
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

            return RedirectToAction("Index", new { page = page, sortOrder = sortOrder, filterOrder = filterOrder });
        }
        

        private bool ToDoExists(int id)
        {
            return unitOfWork.ToDos.GetAll().Any();
        }


        private IEnumerable<ToDo> GetSorted(IEnumerable<ToDo> items, ToDoSortState sortOrder)
        {
            items = sortOrder switch
            {
                ToDoSortState.DeadlineAsc => items.OrderBy(s => s.Deadline),
                ToDoSortState.DeadlineDesc => items.OrderByDescending(s => s.Deadline)
            };

            return items;
        }

        private IEnumerable<ToDo> GetFilteredByTaskStatus(IEnumerable<ToDo> items, ToDoFilter status)
        {
            items = status switch
            {
                ToDoFilter.OnlyOpenTasks => items.Where(i => i.IsCompleted == false),
                ToDoFilter.AllTasks => items
            };

            return items;
        }
    }
}
