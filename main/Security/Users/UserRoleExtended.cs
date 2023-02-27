using Microsoft.OpenApi.Extensions;
using NuGet.Packaging;
using System.Collections.Immutable;

namespace ValuedInBE.Security.Users
{
    public class UserRoleExtended
    {
        public static readonly UserRoleExtended DEFAULT = new(UserRole.DEFAULT, 0);
        public static readonly UserRoleExtended HR = new(UserRole.HR, 1);
        public static readonly UserRoleExtended ORG_ADMIN = new(UserRole.ORG_ADMIN, 2);
        public static readonly UserRoleExtended SYS_ADMIN = new(UserRole.SYS_ADMIN, 3);
        public static readonly ImmutableList<UserRoleExtended> ExtendedRoles =
            ImmutableList.Create(
                DEFAULT,
                HR,
                ORG_ADMIN,
                SYS_ADMIN
            );

        public static UserRoleExtended GetExtended(UserRole userRole)
        {
            UserRoleExtended correspondingExtension =
                ExtendedRoles.Find(extended => extended.UserRole.Equals(userRole));
            if (correspondingExtension != null) return correspondingExtension;

            throw new NotImplementedException($"No extended user role defined for enum {userRole.GetDisplayName()}");
        }


        public static implicit operator string(UserRoleExtended userRole) => userRole.UserRole.GetDisplayName();
        public static implicit operator int(UserRoleExtended userRole) => userRole.Index;
        public static implicit operator UserRoleExtended(UserRole role) => GetExtended(role);
        public static explicit operator UserRole(UserRoleExtended extended) => extended.UserRole;

        private UserRole UserRole { get; init; }
        private int Index { get; init; }


        private UserRoleExtended(UserRole userRole, int index)
        {
            UserRole = userRole;
            Index = index;
        }

        public override bool Equals(object obj)
        {
            return obj is UserRoleExtended extended &&
                   UserRole == extended.UserRole &&
                   Index == extended.Index
                   || obj is UserRole role &&
                   UserRole == role
                   ;
        }

        public HashSet<UserRole> FlattenRoleHierarchy()
        {
            HashSet<UserRoleExtended> checkedRoles = new()
            {
                this
            };

            HashSet<UserRoleExtended> includedRoles = this.GetIncludedRoles();

            while (includedRoles.Except(checkedRoles).Any())
            {
                HashSet<UserRoleExtended> nextLayerRoles = new();
                foreach (UserRoleExtended extended in includedRoles.Except(checkedRoles))
                {
                    nextLayerRoles.AddRange(extended.GetIncludedRoles());
                }

                checkedRoles.AddRange(includedRoles);
                includedRoles = nextLayerRoles;
            }

            return checkedRoles.Select(extended => extended.UserRole).ToHashSet();
        }

        public override string ToString() => this;

        public override int GetHashCode()
        {
            return UserRole.GetHashCode();
        }
    }
}
