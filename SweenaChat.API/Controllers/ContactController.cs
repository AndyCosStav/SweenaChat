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

            var messagesListUser = await _dbContext.Contact
                .Include(m => m.Messages)
                .Where(x =>
                                     (x.Owner == username) && x.Name == contactName).ToListAsync();

            var messagesListContact = await _dbContext.Contact
                .Include(m => m.Messages)
                .Where(x =>
                                     (x.Owner == contactName) && x.Name == username).ToListAsync();

            foreach (var message in messagesListContact)
            {
                foreach (var m in message.Messages)
                {
                    MessagesList.Messages.Add(m);
                }
            }

            foreach (var message in messagesListUser)
            {
                foreach (var m in message.Messages)
                {
                    MessagesList.Messages.Add(m);
                }
            }

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
