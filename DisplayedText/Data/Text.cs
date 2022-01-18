using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayedText.Data
{
    public class Text : Foreman.PluginManager.IPluginInstance
    {
        public int Id { get; set; }
        [Required]
        public string? Content { get; set; }
        public int CourseId { get; set; }
        public string? Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? UserCreatorId { get; set; }
    }
}
