using DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Impl
{
    public class UserRepository : IUserRepository
    {
        private readonly BiddifyDbContext dbContext;

        public UserRepository(BiddifyDbContext context)
        {
            dbContext = context;
        }

        public async Task<UserEntity> GetUserByEmailAsync(string email)
        {
            return await dbContext.Users
      .Where(u => u.NormalizedEmail == email.ToUpper())
      .FirstOrDefaultAsync();

        }

        public async Task<bool> UpdateUserAsync(UserEntity entity)
        {
            var existing = await dbContext.Users.FindAsync(entity.Id);
            if (existing == null) return false;

            existing.DisplayName = entity.DisplayName;
            //existing.Email = entity.Email;

            await dbContext.SaveChangesAsync();
            return true;
        }
    }

}
