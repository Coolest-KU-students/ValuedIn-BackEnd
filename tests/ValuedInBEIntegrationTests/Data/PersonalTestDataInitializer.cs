using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValuedInBE.PersonalValues.Models.Entities;
using ValuedInBE.System.PersistenceLayer.Contexts;

namespace ValuedInBEIntegrationTests.Data
{
    public static class PersonalValueTestDataInitializer
    {
        private const string createdBy = "testData";
        public static async Task Initialize(ValuedInContext valuedInContext)
        {
            List<PersonalValueGroup> groups = CreateTestingValueGroups();
            valuedInContext.ValueGroups.AddRange(groups);

            await valuedInContext.SaveChangesAsync();
        }
        
        private static List<PersonalValueGroup> CreateTestingValueGroups() 
        {
            long groupId = 0;
            long valueId = 0;
            return new List<PersonalValueGroup>()
            {
                new PersonalValueGroup() { Id = ++groupId, Name = "Loyalty",
                    PersonalValues = new()
                    {
                        new PersonalValue(){Id = ++valueId, GroupId = groupId, Name = "Loyal", Modifier = 5, CreatedBy = createdBy},
                        new PersonalValue(){Id = ++valueId, GroupId = groupId, Name = "Trusting", Modifier = 1, CreatedBy = createdBy},
                        new PersonalValue(){Id = ++valueId, GroupId = groupId, Name = "Honorable", Modifier = 2, CreatedBy = createdBy}
                    }, CreatedBy = createdBy },
                new PersonalValueGroup() { Id = ++groupId, Name = "Work-Life Balance",
                    PersonalValues = new()
                    {
                        new PersonalValue(){Id = ++valueId, GroupId = groupId, Name = "Workaholic", Modifier = -5, CreatedBy = createdBy},
                        new PersonalValue(){Id = ++valueId, GroupId = groupId, Name = "Family Oriented", Modifier = 5, CreatedBy = createdBy}
                    }, CreatedBy = createdBy },
                new PersonalValueGroup() { Id = ++groupId, Name = "Communication",
                    PersonalValues = new()
                    {
                        new PersonalValue(){Id = ++valueId, GroupId = groupId, Name = "Extrovert", Modifier = 2, CreatedBy = createdBy},
                        new PersonalValue(){Id = ++valueId, GroupId = groupId, Name = "Honest", Modifier = 3, CreatedBy = createdBy}
                    }, CreatedBy = createdBy }
            };
        }
    }
}
