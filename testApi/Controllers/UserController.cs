using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PhoneContacts.Business.Messages;
using PhoneContacts.Business.Services.ContactServices;
using PhoneContacts.Business.Services.UserService;
using PhoneContacts.Business.StatusCode;
using PhoneContacts.Data.DTOs;
using System.Text;

namespace PhoneContacts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;
        private readonly IContactService _contactService;
        public UserController(IUserServices userServices, IContactService contactService)
        {
            _userServices = userServices;
            _contactService = contactService;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ReturnDTO>> GetUserById(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }

            ReturnDTO result = await _userServices.GetUsersById(id);
            result = Hnadler.SetStatusCode(result);

            if (result.statusCode == 400)
            {
                return BadRequest(result);
            }
            else if (result.statusCode == 404)
            {
                return NotFound(result);
            }
            else if (result.statusCode == 500)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("getAllUser")]
        public async Task<ActionResult<ReturnDTO>> GetAllUser()
        {
            ReturnDTO result = await _userServices.GetAllUsers();
            result = Hnadler.SetStatusCode(result);

            if (result.statusCode == 400)
            {
                return BadRequest(result);
            }
            else if (result.statusCode == 404)
            {
                return NotFound(result);
            }
            else if (result.statusCode == 500)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("addNewContact")]
        public async Task<ActionResult<ReturnDTO>> AddNewContact(
            [FromBody] ContactDTO contact
            )
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            ReturnDTO result = await _contactService.CreateContactToThisUser(contact);
            result = Hnadler.SetStatusCode(result);

            if (result.statusCode == 400)
            {
                return BadRequest(result);
            }
            else if (result.statusCode == 404)
            {
                return NotFound(result);
            }
            else if (result.statusCode == 500)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        [HttpGet("ShowUserContacts/{id}")]
        public async Task<ActionResult<ReturnDTO>> ShowUserContacts(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }
            ReturnDTO result = await _contactService.GetAllContactsOfThisUser((long)id);
            result = Hnadler.SetStatusCode(result);

            if (result.statusCode == 400)
            {
                return BadRequest(result);
            }
            else if (result.statusCode == 404)
            {
                return NotFound(result);
            }
            else if (result.statusCode == 500)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("UpdateContact")]
        public async Task<ActionResult<ReturnDTO>> UpdateContact(
            [FromBody] ContactDTO contact
            )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            ReturnDTO result = await _contactService.UpdateContact(contact);
            result = Hnadler.SetStatusCode(result);

            if (result.statusCode == 400)
            {
                return BadRequest(result);
            }
            else if (result.statusCode == 404)
            {
                return NotFound(result);
            }
            else if (result.statusCode == 500)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        [HttpGet("RemoveContact/{id}")]
        public async Task<ActionResult<ReturnDTO>> RemoveContact(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }
            ReturnDTO result = await _contactService.RemoveContact((long)id);
            result = Hnadler.SetStatusCode(result);

            if (result.statusCode == 400)
            {
                return BadRequest(result);
            }
            else if (result.statusCode == 404)
            {
                return NotFound(result);
            }
            else if (result.statusCode == 500)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        



    }
}
