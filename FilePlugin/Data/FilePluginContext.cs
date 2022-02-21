using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePlugin.Data
{
    public class FilePluginContext : DbContext
    {
        public FilePluginContext(DbContextOptions<FilePluginContext> options)
            : base(options)
        {

        }
        public DbSet<FilePluginInstance> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FilePluginInstance>().ToTable("FilePlugin_FilePluginInstance");
        }
    }
}
