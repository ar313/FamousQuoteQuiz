using FamousQuoteQuiz.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamousQuoteQuiz.ViewModel;

namespace FamousQuoteQuiz.Data
{
    public class QuizDbContext : IdentityDbContext<User>
    {
        public QuizDbContext() { }

        public QuizDbContext(DbContextOptions<QuizDbContext> options)
        : base(options)
        {

        }

        public DbSet<User> User { get; set; }
        public DbSet<Quote> Quote { get; set; }
        public DbSet<UserAnswer> UserAnswer { get; set; }
        public DbSet<FamousQuoteQuiz.ViewModel.UserAnswersViewModel> UserAnswersViewModel { get; set; }
    }
}
