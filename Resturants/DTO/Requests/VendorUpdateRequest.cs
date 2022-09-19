using Resturants.Models;

namespace Resturants.DTO.Requests
{
    public class VendorUpdateRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public IFormFile Photo { get; set; }
        public string Description { get; set; }
        public string WorkDays { get; set; }
        public string WorkHours { get; set; }
        public List<Address> AddressList { get; set; }
        public List<Photo> PhotoList { get; set; }
        public List<Product> ProductList { get; set; }

    }
}
