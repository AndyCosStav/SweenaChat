using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SweenaChat.API.Data;
using SweenaChat.API.Hubs;
using SweenaChat.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SweenaChat.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private IHubContext<ChatHub> _chat;

        public MessageController(ApplicationDbContext dbContext, IHubContext<ChatHub> chat)
        {
            _dbContext = dbContext;
            _chat = chat;
        }

        [Route("GetAllMessages")]
        [HttpGet]
        public async Task<List<Message>> GetAllMessages(string user)
        {
            var messages = _dbContext.Messages
                .Where(x => x.Receiver == user || x.Sender == user).ToListAsync();

            return await messages;
        }


        [Route("GetConversation")]
        [HttpGet]
        public ActionResult GetConverstion([FromQuery] string user1, string user2 )
        {
            var conversations = _dbContext.Messages.Where(x =>
                                     (x.Receiver == user1 || x.Receiver == user2) &&
                                     (x.Sender == user2 || x.Sender == user1)).ToList();

            return Ok(new { conversations });
        }


        [Route("sendMessage")]
        [HttpPost]
        public async Task<ActionResult> SendMessage([FromBody] Message message)
        {

            var newMessage = new Message
            {
                MessageContent = message.MessageContent,
                DateCreated = DateTime.Now,
                Sender = message.Sender,
                Receiver = message.Receiver

            };

            _dbContext.Messages.Add(newMessage);

            var user = _dbContext.Users.Include(u => u.Messages).SingleOrDefault(x => x.Username == message.Sender);

            var contact = _dbContext.Contact.Include(u => u.Messages).SingleOrDefault(x => x.Name == message.Receiver);

            if (user != null)
            {
                if(contact != null)
                {
                    contact.Messages.Add(newMessage);
                    user.Messages.Add(newMessage);


                    await _dbContext.SaveChangesAsync();

                    return Ok(new { MessageContent = newMessage.MessageContent });
                }

                return NotFound("message was not sent ");
                

            }

            return NotFound();


        }


    }
}