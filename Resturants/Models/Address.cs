using System.ComponentModel.DataAnnotations;

namespace Resturants.Models
{
    public class Address
    {
        public Address()
        {
            Title = "";
            Phone = "";
        }

        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }
        public int UserId { get; set; }
    }
}
