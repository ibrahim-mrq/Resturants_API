using Microsoft.AspNetCore.Mvc;
using Resturants.DTO.Requests;
using Resturants.Repositories.Interfaces;

namespace Resturants.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var respone = _userRepository.GetAllUsers();
            return Ok(respone);
        }

        [HttpGet("GetUsers")]
        public IActionResult GetUsers()
        {
            var respone = _userRepository.GetUsers();
            return Ok(respone);
        }

        [HttpGet("GetVendors")]
        public IActionResult GetVendors()
        {
            var respone = _userRepository.GetVendors();
            return Ok(respone);
        }

        [HttpPost("UserRegister")]
        public IActionResult UserRegister([FromForm] UserRequest userRequest)
        {
            var respone = _userRepository.UserRegistration(userRequest);
            if (!respone.Status)
            {
                return BadRequest(respone);
            }
            return Ok(respone);
        }

        [HttpPost("VendorRegister")]
        public IActionResult VendorRegister(VendorRequest vendorRequest)
        {
            var respone = _userRepository.VendorRegistration(vendorRequest);
            if (!respone.Status)
            {
                return BadRequest(respone);
            }
            return Ok(respone);
        }

        [HttpPost("Login")]
        public IActionResult Login([FromForm] UserLogin userLogin)
        {
            var respone = _userRepository.Login(userLogin);
            if (!respone.Status)
            {
                return BadRequest(respone);
            }
            return Ok(respone);
        }

        [HttpGet("LoadProfile/{Id}")]
        public IActionResult LoadProfile(int Id)
        {
            var respone = _userRepository.LoadProfile(Id);
            return Ok(respone);
        }

        [HttpDelete("DeleteUser/{Id}")]
        public IActionResult DeleteUser(int Id)
        {
            var respone = _userRepository.DeleteUser(Id);
            return Ok(respone);
        }

        [HttpDelete("Delete")]
        public IActionResult Delete()
        {
            var respone = _userRepository.ClearAllUser();
            return Ok(respone);
        }


    }
}
