using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foreman.Shared.Data.Identity
{
    public class InstitutionRequest
    {
        public int UserId { get; set; }
        public int InstitutionId { get; set; }
        public bool? IsAccepted { get; set; }
        public DateTime RequestDate{ get; set; }
        public DateTime AnswerDate { get; set; }

        public Institution Institution { get; set; }
        public UserProfile User { get; set; }

    }
}
