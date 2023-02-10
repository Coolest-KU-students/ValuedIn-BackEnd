using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ValuedInBE.Contexts;
using ValuedInBE.DataControls;
using ValuedInBE.DataControls.Paging;
using ValuedInBE.Models.DTOs.Responses;
using ValuedInBE.Models.Users;
using ValuedInBE.Services.Users;

namespace ValuedInBE.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserCredentialsController : ControllerBase
    {
        private readonly ValuedInContext _context;
        private readonly IUserService _credentialService;

        public UserCredentialsController(ValuedInContext context, IUserService userCredentialsService)
        {
            _context = context;
            _credentialService = userCredentialsService;
        }

        // GET: api/UserCredentials
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserSystemInfo>>> GetUserCredentials()
        {
            return await _credentialService.GetAllUsers();
        }

        // GET: api/UserCredentials/page
        [HttpGet("/page")]
        public async Task<ActionResult<Page<UserSystemInfo>>> GetUserPage()
        {
            return await _credentialService.GetUserPage(new PageConfig(0, 5, new List<OrderByColumn>()));
        }



        // GET: api/UserCredentials/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserCredentials>> GetUserCredentials(string id)
        {
            var userCredentials = await _context.UserCredentials.FindAsync(id);

            if (userCredentials == null)
            {
                return NotFound();
            }

            return userCredentials;
        }

        // PUT: api/UserCredentials/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserCredentials(string id, UserCredentials userCredentials)
        {
            if (id != userCredentials.Login)
            {
                return BadRequest();
            }

            _context.Entry(userCredentials).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserCredentialsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/UserCredentials
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserCredentials>> PostUserCredentials(UserCredentials userCredentials)
        {
            _context.UserCredentials.Add(userCredentials);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserCredentialsExists(userCredentials.Login))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUserCredentials", new { id = userCredentials.Login }, userCredentials);
        }

        // DELETE: api/UserCredentials/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserCredentials(string id)
        {
            var userCredentials = await _context.UserCredentials.FindAsync(id);
            if (userCredentials == null)
            {
                return NotFound();
            }

            _context.UserCredentials.Remove(userCredentials);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserCredentialsExists(string id)
        {
            return _context.UserCredentials.Any(e => e.Login == id);
        }
    }
}
