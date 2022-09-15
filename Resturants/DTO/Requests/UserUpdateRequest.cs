

namespace Resturants.DTO.Requests
{
    public class UserUpdateRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public IFormFile Photo { get; set; }

    }
}
