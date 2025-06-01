using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Impl
{
    public class UserRepository : IUserRepository
    {
        public Task<UserEntity> GetUserByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUserAsync(UserEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
