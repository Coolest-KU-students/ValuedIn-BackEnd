namespace ValuedInBE.System.Security.Users
{
    internal static class UserRoleHierarchyExtension
    {
        private static readonly Dictionary<UserRoleExtended, HashSet<UserRoleExtended>> hierarchy = new();

        private static void Includes(UserRoleExtended userRole, params UserRoleExtended[] inheritedRoles)
        {
            if (inheritedRoles.Contains(userRole))
                throw new Exception($"User Role {userRole} cannot inherit itself");
            hierarchy.Add(userRole, inheritedRoles.ToHashSet());
        }

        public static HashSet<UserRoleExtended> GetIncludedRoles(this UserRoleExtended userRole)
        {
            return new(hierarchy[userRole]);
        }

        static UserRoleHierarchyExtension() 
        {
            Includes(UserRole.DEFAULT); //UserRole.DEFAULT does not inherit anything
            Includes(UserRole.HR, UserRole.DEFAULT);
            Includes(UserRole.ORG_ADMIN, UserRole.HR);
            Includes(UserRole.SYS_ADMIN);//UserRole.SYS_ADMIN does not inherit anthing
        }
    }
}
