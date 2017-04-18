using System.Collections.Generic;
using System.Linq;

namespace mmmsl.Models
{
    public static class AppRoles
    {
        public const string Administrator = "admin";
        public const string Manager = "manager";
        public const string Player = "player";

        public static bool HasRole(this IEnumerable<Role> roles, string roleName)
        {
            return roles.Any(role => role.Name == roleName);
        }

        public static Role GetRole(this IEnumerable<Role> roles, string roleName)
        {
            return roles.SingleOrDefault(role => role.Name == roleName);
        }
    }
}
