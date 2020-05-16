using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FamousQuoteQuiz.Controllers
{
    public class ErrorController : Controller
    {
        [Route("/error")]
        public IActionResult Error() => View("Error");
    }
}