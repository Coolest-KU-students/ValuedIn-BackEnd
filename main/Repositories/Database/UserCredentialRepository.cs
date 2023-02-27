using Microsoft.EntityFrameworkCore;
using NuGet.Configuration;
using System.Linq.Expressions;
using ValuedInBE.Contexts;
using ValuedInBE.DataControls.Ordering;
using ValuedInBE.DataControls.Ordering.Internal;
using ValuedInBE.DataControls.Paging;
using ValuedInBE.Models.Users;

namespace ValuedInBE.Repositories.Database
{

    public class UserCredentialRepository : IUserCredentialRepository
    {
        private readonly CustomColumnMapping<UserCredentials> _customColumnMapping = new()
        {
            {"LastName",  new OrderingExpression<UserCredentials>(creds => creds.UserDetails.LastName + creds.UserDetails.FirstName) }
        };

        private readonly ValuedInContext _context;

        public UserCredentialRepository(ValuedInContext context)
        {
            _context = context;
        }

        public async Task<int> CountWithSameName(string firstName, string lastName)
        {
            return await _context.UserDetails.CountAsync(a => a.FirstName == firstName && a.LastName == lastName);
        }

        public async Task<List<UserCredentials>> GetAllUsers()
        {
            var credentialQuery = from c in _context.UserCredentials
                                            .Include(a => a.UserDetails) 
                                  select c;
            return await credentialQuery.ToListAsync();
        }

        public async Task<UserCredentials> GetByLogin(string login)
        {
            var credentialQuery = from c in _context.UserCredentials
                                  where c.Login == login
                                  select c;
            return await credentialQuery.FirstOrDefaultAsync();
        }
        public async Task<UserCredentials> GetByLoginWithDetails(string login)
        {
            var credentialQuery = from c in _context.UserCredentials
                                            .Include(creds => creds.UserDetails)
                                           
                                  where c.Login == login
                                  select c;
            return await credentialQuery.FirstOrDefaultAsync();
        }

        public async Task<UserCredentials> GetByUserIdWithDetails(string userID)
        {
            var credentialQuery = from c in _context.UserCredentials
                                            .Include(creds => creds.UserDetails)

                                  where c.UserID == userID
                                  select c;
            return await credentialQuery.FirstOrDefaultAsync();
        }


        public async Task<Page<UserCredentials>> GetUserPageWithDetails(PageConfig config)
        {
            var credentialQuery = from c in _context.UserCredentials
                                            .Include(a => a.UserDetails)
                                            .ApplyOrderingInLinq(config.OrderByColumns, _customColumnMapping)
                                  select c;

            int total = credentialQuery.Count();
            credentialQuery.Skip(config.Page * config.Size);
            credentialQuery.Take(config.Size);

            List<UserCredentials> credentials = await credentialQuery.ToListAsync();

            return new Page<UserCredentials>(credentials, total, config.Page + 1);
        }

        public async Task Insert(UserCredentials userCredentials)
        {
            _context.UserCredentials.Add(userCredentials);
            await _context.SaveChangesAsync();

        }

        public async Task<bool> LoginExists(string login)
        {
            return await _context.UserCredentials.AnyAsync(c => c.Login == login);
        }

        public async Task Update(UserCredentials userCredentials)
        {
            _context.UserCredentials.Update(userCredentials);
            await _context.SaveChangesAsync();

        }
    }
}
