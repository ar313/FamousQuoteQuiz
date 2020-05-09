using FamousQuoteQuiz.Data.Interfaces;
using FamousQuoteQuiz.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamousQuoteQuiz.Data.Repository
{
    public class UserAnswerRepository : Repository<UserAnswer>, IUserAnswerRepository
    {
        public UserAnswerRepository(QuizDbContext context): base(context)
        {

        }
        public async IAsyncEnumerable<UserAnswer> GetUserAnswers()
        {
            var UserAnswers = _context.UserAnswer.Include(a => a.Quote).Include(a => a.User).ToListAsync();

            foreach(var User in await UserAnswers)
            {
                yield return User;
            }
        }
    }
}
