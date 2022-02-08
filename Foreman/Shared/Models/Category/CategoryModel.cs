using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foreman.Shared.Models.Category
{
    public class CategoryModel
    {
        public int? Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(255)]
        public string Description { get; set; }
        [Required]
        public bool IsVisible { get; set; }

        public int? InstitutionId { get; set; }

        public int? ParentCategoryId { get; set; }
    }
}
