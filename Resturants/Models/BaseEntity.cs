using Resturants.Helper;

namespace Resturants.Models
{
    public class BaseEntity
    {
        public string CreatedBy { get; set; } = "";
        public string CreatedAt { get; set; } = "";
        public string UpdatedBy { get; set; } = "";
        public string UpdatedAt { get; set; } = "";

        public string Type { get; set; } = "";

        public Boolean IsDelete { get; set; }
        public Boolean IsActive { get; set; }
        public Boolean IsCompelte { get; set; }

        public BaseEntity()
        {
            CreatedBy = Constants.TYPE_USER;
            CreatedAt = DateTime.Now.ToString("dd-MMM-yyyy HH:mm tt");
            IsActive = true;
            IsCompelte = true;
            IsDelete = false;
        }

    }
}
