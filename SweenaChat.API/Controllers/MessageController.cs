using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SweenaChat.API.Data;
using SweenaChat.API.Models;
using SweenaChat.API.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SweenaChat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public MessageController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
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

            if (user != null)
            {
                user.Messages.Add(newMessage);


                await _dbContext.SaveChangesAsync();

                return Ok(new { MessageContent = newMessage.MessageContent });
            }

            return NotFound();


        }


    }
}