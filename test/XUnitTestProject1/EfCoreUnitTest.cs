using System;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using FsLib.EfCore.Application;
using FsLib.EfCore.Domain;
using FsLib.EfCore.EntityFrameworkCore;
using FsLib.EfCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Miko.Domain;
using Miko.Domain.Entity;
using Xunit;

namespace XUnitTestProject1
{
    public class EfCoreUnitTest
    {
        [Fact]
        public void Test1()
        {
            //string connectionStrings = "Server=47.99.199.184;Port=3306;charset=UTF8;Database=maccms10;User=root;Password=123456;SslMode=none;";
            //var services = new ServiceCollection();
            //// services.AddDbContext<AppDbContext>(options => options.UseMySql(connectionStrings, b => b.EnableRetryOnFailure()), ServiceLifetime.Transient);
            //services.AddDbContextPool<AppDbContext>(options => options.SetOptionsBuilder(connectionStrings, DbTypeEnum.MySql, nameof(Miko.Domain))); //数据库连接注入
            //services.AddTransient(typeof(IDbContext), typeof(AppDbContext));
            //services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));
            //services.AddTransient(typeof(IUserRepository), typeof(UserRepository));
            //var serviceProvider = services.BuildServiceProvider();

            //IUserRepository newsService = serviceProvider.GetService(typeof(IUserRepository)) as IUserRepository;
            //var v =newsService.Find(user => user.Id>0);

            var options = CreateDbContextOptions();
            var context = new AppDbContext(options);
            var model =  context.Set<MUser>().FirstOrDefault();

            Assert.True(true);
        }

        public DbContextOptions<AppDbContext> CreateDbContextOptions()
        {
            var serviceProvider = new ServiceCollection().AddEntityFrameworkMySql().BuildServiceProvider();
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder
                .UseMySql(
                    "Server=47.99.199.184;Port=3306;charset=UTF8;Database=maccms10;User=root;Password=123456;SslMode=none;")
                .UseInternalServiceProvider(serviceProvider);
            return builder.Options;

        }
    }
}