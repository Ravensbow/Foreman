﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayedText.Data
{
    public class DisplayedTextContext : DbContext
    {
        public DisplayedTextContext(DbContextOptions options)
            : base(options)
        {

        }
        public DbSet<Text> Texts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Text>().ToTable("DisplayedText_Text");
        }
    }
}
