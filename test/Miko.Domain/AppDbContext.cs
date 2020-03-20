using System;
using System.Linq;
using System.Reflection;
using FsLib.EfCore.Domain;
using Microsoft.EntityFrameworkCore;
using Miko.Domain.Entity;

namespace Miko.Domain
{
    public class AppDbContext : DbContext, IDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseNpgsql(
                //    @"server=www.shangjin666.cn,1443;database=stock_qiquan;user id=Mdkj369;password=Mdkj123;");
            }
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MUser>();
            builder.Entity<MBgm>();
            builder.Entity<MType>();
            builder.Entity<MVideo>();
            builder.Entity<MAccount>();
            builder.Entity<MVideoDetail>();
            builder.Entity<MDecade>();
            builder.Entity<MArea>();

            base.OnModelCreating(builder);
        }

    }
}
