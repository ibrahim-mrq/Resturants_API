using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Resturants.Helper
{
    public class Constants
    {
        public static string TYPE_USER = "User";
        public static string TYPE_VENDOR = "Vendor";
        public static string TYPE_LOGO = "https://www.wepal.net/ar/uploads/2732018-073911PM-1.jpg";


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
            using (var hash = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var newPassHash = hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < newPassHash.Length; i++)
                {
                    if (newPassHash[i] != passwordHash[i]) return false;
                }
                return true;
            }
        }

        public static OperationType IsNullOrEmpty(object model)
        {
            OperationType operationType = new OperationType() { Status = true, Message = "", Code = 200 }; ; ;
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
