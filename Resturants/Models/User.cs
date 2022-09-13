using System.ComponentModel.DataAnnotations;

namespace Resturants.Models
{
    public class User : BaseEntity
    {
        public User()
        {
            Name = "";
            Email = "";
            Phone = "";
            Address = "";
            Photo = "";
            Description = "";
            WorkDays = "";
            WorkHours = "";
            AddressList = new List<Address>();
            PhotoList = new List<Photo>();
            MenuList = new List<Menu>();

        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; } 
        public string Email { get; set; } 
        public string Phone { get; set; } 
        public string Address { get; set; } 
        public string Photo { get; set; }
        public float TotalPayment { get; set; } 
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Token { get; set; }


        public string Description { get; set; }
        public string WorkDays { get; set; } 
        public string WorkHours { get; set; } 


        public List<Address> AddressList { get; set; }
        public List<Photo> PhotoList { get; set; } 
        public List<Menu> MenuList { get; set; } 


    }
}

