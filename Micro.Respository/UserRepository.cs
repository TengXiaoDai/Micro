using Micro.Models;

namespace Micro.Respository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(MicroContext context) : base(context)
        {

        }
    }
}
