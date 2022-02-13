using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foreman.Shared.Models.Category
{
    public class ForemanFileModel : IForemanFileModel
    {
        public byte[] FileData { get; set; }
        public int ContextId { get; set; }
        public string Component { get; set; }
        public int? ItemId { get; set; }
        public int? UserId { get; set; }
        public string Filename { get; set; }
        public string MimeType { get; set; }
    }
}
