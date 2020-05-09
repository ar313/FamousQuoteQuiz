using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FamousQuoteQuiz.Data;
using FamousQuoteQuiz.Models;
using FamousQuoteQuiz.Data.Repository;
using FamousQuoteQuiz.Data.Interfaces;
using FamousQuoteQuiz.ViewModel;
using Microsoft.Data.SqlClient;

namespace FamousQuoteQuiz.Controllers
{
    public class UserAnswersController : Controller
    {
        private readonly IUserAnswerRepository _userAnswers;

        public UserAnswersController(IUserAnswerRepository userAnswers)
        {
            _userAnswers = userAnswers;
        }

        // GET: UserAnswers
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            var UserAnswersVM = new List<UserAnswersViewModel>();

            ViewData["AuthorSort"] = String.IsNullOrEmpty(sortOrder) ? "author" : "";
            ViewData["EmailSort"] = sortOrder == "email" ? "email_desc" : "email";
            ViewData["TimeSort"] = sortOrder == "time" ? "time_desc" : "time";
            ViewData["DescSort"] = sortOrder == "desc" ? "desc_desc" : "desc";
            ViewData["AnswerSort"] = sortOrder == "answer" ? "answer_desc" : "answer";
            ViewData["CurrentFilter"] = searchString;

            List<UserAnswer> users = new List<UserAnswer>();

            await foreach(var answer in _userAnswers.GetUserAnswers())
            {
                users.Add(answer);
            }

            if (users.Count() == 0) { return View(UserAnswersVM); }

            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.Quote.Author.Contains(searchString)
                                        || u.User.Email.Contains(searchString)).ToList();
            }

            users = Sorting(sortOrder, users);

            foreach (var UserAnswer in users)
            {
                UserAnswersViewModel user = new UserAnswersViewModel
                {
                    Id = UserAnswer.Id,
                    Answer = UserAnswer.Answer.ToString(),
                    Description = UserAnswer.Quote.Description,
                    Author = UserAnswer.Quote.Author,
                    Email = UserAnswer.User.Email,
                    AnswerTime = UserAnswer.AnswerTime
                };

                UserAnswersVM.Add(user);
            }

            return View(UserAnswersVM);
        }

        private List<UserAnswer> Sorting(string sortOrder, List<UserAnswer> userAnswers)
        {

            switch (sortOrder)
            {
                case "author":
                    userAnswers = userAnswers.OrderByDescending(u => u.Quote.Author).ToList();
                    break;
                case "email":
                    userAnswers = userAnswers.OrderBy(u => u.User.Email).ToList();
                    break;
                case "email_desc":
                    userAnswers = userAnswers.OrderByDescending(u => u.User.Email).ToList();
                    break;
                case "time":
                    userAnswers = userAnswers.OrderBy(u => u.AnswerTime).ToList();
                    break;
                case "time_desc":
                    userAnswers = userAnswers.OrderByDescending(u => u.AnswerTime).ToList();
                    break;
                case "desc":
                    userAnswers = userAnswers.OrderBy(u => u.Quote.Description).ToList();
                    break;
                case "desc_desc":
                    userAnswers = userAnswers.OrderByDescending(u => u.Quote.Description).ToList();
                    break;
                case "answer":
                    userAnswers = userAnswers.OrderBy(u => u.Answer).ToList();
                    break;
                case "answer_desc":
                    userAnswers = userAnswers.OrderByDescending(u => u.Answer).ToList();
                    break;

                default:
                    userAnswers = userAnswers.OrderBy(u => u.Quote.Author).ToList();
                    break;
            }

            return userAnswers;
        }

    }
}
