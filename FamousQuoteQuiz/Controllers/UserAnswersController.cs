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
        public async Task<IActionResult> Index()
        {
            var UserAnswersVM = new List<UserAnswersViewModel>();

            List<UserAnswer> users = new List<UserAnswer>();

            await foreach(var answer in _userAnswers.GetUserAnswers())
            {
                users.Add(answer);
            }

            if (users.Count() == 0) { return View(UserAnswersVM); }

            foreach(var UserAnswer in users)
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

    }
}
