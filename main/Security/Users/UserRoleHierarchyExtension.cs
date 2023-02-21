namespace ValuedInBE.Security.Users
{
    internal static class UserRoleHierarchyExtension
    {
        private static readonly Dictionary<UserRoleExtended, HashSet<UserRoleExtended>> hierarchy = new();

        private static void Includes(UserRoleExtended userRole, params UserRoleExtended[] otherRoles)
        {
            hierarchy.Add(userRole, otherRoles.ToHashSet());
        }

        public static HashSet<UserRoleExtended> GetIncludedRoles(this UserRoleExtended userRole)
        {
            return new(hierarchy[userRole]);
        }

        static UserRoleHierarchyExtension() //TODO: need a test that verifies this setup is not self looping.
        {
            Includes(UserRole.DEFAULT); //UserRole.DEFAULT does not inherit anything
            Includes(UserRole.HR, UserRole.DEFAULT);
            Includes(UserRole.ORG_ADMIN, UserRole.HR);
            Includes(UserRole.SYS_ADMIN);//UserRole.SYS_ADMIN does not inherit anthing
        }
    }
}
