using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foreman.Shared.Data.Courses
{
    public class ForemanFile
    {
        public int Id { get; set; }
        public string ContentHash { get; set; }
        public string PathNameHash { get; set; }
        public int ContextId { get; set; }
        public string Component { get; set; }
        public int? ItemId { get; set; }
        public int? UserId { get; set; }
        public string Filename { get; set; }
        public string MimeType { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime ModifiedTime { get; set; }
    }
}
