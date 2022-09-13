using System.ComponentModel.DataAnnotations;

namespace Resturants.DTO.Responses
{
    public class UserResponse
    {
        public UserResponse()
        {
            Name = "";
            Email = "";
            Phone = "";
            Address = "";
            Photo = "";
            TotalPayment = 0;
            Token = "";
        }
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; } = "";
        public string? Phone { get; set; } = "";
        public string Address { get; set; } = "";
        public string Photo { get; set; } = "";
        public float TotalPayment { get; set; } = 0;
        public string? Token { get; set; } = "";
    }
}
