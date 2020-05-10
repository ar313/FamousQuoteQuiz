using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamousQuoteQuiz.Data.Interfaces;
using FamousQuoteQuiz.Data.Repository;
using FamousQuoteQuiz.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FamousQuoteQuiz.Controllers
{
    public class QuotesAPIController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IQuotesRepository _quotesRepository;
        private readonly IUserAnswerRepository _userAnswer;

        public QuotesAPIController(IQuotesRepository quoteRepository, IUserAnswerRepository userAnswer, UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _quotesRepository = quoteRepository;
            _userAnswer = userAnswer;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Quote()
        {
            List<Quote> quotes = new List<Quote>(); 
            
            await foreach(var quote in  _quotesRepository.GetQuotes())
            {
                quotes.Add(quote);
            }
            
            if (quotes.Count() == 0) { return NotFound(); }
            var response = quotes.OrderBy(r => Guid.NewGuid()).Take(1).First();
           
            response.Author = "";

            return Ok(Json(response));
        }

        [HttpGet]
        public async Task<IActionResult> Author(int mode, Guid id)
        {

            List<Quote> quotes = new List<Quote>();

            await foreach (var quote in _quotesRepository.GetQuotes())
            {
                quotes.Add(quote);
            }

            if (mode == 1)
            {
                Quote response = quotes.OrderBy(r => Guid.NewGuid()).Take(2).First();
                string author = response.Author;
                
                return Ok(Json(author));
            }
            else if (mode == 2)
            {
                List<Quote> quote = new List<Quote>(quotes.Where(q => q.Id != id).OrderBy(r => Guid.NewGuid()).Take(2));
                Quote answer = await _quotesRepository.GetQuote(id);
                List<string> authors = new List<string>();

                authors.Add(answer.Author);
                authors.AddRange(quote.Select(q => q.Author));
                authors = authors.OrderBy(a => Guid.NewGuid()).ToList();

                return Ok(Json(authors));
            }
            else
            {
                Quote response = quotes.OrderBy(r => Guid.NewGuid()).Take(2).First();
                string author = response.Author;

                return Ok(Json(author));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Answer(Guid id)
        {
            //Guid gId = Guid.Parse(id);
            Quote quoteOnPage = await _quotesRepository.GetQuote(id);
            
            string response = quoteOnPage.Author;

            return Ok(Json(response));
        }

        [HttpPost]
        public async Task<IActionResult> SaveAnswer(Guid id, string result)
        {
            
            if(!User.Identity.IsAuthenticated)
            {
                return NotFound();
            }

            User toAdd = await _userManager.GetUserAsync(User);
            Quote quoteOnPage = await _quotesRepository.GetQuote(id);
            bool answer = result == "true" ? true : false;
            
            UserAnswer userAnswer = new UserAnswer { Id = Guid.NewGuid(), User = toAdd, Quote = quoteOnPage, Answer = answer, AnswerTime = DateTime.Now };
            _userAnswer.Create(userAnswer);

            return Ok(Json("Success"));
        }
    }
}   