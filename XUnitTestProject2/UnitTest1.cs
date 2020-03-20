using Miko.Domain;
using Miko.Domain.Entity;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace XUnitTestProject2
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var options = CreateDbContextOptions();
            var context = new AppDbContext(options);
            var model = context.Set<MUser>().FirstOrDefault();

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
