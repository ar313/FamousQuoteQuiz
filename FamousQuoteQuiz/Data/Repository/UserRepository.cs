using FamousQuoteQuiz.Data.Interfaces;
using FamousQuoteQuiz.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamousQuoteQuiz.Data.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(QuizDbContext context) : base(context)
        {

        }

        public async Task<User> GetUser(string id)
        {
            return await _context.User.SingleOrDefaultAsync(q => q.Id == id);
        }

        public async IAsyncEnumerable<User> GetUsers()
        {
            var users = _context.User.ToListAsync();

            foreach (var user in await users)
            {
               yield return user;
            }
        }
    }
}
