using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamousQuoteQuiz.Data.Interfaces;
using FamousQuoteQuiz.Models;
using FamousQuoteQuiz.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FamousQuoteQuiz.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["NameSort"] = String.IsNullOrEmpty(sortOrder) ? "name" : "";
            ViewData["EmailSort"] = sortOrder == "email_desc" ? "email" : "email_desc";
            ViewData["DateSort"] = sortOrder == "date_desc" ? "date" : "date_desc";
            ViewData["CurrentFilter"] = searchString;

            var UsersVM = new List<UserViewModel>();

            var Users = _userRepository.GetUsers();

            await foreach(var User in _userRepository.GetUsers())
            {
                UsersVM.Add(new UserViewModel
                {
                    Id = User.Id,
                    Email = User.Email,
                    Name = User.Name,
                    JoinedDate = User.JoinedDate,
                });
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                UsersVM = UsersVM.Where(u => u.Name.Contains(searchString) 
                                        || u.Email.Contains(searchString)).ToList();
            }
            

            switch (sortOrder)
            {
                case "name":
                    UsersVM = UsersVM.OrderByDescending( u => u.Name).ToList();
                    break;
                case "email":
                    UsersVM = UsersVM.OrderBy(u => u.Email).ToList();
                    break;
                case "email_desc":
                    UsersVM = UsersVM.OrderByDescending(u => u.Email).ToList();
                    break;
                case "date":
                    UsersVM = UsersVM.OrderBy(u => u.JoinedDate).ToList();
                    break;
                case "date_desc":
                    UsersVM = UsersVM.OrderByDescending(u => u.JoinedDate).ToList();
                    break;
                default:
                    UsersVM = UsersVM.OrderBy(u => u.Name).ToList();
                    break;
            }


            return View(UsersVM);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var u =  await _userRepository.GetUser(id); 

            if (u == null)
            {
                return NotFound();
            }

            UserViewModel user = new UserViewModel
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                JoinedDate = u.JoinedDate,
            };

            return View(user);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var u = await _userRepository.GetUser(id);

            if (u == null)
            {
                return NotFound();
            }

            UserViewModel user = new UserViewModel
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                JoinedDate = u.JoinedDate
            };


            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Email, JoinedDate")] UserViewModel user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    User u = await _userRepository.GetUser(id);
                    u.Email = user.Email;
                    u.Name = user.Name;
                    u.JoinedDate = user.JoinedDate;
                    _userRepository.Update(u);
                }
                catch (DbUpdateConcurrencyException)
                {
                    var result = await UserExists(user.Id);
                    if (!result)
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
            return View(user);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var u = await _userRepository.GetUser(id);

            if (u == null)
            {
                return NotFound();
            }

            UserViewModel user = new UserViewModel
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                JoinedDate = u.JoinedDate
            };


            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userRepository.GetUser(id);
            _userRepository.Delete(user);

            return RedirectToAction(nameof(Index));
        }

        public async Task<bool> UserExists(string id)
        {
            await foreach (var User in _userRepository.GetUsers())
            {
                if(id==User.Id)
                {
                    return true;
                }
            }

            return false;
        }
    }
}