using System.ComponentModel.DataAnnotations;

namespace Resturants.Models
{
    public class Photo
    {

        [Key]
        public int Id { get; set; }
        public string Path { get; set; } 
        public int VendorId { get; set; }
    }
}
