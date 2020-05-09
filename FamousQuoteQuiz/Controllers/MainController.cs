using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FamousQuoteQuiz.Data;
using FamousQuoteQuiz.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FamousQuoteQuiz.Controllers
{
    public class MainController : Controller
    {
        private readonly QuizDbContext _context;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;


        public MainController(QuizDbContext context, UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View("Main");
        }

        public IActionResult Settings()
        {
            return View();
        }

    }
}