using System.ComponentModel.DataAnnotations;

namespace Resturants.Models
{
    public class Menu
    {

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public int UserId { get; set; }
    }
}
