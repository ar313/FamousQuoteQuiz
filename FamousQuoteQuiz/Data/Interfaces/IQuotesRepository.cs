using FamousQuoteQuiz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamousQuoteQuiz.Data.Interfaces
{
    public interface IQuotesRepository : IRepository<Quote>
    {
        IAsyncEnumerable<Quote>  GetQuotes();

        Task<Quote> GetQuote(Guid id);
    }
}
