using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FamousQuoteQuiz.ViewModel
{
    [NotMapped]
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(128, MinimumLength = 6)]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool Remember { get; set; }
    }
}
