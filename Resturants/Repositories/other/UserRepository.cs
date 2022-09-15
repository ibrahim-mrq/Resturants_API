using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Resturants.DTO.Requests;
using Resturants.DTO.Responses;
using Resturants.Helper;
using Resturants.Models;
using Resturants.Repositories.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Resturants.Repositories.other
{
    public class UserRepository : IUserRepository
    {
        private readonly DBContext _dbContext;
        private readonly IHostingEnvironment _environment;
        private readonly IMapper _map;

        public UserRepository(DBContext dBContext, IMapper map, IHostingEnvironment environment)
        {
            this._dbContext = dBContext;
            this._map = map;
            this._environment = environment;
        }

        public OperationType GetAllUsers()
        {
            var users = _dbContext.Users.Where(x => x.Type.Equals(Constants.TYPE_USER));
            var vendors = _dbContext.Users.Where(x => x.Type.Equals(Constants.TYPE_VENDOR));
            var result = new OperationType()
            {
                Status = true,
                Code = 200,
                Message = "success",
                Data = new { users = users.ToList(), vendors = vendors }
            };

            return result;
        }

        public OperationType GetUsers()
        {
            var list = _dbContext.Users.Where(x => x.Type.Equals(Constants.TYPE_USER) && x.IsDelete == false);
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
            var list = _dbContext.Users.Where(x => x.Type.Equals(Constants.TYPE_VENDOR) && x.IsDelete == false);
            var result = new OperationType()
            {
                Status = true,
                Code = 200,
                Message = "success",
                Data = new { users = list.ToList() }
            };
            return result;
        }

        public OperationType Login(LoginRequest userLogin)
        {
            if (!Constants.IsNullOrEmpty(userLogin).Status)
            {
                return Constants.IsNullOrEmpty(userLogin);
            }

            var user = _dbContext.Users.Where(x => x.Phone.Equals(userLogin.Phone)).SingleOrDefault();
            if (user == null)
            {
                return new OperationType() { Status = false, Message = "Phone Number not exists!", Code = 400 };
            }
            if (!Constants.ValidateHash(userLogin.Password, user.PasswordHash, user.PasswordSalt))
            {
                return new OperationType() { Status = false, Message = "incorrect Password!", Code = 400 };
            }
            user.Token = GenerateToken(user);

            var currentUser = new object();
            if (user.Type.Equals(Constants.TYPE_USER)) currentUser = _map.Map<UserResponse>(user);
            else currentUser = _map.Map<VendorResponse>(user);
            var response = new OperationType()
            {
                Status = true,
                Message = "Login successfully",
                Code = 200,
                Data = new { user = currentUser }
            };
            return response;
        }

        public OperationType UserRegistration(UserRequest userRequest)
        {
            var filePath = "https://localhost:7194/Images/monkey-d-luffy-489x1024.png";
            userRequest.Type = Constants.TYPE_USER;
            if (!Constants.IsNullOrEmpty(userRequest).Status)
            {
                return Constants.IsNullOrEmpty(userRequest);
            }
            if (userRequest.Phone.Length != 10)
            {
                return new OperationType() { Status = false, Message = "Please enter a valid phone number!", Code = 400 };
            }
            if (_dbContext.Users.Any(x => x.Phone.Equals(userRequest.Phone)))
            {
                return new OperationType() { Status = false, Message = "Phone Number already exists!", Code = 400 };
            }
            if (userRequest.Password.Length < 6)
            {
                return new OperationType() { Status = false, Message = "the password must be at least 6 characters long!", Code = 400 };
            }
            if (userRequest.Photo != null)
            {
                try
                {
                    string fName = userRequest.Photo.FileName;
                    string path = Path.Combine(_environment.ContentRootPath, "Images", fName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        userRequest.Photo.CopyToAsync(stream);
                    }
                    filePath = "https://localhost:7194/Images/" + fName;
                }
                catch (Exception) { }
            }
            var currentUser = _map.Map<User>(userRequest);
            byte[] hash, salt;
            Constants.GenerateHash(userRequest.Password, out hash, out salt);
            currentUser.PasswordHash = hash;
            currentUser.PasswordSalt = salt;
            currentUser.Photo = filePath;
            currentUser.Token = GenerateToken(currentUser);
            _dbContext.Users.Add(currentUser);
            _dbContext.SaveChanges();
            var response = new OperationType()
            {
                Status = true,
                Message = "Account created successfully",
                Code = 200,
                Data = new { user = _map.Map<UserResponse>(currentUser) }
            };
            return response;
        }

        public OperationType VendorRegistration(VendorRequest vendorRequest)
        {
            var filePath = "https://localhost:7194/Images/monkey-d-luffy-489x1024.png";
            vendorRequest.Type = Constants.TYPE_VENDOR;
            if (!Constants.IsNullOrEmpty(vendorRequest).Status)
            {
                return Constants.IsNullOrEmpty(vendorRequest);
            }
            if (vendorRequest.Phone.Length != 10)
            {
                return new OperationType() { Status = false, Message = "Please enter a valid phone number!", Code = 400 };
            }
            if (_dbContext.Users.Any(x => x.Phone.Equals(vendorRequest.Phone)))
            {
                return new OperationType() { Status = false, Message = "Phone Number already exists!", Code = 400 };
            }
            if (vendorRequest.Password.Length < 6)
            {
                return new OperationType() { Status = false, Message = "the password must be at least 6 characters long!", Code = 400 };
            }
            if (vendorRequest.Photo != null)
            {
                try
                {
                    string fName = vendorRequest.Photo.FileName;
                    string path = Path.Combine(_environment.ContentRootPath, "Images", fName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        vendorRequest.Photo.CopyToAsync(stream);
                    }
                    filePath = "https://localhost:7194/Images/" + fName;
                }
                catch (Exception) { }
            }

            var currentUser = _map.Map<User>(vendorRequest);
            byte[] hash, salt;
            Constants.GenerateHash(vendorRequest.Password, out hash, out salt);
            currentUser.PasswordHash = hash;
            currentUser.PasswordSalt = salt;
            currentUser.Photo = filePath;
            currentUser.Token = GenerateToken(currentUser);
            _dbContext.Users.Add(currentUser);
            _dbContext.SaveChanges();
            var response = new OperationType()
            {
                Status = true,
                Message = "Account created successfully",
                Code = 200,
                Data = new { user = _map.Map<VendorResponse>(currentUser) }
            };
            return response;
        }

        public OperationType UpdateVendor(int id, VendorRequest vendorRequest)
        {
            var user = _dbContext.Users.Where(x => x.Id.Equals(id) && x.IsDelete == false).SingleOrDefault();
            if (user == null)
            {
                return new OperationType() { Status = false, Message = "User Id not exists!", Code = 400 };
            }
            var currentUser = _map.Map(vendorRequest, user);
            _dbContext.SaveChanges();
            var response = new OperationType()
            {
                Status = true,
                Message = "Load User Profile successfully",
                Code = 200,
                Data = new { user = currentUser }
            };
            return response;
        }

        public OperationType UpdateUser(int Id, UserUpdateRequest userUpdate)
        {
            var user = _dbContext.Users.Where(x => x.Id == Id && x.IsDelete== false).SingleOrDefault();
            if (user == null)
            {
                return new OperationType() { Status = false, Message = "User Id not exists!", Code = 400 };
            }
            var filePath = user.Photo;
            if (userUpdate.Photo != null)
            {
                try
                {
                    string fName = userUpdate.Photo.FileName;
                    string path = Path.Combine(_environment.ContentRootPath, "Images", fName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        userUpdate.Photo.CopyToAsync(stream);
                    }
                    filePath = "https://localhost:7194/Images/" + fName;
                }
                catch (Exception) { }
            }
            var currentUser = _map.Map(userUpdate, user);
            _dbContext.SaveChanges();
            var response = new OperationType()
            {
                Status = true,
                Message = "Update User successfully",
                Code = 200,
                Data = new { user = currentUser }
            };
            return response;
        }

        public OperationType LoadProfile(int id, string token)
        {
            var user = _dbContext.Users.Where(x => x.Id.Equals(id) && x.IsDelete == false).SingleOrDefault();
            if (user == null)
            {
                return new OperationType() { Status = false, Message = "User Id not exists!", Code = 400 };
            }
            if (string.IsNullOrEmpty(token) || user.Token != token)
            {
                return new OperationType() { Status = false, Message = "Unauthorized! \n" + token, Code = 401 };
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
            var user = _dbContext.Users.Where(x => x.Id.Equals(id) && x.IsDelete == false).SingleOrDefault();
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
            _dbContext.Users.Update(user);
            _dbContext.SaveChanges();
            return response;
        }

        public OperationType ClearAllUser()
        {
            var list = _dbContext.Users.ToList();
            foreach (var item in _dbContext.Users)
            {
                _dbContext.Users.Remove(item);
            }
            _dbContext.SaveChanges();
            return new OperationType() { Status = true, Message = "All users have been successfully deleted!", Code = 200, Data = new { users = list } };
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
