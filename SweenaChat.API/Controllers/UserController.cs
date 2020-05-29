using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SweenaChat.API.Data;
using SweenaChat.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SweenaChat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public UserController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext; 
        }


        // GET: api/User
        [Route("getusers")]
        [HttpGet]
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _dbContext.Users
                .ToListAsync();
        }

        [Route("getuserbyid")]
        [HttpGet]
        public async Task <User> GetUserById([FromQuery] int id)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(x => x.UserId == id);

        }

        [Route("GetUserMessagesById")]
        [HttpGet]
        public async Task <User> GetUserMessagesByIds([FromQuery] int id)
        {
            return await _dbContext.Users
                .Include(m => m.Messages)
                .SingleOrDefaultAsync(x => x.UserId == id);
            
        }



    }
}
