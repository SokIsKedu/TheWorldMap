using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWorld.ViewModels
{
    public class ContactViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [MinLength(5,ErrorMessage = "Message should be at least 5 characters long")]
        [Required]
        public string Message { get; set; }
    }
}
