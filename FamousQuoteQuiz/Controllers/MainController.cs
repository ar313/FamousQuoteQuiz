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

        public MainController()
        {

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