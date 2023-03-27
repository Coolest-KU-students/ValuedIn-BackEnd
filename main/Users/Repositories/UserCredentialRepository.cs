using Microsoft.EntityFrameworkCore;
using ValuedInBE.DataControls.Ordering;
using ValuedInBE.System.DataControls.Ordering;
using ValuedInBE.System.DataControls.Paging;
using ValuedInBE.System.PersistenceLayer.Contexts;
using ValuedInBE.System.PersistenceLayer.Extensions;
using ValuedInBE.Users.Models;
using ValuedInBE.Users.Models.DTOs.Request;
using ValuedInBE.Users.Models.Entities;

namespace ValuedInBE.Users.Repositories
{

    public class UserCredentialRepository : IUserCredentialRepository
    {
        private readonly CustomColumnMapping<UserCredentials> _customColumnMapping = new()
        {
            {"LastName",  new OrderingExpression<UserCredentials>(creds => creds.UserDetails!.LastName + creds.UserDetails.FirstName) }
        };

        private readonly ValuedInContext _context;
        private readonly ILogger<UserCredentialRepository> _logger;

        public UserCredentialRepository(ValuedInContext context, ILogger<UserCredentialRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<int> CountWithSameNamesAsync(string firstName, string lastName)
        {
            _logger.LogTrace("Requested user count with First Name '{firstName}', Last Name '{lastName}'", firstName, lastName);
            return await _context.UserDetails.CountAsync(a => a.FirstName == firstName && a.LastName == lastName);
        }

        public async Task<IEnumerable<UserCredentials>> GetAllUsersAsync()
        {
            _logger.LogTrace("All User List Requested");
            var credentialQuery = from c in _context.UserCredentials
                                            .Include(a => a.UserDetails)
                                  select c;
            await credentialQuery.LoadAsync();
            return credentialQuery;
        }

        public async Task<UserCredentials?> GetByLoginAsync(string login)
        {
            _logger.LogTrace("Request user credentials with login {login}", login);
            var credentialQuery = from c in _context.UserCredentials
                                  where c.Login == login
                                  select c;
            return await credentialQuery.FirstOrDefaultAsync();
        }
        public async Task<UserCredentials?> GetByUserIDAsync(string userID)
        {
            _logger.LogTrace("Request user credentials with userID {userID}", userID);
            var credentialQuery = from c in _context.UserCredentials
                                  where c.UserID == userID
                                  select c;
            return await credentialQuery.FirstOrDefaultAsync();
        }

        public async Task<UserCredentials?> GetByLoginWithDetailsAsync(string login)
        {
            _logger.LogTrace("Request user credentials and details with login {login}", login);
            var credentialQuery = from c in _context.UserCredentials
                                            .Include(creds => creds.UserDetails)

                                  where c.Login == login
                                  select c;
            return await credentialQuery.FirstOrDefaultAsync();
        }

        public async Task<UserCredentials?> GetByUserIdWithDetailsAsync(string userID)
        {
            _logger.LogTrace("Request user credentials and details with user ID {userId}", userID);
            var credentialQuery = from c in _context.UserCredentials
                                            .Include(creds => creds.UserDetails)

                                  where c.UserID == userID
                                  select c;
            return await credentialQuery.FirstOrDefaultAsync();
        }


        public async Task<Page<UserCredentials>> GetUserPageWithDetailsAsync(UserPageRequest config)
        {
            _logger.LogTrace("Request Requested {size} User Page {page}", config.Size, config.Page);

            var credentialQuery = from c in _context.UserCredentials
                                            .Include(a => a.UserDetails)
                                            .ApplyOrderingInLinq(config.OrderByColumns, _customColumnMapping)
                                            .Where(a => a.IsExpired == config.ShowExpired)
                                  select c;

            await credentialQuery.LoadAsync();
            int total = credentialQuery.Count();
            credentialQuery.Skip(config.Page * config.Size);
            credentialQuery.Take(config.Size);

            return new Page<UserCredentials> { 
                Results = credentialQuery, 
                Total = total, 
                PageNo = config.Page + 1 };
        }

        public async Task InsertAsync(UserCredentials userCredentials, UserContext createdBy)
        {
            _logger.LogTrace("Inserting user with login {login}", userCredentials.Login);
            _context.UserCredentials.Add(userCredentials);
            _context.ChangeTracker.CheckAuditing(createdBy);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> LoginExistsAsync(string login)
        {
            _logger.LogTrace("Inserting user with login {login}", login);
            return await _context.UserCredentials.AnyAsync(c => c.Login == login);
        }

        public async Task UpdateAsync(UserCredentials userCredentials, UserContext updatedBy)
        {
            _logger.LogTrace("Updating user with login {login}", userCredentials.Login);
            _context.UserCredentials.Update(userCredentials);
            _context.ChangeTracker.CheckAuditing(updatedBy);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> VerifyUserIdsAsync(IEnumerable<string> userIds)
        {
            int count = await _context.UserCredentials.CountAsync(c => userIds.Contains(c.UserID));
            return count == userIds.Count();
        }
    }
}
