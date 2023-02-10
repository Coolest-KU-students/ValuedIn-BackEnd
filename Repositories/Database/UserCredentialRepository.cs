using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ValuedInBE.Contexts;
using ValuedInBE.DataControls;
using ValuedInBE.DataControls.Paging;
using ValuedInBE.Models.Users;

namespace ValuedInBE.Repositories.Database
{

    public class UserCredentialRepository : IUserCredentialRepository
    {

        private readonly ValuedInContext _context;

        public UserCredentialRepository(ValuedInContext context)
        {
            _context = context;
        }

        public async Task<List<UserCredentials>> GetAllUsers()
        {
            var credentialQuery = from c in _context.UserCredentials.Include(a=>a.UserDetails) select c;
            return await credentialQuery.ToListAsync();
        }

        public  async Task<UserCredentials> GetByLogin(string login)
        {
            var credentialQuery = from c in _context.UserCredentials
                    where c.Login == login
                    select c;
            return await credentialQuery.FirstAsync();

        }

        public async Task<Page<UserCredentials>> GetUserPage(PageConfig config)
        {
            string compiledOrderBy = string.Join(", ", config.OrderByColumns.Select(order => $"{order.Column} {(order.Ascending ? "ASC" : "DESC")}"));

            var credentialQuery = from c in _context.UserCredentials.Include(a => a.UserDetails)
                    orderby compiledOrderBy
                    select c;

            int total = credentialQuery.Count();

            credentialQuery.Skip(config.Skip);
            credentialQuery.Take(config.Size);

            List<UserCredentials> credentials = await credentialQuery.ToListAsync();

            return new Page<UserCredentials>(credentials, total, config.Skip + 1);
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
