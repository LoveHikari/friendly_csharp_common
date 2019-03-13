using System;
using System.Linq;
using System.Reflection;
using FsLib.FreeSql.Domain;
using Microsoft.EntityFrameworkCore;

namespace FsLib.FreeSql.Infrastructure
{
    public class AppDbContext : DbContext, IDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                throw new Exception("未配置数据库连接");
                //optionsBuilder.UseMySQL(
                //    @"server=www.shangjin666.cn,1443;database=stock_qiquan;user id=Mdkj369;password=Mdkj123;");
            }
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Type type = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod().DeclaringType;  //当前类型
            Assembly assembyle = Assembly.GetAssembly(typeof(IAggregateRoot));  //查找类库
            var typeList = assembyle.GetTypes().Where(t => ((TypeInfo)t).ImplementedInterfaces.Contains(typeof(IAggregateRoot)));  //查找该命名空间下实现了接口的所有类型
            foreach (Type type1 in typeList)
            {
                builder.Entity(type1);
            }
            //builder.Entity<Student>();
            // builder.Entity<Teacher>();

            base.OnModelCreating(builder);
        }

        //public DbSet<Student> Students { get; set; }
        //public DbSet<Teacher> Teachers { get; set; }

    }
}