using Resturants.Models;

namespace Resturants.DTO.Responses
{
    public class VendorResponse
    {
        public VendorResponse()
        {
            Name = "";
            Email = "";
            Phone = "";
            Address = "";
            Photo = "";
            Token = "";
            Description = "";
            WorkDays = "";
            WorkHours = "";
            AddressList = new List<Address>();
            PhotoList = new List<Photo>();
            MenuList = new List<Menu>();
        }

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
        public List<Menu> MenuList { get; set; }
    }
}
