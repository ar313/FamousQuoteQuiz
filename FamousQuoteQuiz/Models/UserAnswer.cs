using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FamousQuoteQuiz.Models
{
    public class UserAnswer
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        public bool Answer { get; set; }

        [Required]
        public DateTime AnswerTime { get; set; }

        [Required]
        public virtual User User { get; set; }

        [Required]
        public virtual Quote Quote { get; set; }
    }
}
