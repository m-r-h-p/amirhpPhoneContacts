using Microsoft.AspNetCore.Mvc;
using PhoneContacts.Business.Messages;
using PhoneContacts.Business.Services.UserService;
using PhoneContacts.Business.StatusCode;
using PhoneContacts.Data.DTOs;

namespace PhoneContacts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserServices _userServices;
        public AccountController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ReturnDTO>> Register(
            [FromBody] RegisterDTO register
            )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(register);
            }
            ReturnDTO returnDTO = new ReturnDTO();

            returnDTO = await _userServices.CreateUser(register);
            returnDTO = Hnadler.SetStatusCode(returnDTO);

            if (returnDTO.meta.message == SystemOperationMessages.OperationStatus.Null.ToString())
            {
                return BadRequest(returnDTO);
            }
            else if (returnDTO.meta.message == SystemOperationMessages.OperationStatus.EmailIsExist.ToString())
            {
                return BadRequest(returnDTO);
            }
            else if (returnDTO.meta.message == SystemOperationMessages.OperationStatus.PhoneNuberIsExist.ToString())
            {
                return BadRequest(returnDTO);
            }
            else if (returnDTO.meta.message == SystemOperationMessages.OperationStatus.DataBaseError.ToString())
            {
                return BadRequest(returnDTO);
            }
            else if (returnDTO.meta.message == SystemOperationMessages.OperationStatus.SmsSendingFaild.ToString())
            {
                return BadRequest(returnDTO);
            }


            return NoContent();
        }

        [HttpPost("login")]
        public async Task<ActionResult<ReturnDTO>> Login([FromBody] LoginDTO login)
        {
            if (String.IsNullOrEmpty(login.email) || String.IsNullOrEmpty(login.password))
            {
                return BadRequest();
            }

            ReturnDTO result = await _userServices.LoginUser(login.email, login.password);

            result = Hnadler.SetStatusCode(result);
           
            return Ok(result);

        }

        [HttpPost("UserVerify/{emailAddress}/{activitionCode}")]
        public async Task<ActionResult<ReturnDTO>> UserVerify(string emailAddress, int activitionCode)
        {
            if (!String.IsNullOrEmpty(emailAddress) && activitionCode < 0)
            {
                return BadRequest();
            }
            ReturnDTO result = await _userServices.UserActivition(emailAddress, activitionCode);
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
