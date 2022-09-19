using Resturants.Models;

namespace Resturants.DTO.Responses
{
    public class VendorResponse
    {


        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Photo { get; set; }
        public string Token { get; set; }
        public string Description { get; set; }
        public string WorkDays { get; set; }
        public string WorkHours { get; set; }
        public List<Address> AddressList { get; set; }
        public List<Photo> PhotoList { get; set; }
        public List<Product> ProductList { get; set; }
    }
}
