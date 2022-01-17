using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayedText.Data
{
    public class Text
    {
        public int Id { get; set; }
        public string? Content { get; set; }
    }
}
