using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Resturants.DTO.Requests;
using Resturants.DTO.Responses;
using Resturants.Helper;
using Resturants.Models;
using Resturants.Repositories.Interfaces;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Resturants.Repositories.other
{
    public class UserRepository : IUserRepository
    {
        private readonly DBContext _dBContext;
        private readonly IMapper _map;

        public UserRepository(DBContext dBContext, IMapper map)
        {
            this._dBContext = dBContext;
            this._map = map;
        }

        public OperationType GetAllUsers()
        {
            var list = _dBContext.Users;
            var result = new OperationType()
            {
                Status = true,
                Code = 200,
                Message = "success",
                Data = new { users = list.ToList() }
            };

            return result;
        }

        public OperationType GetUsers()
        {
            var list = _dBContext.Users.Where(x => x.Type.Equals(Constants.TYPE_USER));
            var result = new OperationType()
            {
                Status = true,
                Code = 200,
                Message = "success",
                Data = new { users = list.ToList() }
            };
            return result;
        }

        public OperationType GetVendors()
        {
            var list = _dBContext.Users.Where(x => x.Type.Equals(Constants.TYPE_VENDOR));
            var result = new OperationType()
            {
                Status = true,
                Code = 200,
                Message = "success",
                Data = new { users = list.ToList() }
            };
            return result;
        }

        public OperationType Login(UserLogin userLogin)
        {
            if (!Constants.IsNullOrEmpty(userLogin).Status)
            {
                return Constants.IsNullOrEmpty(userLogin);
            }

            var user = _dBContext.Users.Where(x => x.Phone.Equals(userLogin.Phone)).SingleOrDefault();
            if (user == null)
            {
                return new OperationType() { Status = false, Message = "Phone Number not exists!", Code = 400 };
            }
            if (!Constants.ValidateHash(userLogin.Password, user.PasswordHash, user.PasswordSalt))
            {
                return new OperationType() { Status = false, Message = "incorrect Password!", Code = 400 };
            }
            user.Token = GenerateToken(user);
            var currentUser = _map.Map<UserResponse>(user);
            var response = new OperationType()
            {
                Status = true,
                Message = "Login successfully",
                Code = 200,
                Data = new { user = _map.Map<VendorResponse>(currentUser) }
            };
            return response;
        }

        public OperationType UserRegistration(UserRequest userRequest)
        {
            userRequest.Type = Constants.TYPE_USER;
            if (!Constants.IsNullOrEmpty(userRequest).Status)
            {
                return Constants.IsNullOrEmpty(userRequest);
            }
            if (userRequest.Phone.Length != 10)
            {
                return new OperationType() { Status = false, Message = "Please enter a valid phone number!", Code = 400 };
            }
            if (string.IsNullOrEmpty(userRequest.Photo))
            {
                userRequest.Photo = Constants.TYPE_LOGO;
            }

            if (_dBContext.Users.Any(x => x.Phone.Equals(userRequest.Phone)))
            {
                return new OperationType() { Status = false, Message = "Phone Number already exists!", Code = 400 };
            }
            if (userRequest.Password.Length < 6)
            {
                return new OperationType() { Status = false, Message = "the password must be at least 6 characters long!", Code = 400 };
            }

            var currentUser = _map.Map<User>(userRequest);
            byte[] hash, salt;
            Constants.GenerateHash(userRequest.Password, out hash, out salt);
            currentUser.PasswordHash = hash;
            currentUser.PasswordSalt = salt;
            currentUser.Token = GenerateToken(currentUser);
            _dBContext.Users.Add(currentUser);
            _dBContext.SaveChanges();
            var response = new OperationType()
            {
                Status = true,
                Message = "Account created successfully",
                Code = 200,
                Data = new { user = _map.Map<VendorResponse>(currentUser) }
            };
            return response;
        }

        public OperationType VendorRegistration(VendorRequest vendorRequest)
        {
            vendorRequest.Type = Constants.TYPE_VENDOR;
            if (!Constants.IsNullOrEmpty(vendorRequest).Status)
            {
                return Constants.IsNullOrEmpty(vendorRequest);
            }
            if (vendorRequest.Phone.Length != 10)
            {
                return new OperationType() { Status = false, Message = "Please enter a valid phone number!", Code = 400 };
            }
            if (string.IsNullOrEmpty(vendorRequest.Photo))
            {
                vendorRequest.Photo = Constants.TYPE_LOGO;
            }

            if (_dBContext.Users.Any(x => x.Phone.Equals(vendorRequest.Phone)))
            {
                return new OperationType() { Status = false, Message = "Phone Number already exists!", Code = 400 };
            }
            if (vendorRequest.Password.Length < 6)
            {
                return new OperationType() { Status = false, Message = "the password must be at least 6 characters long!", Code = 400 };
            }

            var currentUser = _map.Map<User>(vendorRequest);
            byte[] hash, salt;
            Constants.GenerateHash(vendorRequest.Password, out hash, out salt);
            currentUser.PasswordHash = hash;
            currentUser.PasswordSalt = salt;
            currentUser.Token = GenerateToken(currentUser);
            _dBContext.Users.Add(currentUser);
            _dBContext.SaveChanges();
            var response = new OperationType()
            {
                Status = true,
                Message = "Account created successfully",
                Code = 200,
                Data = new { user = _map.Map<VendorResponse>(currentUser) }
            };
            return response;
        }

        public OperationType UpdateUser(int id, VendorRequest vendorRequest)
        {
            var user = _dBContext.Users.Where(x => x.Id.Equals(id) && x.IsDelete == false).SingleOrDefault();
            if (user == null)
            {
                return new OperationType() { Status = false, Message = "User Id not exists!", Code = 400 };
            }
            var currentUser = _map.Map(vendorRequest, user);
            _dBContext.SaveChanges();
            var response = new OperationType()
            {
                Status = true,
                Message = "Load User Profile successfully",
                Code = 200,
                Data = new { user = currentUser }
            };
            return response;
        }

        public OperationType LoadProfile(int id)
        {
            var user = _dBContext.Users.Where(x => x.Id.Equals(id) && x.IsDelete == false).SingleOrDefault();
            if (user == null)
            {
                return new OperationType() { Status = false, Message = "User Id not exists!", Code = 400 };
            }
            var currentUser = new object();
            if (user.Type.Equals(Constants.TYPE_USER)) currentUser = _map.Map<UserResponse>(user);
            else currentUser = _map.Map<VendorResponse>(user);
            var response = new OperationType()
            {
                Status = true,
                Message = "Load User Profile successfully",
                Code = 200,
                Data = new { user = currentUser }
            };
            return response;
        }

        public OperationType DeleteUser(int id)
        {
            var user = _dBContext.Users.Where(x => x.Id.Equals(id) && x.IsDelete == false).SingleOrDefault();
            if (user == null)
            {
                return new OperationType() { Status = false, Message = "User Id not exists!", Code = 400 };
            }
            user.IsDelete = true;
            var currentUser = new object();
            if (user.Type.Equals(Constants.TYPE_USER)) currentUser = _map.Map<UserResponse>(user);
            else currentUser = _map.Map<VendorResponse>(user);
            var response = new OperationType()
            {
                Status = true,
                Message = "User Deleted successfully",
                Code = 200,
                Data = new { user = currentUser }
            };
            _dBContext.Users.Update(user);
            _dBContext.SaveChanges();
            return response;
        }

        public OperationType ClearAllUser()
        {
            var list = _dBContext.Users.ToList();
            foreach (var item in _dBContext.Users)
            {
                _dBContext.Users.Remove(item);
            }
            _dBContext.SaveChanges();
            return new OperationType() { Status = true, Message = "All users have been successfully deleted!", Code = 200, Data = new { users = list } }; ;
        }

        private string GenerateToken(User currentUser)
        {
            var keyByte = Encoding.ASCII.GetBytes("LZImjD2eUbUxhxjIdyOJuYT4FjWhKSJy");
            var descriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddMonths(12),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyByte), SecurityAlgorithms.HmacSha512Signature),
                Subject = new ClaimsIdentity(
                    new Claim[] {
                        new Claim(JwtRegisteredClaimNames.Sub , currentUser.Name),
                        new Claim(JwtRegisteredClaimNames.Email , currentUser.Email),
                        new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()),
                        new Claim("userId" , currentUser.Id.ToString()),
                    }
                )
            };
            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(descriptor);
            return handler.WriteToken(token);
        }


    }
}
