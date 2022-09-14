using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resturants.DTO.Requests;
using Resturants.Models;
using Resturants.Repositories.Interfaces;
using System.IO;
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
        public IActionResult LoadProfile(int Id, [FromHeader] string token)
        {
            var respone = _userRepository.LoadProfile(Id, token);
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
        public async Task<IActionResult> UploadFile()
        {
            try
            {
                var file = Request.Form.Files[0];
                string fName = file.FileName;
                string path = Path.Combine(_environment.ContentRootPath, "Images", file.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                //   return path;
                //  return "https://localhost:7194/" + fName;
                return Ok(new { path = path, url = "https://localhost:7194/" + fName });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
