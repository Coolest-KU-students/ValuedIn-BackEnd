using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            IExecutionStrategy execution = valuedInContext.Database.CreateExecutionStrategy();
            execution.ExecuteInTransaction(() => {
                List<PersonalValueGroup> groups = CreateTestingValueGroups();
                // turn on IDENTITY_INSERT for the table

                valuedInContext.ValueGroups.AddRange(groups);
                valuedInContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.ValueGroups ON;");

                valuedInContext.SaveChanges();
                valuedInContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.ValueGroups OFF");
            }, () => { return true; });
            execution.ExecuteInTransaction(() => {
                List<PersonalValue> values = CreateTestingPersonalValue();
                valuedInContext.Values.AddRange(values);
                valuedInContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.[Values] ON;");
                valuedInContext.SaveChanges();
                valuedInContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.[Values] OFF");
            }, () => { return true; });
        }
        
        private static List<PersonalValueGroup> CreateTestingValueGroups() 
        {
            long groupId = 0;
            return new List<PersonalValueGroup>()
            {
                new PersonalValueGroup() { Id = ++groupId, Name = "Loyalty", CreatedBy = createdBy },
                new PersonalValueGroup() { Id = ++groupId, Name = "Work-Life Balance", CreatedBy = createdBy },
                new PersonalValueGroup() { Id = ++groupId, Name = "Communication", CreatedBy = createdBy }
            };
        }
        
        private static List<PersonalValue> CreateTestingPersonalValue()
        {
            long valueId = 0;
            long groupId = 0;
            return new()
            {
                new PersonalValue(){Id = ++valueId, GroupId = ++groupId, Name = "Loyal", Modifier = 5, CreatedBy = createdBy},
                new PersonalValue(){Id = ++valueId, GroupId = groupId, Name = "Trusting", Modifier = 1, CreatedBy = createdBy},
                new PersonalValue(){Id = ++valueId, GroupId = groupId, Name = "Honorable", Modifier = 2, CreatedBy = createdBy},
                new PersonalValue(){Id = ++valueId, GroupId = ++groupId, Name = "Workaholic", Modifier = -5, CreatedBy = createdBy},
                new PersonalValue(){Id = ++valueId, GroupId = groupId, Name = "Family Oriented", Modifier = 5, CreatedBy = createdBy},
                new PersonalValue(){Id = ++valueId, GroupId = ++groupId, Name = "Extrovert", Modifier = 2, CreatedBy = createdBy},
                new PersonalValue(){Id = ++valueId, GroupId = groupId, Name = "Honest", Modifier = 3, CreatedBy = createdBy}
            };
        }
    }
}
