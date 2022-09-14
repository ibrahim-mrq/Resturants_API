using System.ComponentModel.DataAnnotations;

namespace Resturants.DTO.Requests
{
    public class UserRequest
    {

        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public string Photo { get; set; }
        //public IFormFile Photo { get; set; }
        public string Type { get; set; }
    }
}
