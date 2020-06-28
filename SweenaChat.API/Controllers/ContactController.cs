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


        [Route("GetContactMessages")]
        [HttpGet]
        public async Task<ContactMessageViewModel> GetContactMessages(string username, string contactName)
        {
            ContactMessageViewModel MessagesList = new ContactMessageViewModel();

            var messages = await _dbContext.Contact
                .Where(x => (x.Owner == username && x.Name == contactName) || (x.Owner == contactName && x.Name == username))
                .SelectMany(m => m.Messages)
                .ToListAsync();
             MessagesList.Messages.AddRange(messages);

            return MessagesList;

        }


        [Route("AddContact")]
        [HttpPost]
        public async Task<Contact> AddContact(string username, string contactName)
        {
            var user = _dbContext.Users.Include(m => m.Contacts).SingleOrDefault(x => x.Username == username);

            var contact = new Contact
            {
                Name = contactName,
                Owner = username
            };

            user.Contacts.Add(contact);

            await _dbContext.SaveChangesAsync();

            return contact;
        }
    }
}
