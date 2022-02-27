using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Foreman.Shared.Data.Identity;

namespace Foreman.Shared.Models.Account
{
    public class EditModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        public Institution Institution { get; set; }

    }
}
