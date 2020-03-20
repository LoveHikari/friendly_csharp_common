using FsLib.EfCore.Domain;
using FsLib.EfCore.Repository;
using Miko.Domain.Entity;

namespace XUnitTestProject1
{
    public class UserRepository : BaseRepository<MUser>, IUserRepository
    {
        public UserRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}