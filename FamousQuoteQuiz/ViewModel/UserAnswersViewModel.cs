using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FamousQuoteQuiz.ViewModel
{
    [NotMapped]
    public class UserAnswersViewModel
    {

        [Required]
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        public string Description { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Answer { get; set; }

        [Required]
        public DateTime AnswerTime { get; set; }

    }
}
