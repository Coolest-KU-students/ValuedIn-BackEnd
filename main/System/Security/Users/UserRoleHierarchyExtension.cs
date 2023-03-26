namespace ValuedInBE.System.Security.Users
{
    internal static class UserRoleHierarchyExtension
    {
        private static readonly Dictionary<UserRoleExtended, HashSet<UserRoleExtended>> _hierarchy = new();

        private static void Inherits(UserRoleExtended userRoleThatInherits, params UserRoleExtended[] inheritedRoles)
        {
            if (inheritedRoles.Contains(userRoleThatInherits))
                throw new ApplicationException($"User Role {userRoleThatInherits} cannot inherit itself");
            _hierarchy.Add(userRoleThatInherits, inheritedRoles.ToHashSet());
        }

        public static HashSet<UserRoleExtended> GetInherittedRoles(this UserRoleExtended userRole)
        {
            return new(_hierarchy[userRole]);
        }

        static UserRoleHierarchyExtension() 
        {
            Inherits(UserRole.DEFAULT); //UserRole.DEFAULT does not inherit anything
            Inherits(UserRole.HR, UserRole.DEFAULT);
            Inherits(UserRole.ORG_ADMIN, UserRole.HR);
            Inherits(UserRole.SYS_ADMIN);//UserRole.SYS_ADMIN does not inherit anthing
        }
    }
}
