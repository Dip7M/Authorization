using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Models
{
    public class Parents
    {
        [Key]
        public int RegId { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Pwd { get; set; }
    }
}
