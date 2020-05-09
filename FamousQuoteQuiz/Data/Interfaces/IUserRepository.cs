using FamousQuoteQuiz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamousQuoteQuiz.Data.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        public IAsyncEnumerable<User> GetUsers();

        Task<User> GetUser(string id);
    }
}
