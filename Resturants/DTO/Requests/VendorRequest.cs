using Newtonsoft.Json.Linq;
using Resturants.Models;
using System.ComponentModel.DataAnnotations;

namespace Resturants.DTO.Requests
{
    public class VendorRequest
    {
        public string Name { get; set; }
        public string Email { get; set; } 
        public string Phone { get; set; } 
        public string Address { get; set; } 
        public string Password { get; set; }
        public IFormFile Photo { get; set; }
        public string Type { get; set; }
        public string Description { get; set; } 
        public string WorkDays { get; set; } 
        public string WorkHours { get; set; }
        public List<Address> AddressList { get; set; }
        public List<Photo> PhotoList { get; set; }
        public List<Product> ProductList { get; set; }


    }
}
