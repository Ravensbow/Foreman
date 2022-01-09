using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foreman.Shared.Data.Identity
{
    public class Institution
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int OwnerId { get; set; }
    }
}
