using System.ComponentModel.DataAnnotations;

namespace Resturants.DTO.Requests
{
    public class MenuRequest
    {

        public string Name { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public IFormFile Photo { get; set; }

    }
}
