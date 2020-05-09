using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FamousQuoteQuiz.Data;
using FamousQuoteQuiz.Models;
using FamousQuoteQuiz.Data.Interfaces;

namespace FamousQuoteQuiz.Controllers
{
    public class QuotesController : Controller
    {
        private readonly IQuotesRepository _quotesRepository;

        public QuotesController(IQuotesRepository quotesRepository)
        {
            _quotesRepository = quotesRepository;
        }

        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
             ViewData["NameSort"] = String.IsNullOrEmpty(sortOrder) ? "author" : "";
             ViewData["DescSort"] = sortOrder == "desc_descending" ? "desc" : "desc_descending";
             ViewData["CurrentFilter"] = searchString;

            List<Quote> quotes = new List<Quote>();

            await foreach (var quote in _quotesRepository.GetQuotes())
            {
                 quotes.Add(quote);
             }

            if (!String.IsNullOrEmpty(searchString))
            {
                quotes = quotes.Where(q => q.Author.Contains(searchString)).ToList();
            }

            switch (sortOrder)
                {
                    case "author":
                        quotes = quotes.OrderByDescending(q => q.Author).ToList();
                        break;
                    case "desc":
                        quotes = quotes.OrderBy(q => q.Description).ToList();
                        break;
                    case "desc_descending":
                        quotes = quotes.OrderByDescending(q => q.Description).ToList();
                        break;
                    default:
                        quotes = quotes.OrderBy(q => q.Author).ToList();
                        break;
                }

                return View(quotes.ToList());
        }

        public IActionResult Details(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quote = _quotesRepository.GetQuote(id);
            
            if (quote == null)
            {
                return NotFound();
            }

            return View(quote);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Description,Author")] Quote quote)
        {
            if (ModelState.IsValid)
            {
                quote.Id = Guid.NewGuid();
                _quotesRepository.Create(quote);

                return RedirectToAction(nameof(Index));
            }
            return View(quote);
        }

        public IActionResult Edit(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quote = _quotesRepository.GetQuote(id);

            if (quote == null)
            {
                return NotFound();
            }
            return View(quote);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Description,Author")] Quote quote)
        {
            if (id != quote.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _quotesRepository.Update(quote);
                }
                catch (DbUpdateConcurrencyException)
                {
                    var result = await QuoteExists(quote.Id);
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
            return View(quote);
        }

        public IActionResult Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quote = _quotesRepository.GetQuote(id);

            if (quote == null)
            {
                return NotFound();
            }

            return View(quote);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var quote = await _quotesRepository.GetQuote(id);
            _quotesRepository.Delete(quote);

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> QuoteExists(Guid id)
        {
            await foreach (var quote in _quotesRepository.GetQuotes())
            {
                if (id == quote.Id)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
