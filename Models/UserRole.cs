using System;
using System.Collections.Generic;

namespace MobileBasedCashFlowAPI.Models
{
    public partial class UserRole
    {
        public UserRole()
        {
            UserAccounts = new HashSet<UserAccount>();
        }

        public string RoleId { get; set; } = null!;
        public string RoleName { get; set; } = null!;
        public DateTime CreateAt { get; set; }

        public virtual ICollection<UserAccount> UserAccounts { get; set; }
    }
}
