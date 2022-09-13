using System.ComponentModel.DataAnnotations;

namespace Resturants.Models
{
    public class Tokens
    {
        [Key]
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public int UserId { get; set; }
        public bool IsActive { get; set; }

    }
}
