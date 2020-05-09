using FamousQuoteQuiz.Data.Interfaces;
using FamousQuoteQuiz.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamousQuoteQuiz.Data.Repository
{
    public class QuotesRepository : Repository<Quote>, IQuotesRepository
    {
        public QuotesRepository(QuizDbContext context) : base(context)
        {

        }

        public Quote GetAuthor(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Quote> GetQuote(Guid id)
        {
            return await _context.Quote.SingleOrDefaultAsync(q => q.Id == id);
        }

        public async IAsyncEnumerable<Quote> GetQuotes()
        {
            var Quotes = _context.Quote.ToListAsync();

            foreach(var quote in await Quotes)
            {
                yield return quote;
            }
        }
    }
}
