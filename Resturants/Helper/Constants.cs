using System.Text;

namespace Resturants.Helper
{
    public class Constants
    {
        public static readonly string TYPE_USER = "User";
        public static readonly string TYPE_VENDOR = "Vendor";
        public static readonly string TYPE_LOGO = "https://www.wepal.net/ar/uploads/2732018-073911PM-1.jpg";
        public static readonly string TYPE_LOCAL_URL = "https://localhost:7194/Images/";

        public static readonly string STATUS_CREATED = "Created";
        public static readonly string STATUS_APPROVED = "Approved";
        public static readonly string STATUS_WORKING = "Working";
        public static readonly string STATUS_DELIVERT = "Delivert";
        public static readonly string STATUS_COMPLETE = "Complete";

        public static readonly int STATUS_CREATED_CODE = 1;
        public static readonly int STATUS_APPROVED_CODE = 2;
        public static readonly int STATUS_WORKING_CODE = 3;
        public static readonly int STATUS_DELIVERT_CODE = 4;
        public static readonly int STATUS_COMPLETE_CODE = 5;

        public static string Status(int status)
        {
            return status switch
            {
                1 => STATUS_CREATED,
                2 => STATUS_APPROVED,
                3 => STATUS_WORKING,
                4 => STATUS_DELIVERT,
                5 => STATUS_COMPLETE,
                _ => "error",
            };
        }


        public static void GenerateHash(String password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hash = new System.Security.Cryptography.HMACSHA512())
            {
                passwordHash = hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                passwordSalt = hash.Key;
            }
        }

        public static Boolean ValidateHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hash = new System.Security.Cryptography.HMACSHA512(passwordSalt);
            var newPassHash = hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < newPassHash.Length; i++)
            {
                if (newPassHash[i] != passwordHash[i]) return false;
            }
            return true;
        }

        public static OperationType IsNullOrEmpty(object model)
        {
            OperationType operationType = new OperationType() { Status = true, Message = "", Code = 200 };
            foreach (var item in model.GetType().GetProperties())
            {
                if (item.PropertyType == typeof(string))
                {
                    string? propVal = item.GetValue(model, null) as string;
                    if (string.IsNullOrEmpty(propVal))
                    {
                        operationType = new OperationType() { Status = false, Message = item.Name + " must not be blank!", Code = 400 };
                    }
                }
            }
            return operationType;
        }


    }
}
