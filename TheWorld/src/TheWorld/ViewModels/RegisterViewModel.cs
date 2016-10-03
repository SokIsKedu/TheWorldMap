using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWorld.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; }

        [MinLength(8, ErrorMessage = "Password should be at least 8 characters long")]
        [Required]
        public string Password { get; set; }


        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
