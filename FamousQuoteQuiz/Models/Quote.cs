using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FamousQuoteQuiz.Models
{
    public class Quote
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Description { get; set; }
        
        [Required]
        public string Author { get; set; } 
    }
}
