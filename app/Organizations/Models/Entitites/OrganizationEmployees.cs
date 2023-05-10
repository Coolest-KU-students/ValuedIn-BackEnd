using System.ComponentModel.DataAnnotations;
using ValuedInBE.System.PersistenceLayer.Entities;
using ValuedInBE.Users.Models;
using ValuedInBE.Users.Models.Entities;

namespace ValuedInBE.Organizations.Models.Entitites
{
    public class OrganizationEmployees : AuditCreatedUpdateBase
    {
        [Key]
        public long OrganizationId { get; set; }
        [Key]
        public long UserId { get; set; }
        public long EmployeeId => UserId; //alias
        public DateTime WorkingSince { get; set; }

        public Organization? Organization { get; set; }
        public UserDetails? UserDetails { get; set; }
    }
}
