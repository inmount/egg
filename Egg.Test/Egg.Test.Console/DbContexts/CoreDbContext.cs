using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Egg.Test.Console.Entities;

namespace SqliteEFCore.DbContexts
{
    public class CoreDbContext : DbContext
    {
        public virtual DbSet<People> Peoples { get; set; }
        public virtual DbSet<People2> People2s { get; set; }

        public CoreDbContext(DbContextOptions options) : base(options)
        {
            //_connStr = connStr;
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //optionsBuilder.UseSqlite(_connStr);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
