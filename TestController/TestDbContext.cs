using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestController
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions options)
            : base(options)
        {

        }
        public DbSet<DisplayedText> DisplayedTexts { get; set; }
    }

    public class DisplayedText
    {
        public int Id { get; set; }
        public string? Text { get; set; }
    }
}
