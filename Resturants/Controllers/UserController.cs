using Microsoft.AspNetCore.Mvc;
using Resturants.DTO.Requests;
using Resturants.Models;
using Resturants.Repositories.Interfaces;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Resturants.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IHostingEnvironment _environment;

        public UserController(IUserRepository userRepository, IHostingEnvironment environment)
        {
            this._userRepository = userRepository;
            this._environment = environment;
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



        [HttpPost("Login")]
        public IActionResult Login([FromForm] LoginRequest userLogin)
        {
            var respone = _userRepository.Login(userLogin);
            if (!respone.Status)
            {
                return BadRequest(respone);
            }
            return Ok(respone);
        }

        [HttpGet("LoadProfile/{Id}")]
        public IActionResult LoadProfile(int Id, [FromHeader] string token)
        {
            var respone = _userRepository.LoadProfile(Id, token);
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
        public IActionResult VendorRegister([FromBody] VendorRequest vendorRequest)
        {
            var respone = _userRepository.VendorRegistration(vendorRequest);
            if (!respone.Status)
            {
                return BadRequest(respone);
            }
            return Ok(respone);
        }



        [HttpPut("UpdateUser/{Id}")]
        public IActionResult UpdateUser(int Id, [FromHeader] string Token, [FromForm] UserUpdateRequest userUpdate)
        {
            var respone = _userRepository.UpdateUser(Id, Token, userUpdate);
            if (!respone.Status)
            {
                return BadRequest(respone);
            }
            return Ok(respone);
        }

        [HttpPut("UpdateVendor/{Id}")]
        public IActionResult UpdateVendor(int Id, [FromHeader] string Token, [FromForm] VendorUpdateRequest vendorUpdate)
        {
            var respone = _userRepository.UpdateVendor(Id, Token, vendorUpdate);
            if (!respone.Status)
            {
                return BadRequest(respone);
            }
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


        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
        {
            if (file != null)
            {
                try
                {
                    string fName = file.FileName;
                    string path = Path.Combine(_environment.ContentRootPath, "Images", fName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    return Ok(new { status = true, url = "https://localhost:7194/Images/" + fName, coed = 200 });
                }
                catch (Exception exception)
                {
                    return BadRequest(new { status = false, message = "error: " + exception.Message, coed = 400 });
                }
            }
            else
            {
                return BadRequest(new { status = false, message = "file must not empty or null!", coed = 400 });
            }
        }

    }
}
