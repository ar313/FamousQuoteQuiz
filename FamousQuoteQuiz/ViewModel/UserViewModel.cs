using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FamousQuoteQuiz.ViewModel
{
    [NotMapped]
    public class UserViewModel 
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name="Joined Date")]
        public DateTime JoinedDate { get; set; }

        public bool isDisabled { get; set; }


    }
}
