using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace DataAccess
{
    public class UserEntity : IdentityUser
    {
        public string DisplayName { get; set; }
        public EUserStatus Status { get; set; } = EUserStatus.Inactive;
        public ERole Role { get; set; }
        public decimal Balance { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
