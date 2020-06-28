using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SweenaChat.API.Data;
using SweenaChat.API.Models;

namespace SweenaChat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public ContactController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }




        //if user / contact relationship already exists, throw error explaining so, DO NOT DUPLICATE RELATIONSHIP
        [Route("AddContact")]
        [HttpPost]
        public async Task<Contact> AddContact(string username, string contactName)
        {
            var user = _dbContext.Users.Include(m => m.Contacts).SingleOrDefault(x => x.Username == username);

            var contact = new Contact
            {
                Name = contactName
            };

            user.Contacts.Add(contact);

            await _dbContext.SaveChangesAsync();

            return contact;
        }
    }
}
