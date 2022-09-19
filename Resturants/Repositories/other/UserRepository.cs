using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Resturants.DTO.Requests;
using Resturants.DTO.Responses;
using Resturants.Helper;
using Resturants.Models;
using Resturants.Repositories.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Xml.Linq;
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
            var users = _dbContext.Users.Where(x => x.Type.Equals(Constants.TYPE_USER)).ToList();
            var vendors = _dbContext.Users.Where(x => x.Type.Equals(Constants.TYPE_VENDOR)).ToList();
            var result = new OperationType()
            {
                Status = true,
                Code = 200,
                Message = "success",
                Data = new { users = users, vendors = vendors }
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
            _dbContext.Users.Update(user);
            _dbContext.SaveChanges();

            var response = new OperationType();
            response.Status = true;
            response.Message = "Load User Profile successfully";
            response.Code = 200;

            if (user.Type.Equals(Constants.TYPE_USER))
            {
                var currentUser = _map.Map<UserResponse>(user);
                response.Data = currentUser;
            }
            else
            {
                var currentVendor = _map.Map<VendorResponse>(user);
                InitVenderData(currentVendor, user.Id);
                response.Data = new { user = currentVendor };
            }
            return response;

        }

        public OperationType LoadProfile(int Id, string token)
        {
            var user = _dbContext.Users.Where(x => x.Id.Equals(Id) && x.IsDelete == false).SingleOrDefault();
            if (user == null)
            {
                return new OperationType() { Status = false, Message = "User Id not exists!", Code = 400 };
            }
            if (string.IsNullOrEmpty(token) || user.Token != token)
            {
                return new OperationType() { Status = false, Message = "Unauthorized!", Code = 401 };
            }
            var response = new OperationType();
            response.Status = true;
            response.Message = "Load User Profile successfully";
            response.Code = 200;

            if (user.Type.Equals(Constants.TYPE_USER))
            {
                var currentUser = _map.Map<UserResponse>(user);
                response.Data = new { user = currentUser };
            }
            else
            {
                var currentVendor = _map.Map<VendorResponse>(user);
                InitVenderData(currentVendor, Id);
                response.Data = new { user = currentVendor };
            }
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




        public OperationType UpdateVendor(int Id, string Token, VendorUpdateRequest vendorUpdate)
        {
            var user = _dbContext.Users.Where(x => x.Id.Equals(Id) && x.IsDelete == false && x.Type == Constants.TYPE_VENDOR).SingleOrDefault();
            if (user == null)
            {
                return new OperationType() { Status = false, Message = "User Id not exists!", Code = 400 };
            }
            if (string.IsNullOrEmpty(Token) || user.Token != Token)
            {
                return new OperationType() { Status = false, Message = "Unauthorized!", Code = 401 };
            }

            if (string.IsNullOrEmpty(vendorUpdate.Name))
            {
                vendorUpdate.Name = user.Name;
            }
            if (string.IsNullOrEmpty(vendorUpdate.Email))
            {
                vendorUpdate.Email = user.Email;
            }
            if (string.IsNullOrEmpty(vendorUpdate.Address))
            {
                vendorUpdate.Address = user.Address;
            }
            if (string.IsNullOrEmpty(vendorUpdate.Description))
            {
                vendorUpdate.Description = user.Description;
            }
            if (string.IsNullOrEmpty(vendorUpdate.WorkDays))
            {
                vendorUpdate.WorkDays = user.WorkDays;
            }
            if (string.IsNullOrEmpty(vendorUpdate.WorkHours))
            {
                vendorUpdate.WorkHours = user.WorkHours;
            }

            if (vendorUpdate.AddressList != null)
            {
                foreach (var item in vendorUpdate.AddressList)
                {
                    user.AddressList.Add(item);
                }
            }
            else vendorUpdate.AddressList = user.AddressList;

            if (vendorUpdate.PhotoList != null)
            {
                foreach (var item in vendorUpdate.PhotoList)
                {
                    user.PhotoList.Add(item);
                }
            }
            else vendorUpdate.PhotoList = user.PhotoList;

            if (vendorUpdate.ProductList != null)
            {
                foreach (var item in vendorUpdate.ProductList)
                {
                    user.ProductList.Add(item);
                }
            }
            else vendorUpdate.ProductList = user.ProductList;

            var filePath = "";
            if (string.IsNullOrEmpty(user.Photo))
            {
                filePath = "https://localhost:7194/Images/monkey-d-luffy-489x1024.png";
            }
            else
            {
                filePath = user.Photo;
            }
            if (vendorUpdate.Photo != null)
            {
                try
                {
                    string fName = vendorUpdate.Photo.FileName;
                    string path = Path.Combine(_environment.ContentRootPath, "Images", fName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        vendorUpdate.Photo.CopyToAsync(stream);
                    }
                    filePath = "https://localhost:7194/Images/" + fName;
                }
                catch (Exception) { }
            }

            var updateUser = _map.Map(vendorUpdate, user);
            user.Photo = filePath;
            _dbContext.SaveChanges();
            var vendorResponse = _map.Map<VendorResponse>(user);
            InitVenderData(vendorResponse, Id);
            var response = new OperationType()
            {
                Status = true,
                Message = "Update User successfully",
                Code = 200,
                Data = new { user = _map.Map<VendorResponse>(user) }
            };
            return response;
        }

        public OperationType UpdateUser(int Id, string Token, UserUpdateRequest userUpdate)
        {
            var user = _dbContext.Users.Where(x => x.Id.Equals(Id) && x.IsDelete == false && x.Type == Constants.TYPE_USER).SingleOrDefault();
            if (user == null)
            {
                return new OperationType() { Status = false, Message = "User Id not exists!", Code = 400 };
            }
            if (string.IsNullOrEmpty(Token) || user.Token != Token)
            {
                return new OperationType() { Status = false, Message = "Unauthorized!", Code = 401 };
            }

            if (string.IsNullOrEmpty(userUpdate.Name))
            {
                userUpdate.Name = user.Name;
            }
            if (string.IsNullOrEmpty(userUpdate.Email))
            {
                userUpdate.Email = user.Email;
            }
            if (string.IsNullOrEmpty(userUpdate.Address))
            {
                userUpdate.Address = user.Address;
            }


            var filePath = "";
            if (string.IsNullOrEmpty(user.Photo))
            {
                filePath = "https://localhost:7194/Images/monkey-d-luffy-489x1024.png";
            }
            else
            {
                filePath = user.Photo;
            }
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

            var updateUser = _map.Map(userUpdate, user);
            user.Photo = filePath;
            _dbContext.SaveChanges();
            var response = new OperationType()
            {
                Status = true,
                Message = "Update User successfully",
                Code = 200,
                Data = new { user = _map.Map<UserResponse>(user) }
            };
            return response;
        }




        public OperationType AddAddress(int UserId, string Token, List<AddressRequest> addressRequest)
        {
            var user = _dbContext.Users.Where(x => x.Id == UserId && x.IsDelete == false && x.Type == Constants.TYPE_VENDOR).SingleOrDefault();
            if (user == null)
            {
                return new OperationType() { Status = false, Message = "User Id not exists!", Code = 400, };
            }
            if (string.IsNullOrEmpty(Token) || user.Token != Token)
            {
                return new OperationType() { Status = false, Message = "Unauthorized!", Code = 401 };
            }

            foreach (var item in addressRequest)
            {
                var currentAddress = _map.Map<Address>(item);
                currentAddress.UserId = UserId;
                _dbContext.Address.Add(currentAddress);
                _dbContext.SaveChanges();
            }
            var response = new OperationType()
            {
                Status = true,
                Code = 200,
                Message = "Address Added successfully",
                Data = new { addresses = addressRequest }
            };
            return response;
        }

        public OperationType AddProduct(int UserId, string Token, List<ProductRequest> productRequests)
        {
            var user = _dbContext.Users.Where(x => x.Id == UserId && x.IsDelete == false && x.Type == Constants.TYPE_VENDOR).SingleOrDefault();
            if (user == null)
            {
                return new OperationType() { Status = false, Message = "User Id not exists!", Code = 400, };
            }
            if (string.IsNullOrEmpty(Token) || user.Token != Token)
            {
                return new OperationType() { Status = false, Message = "Unauthorized!", Code = 401 };
            }

            foreach (var item in productRequests)
            {
                var currentMenu = _map.Map<Product>(item);
                if (item.Photo != null)
                {
                    string fName = item.Photo.FileName;
                    string path = Path.Combine(_environment.ContentRootPath, "Images", fName);
                    var stream = new FileStream(path, FileMode.Create);
                    item.Photo.CopyToAsync(stream);
                    currentMenu.Photo = Constants.TYPE_LOCAL_URL + fName;
                }
                else
                {
                    currentMenu.Photo = Constants.TYPE_LOGO;
                }
                currentMenu.UserId = UserId;

                _dbContext.Products.Add(currentMenu);
                _dbContext.SaveChanges();
            }
            var response = new OperationType()
            {
                Status = true,
                Code = 200,
                Message = "Products Added successfully",
                Data = new { products = _dbContext.Products.Where(x => x.UserId == UserId).ToList() }
            };
            return response;
        }

        public OperationType AddPhoto(int UserId, string Token, List<PhotoRequest> photoRequest)
        {
            var user = _dbContext.Users.Where(x => x.Id == UserId && x.IsDelete == false && x.Type == Constants.TYPE_VENDOR).SingleOrDefault();
            if (user == null)
            {
                return new OperationType() { Status = false, Message = "User Id not exists!", Code = 400, };
            }
            if (string.IsNullOrEmpty(Token) || user.Token != Token)
            {
                return new OperationType() { Status = false, Message = "Unauthorized!", Code = 401 };
            }

            if (photoRequest != null)
            {
                foreach (var item in photoRequest)
                {
                    var currentPhoto = _map.Map<Photo>(item);
                    if (item.Photo != null)
                    {
                        string fName = item.Photo.FileName;
                        string path = Path.Combine(_environment.ContentRootPath, "Images", fName);
                        using var stream = new FileStream(path, FileMode.Create);
                        item.Photo.CopyToAsync(stream);
                        currentPhoto.Path = Constants.TYPE_LOCAL_URL + fName;
                    }
                    else currentPhoto.Path = Constants.TYPE_LOGO;

                    currentPhoto.UserId = UserId;
                    _dbContext.Photos.Add(currentPhoto);
                    _dbContext.SaveChanges();

                }
            }
            else
            {
                return new OperationType() { Status = false, Message = "file must not empty or null!", Code = 400 };
            }

            var response = new OperationType()
            {
                Status = true,
                Code = 200,
                Message = "Photos Photos successfully",
            };
            return response;
        }



        public OperationType RemoveAddress(int UserId, int AddressId, string Token)
        {
            var user = _dbContext.Users.Where(x => x.Id.Equals(UserId) && x.IsDelete == false && x.Type == Constants.TYPE_VENDOR).SingleOrDefault();
            if (user == null)
            {
                return new OperationType() { Status = false, Message = "User Id not exists!", Code = 400 };
            }
            if (string.IsNullOrEmpty(Token) || user.Token != Token)
            {
                return new OperationType() { Status = false, Message = "Unauthorized!", Code = 401 };
            }
            var address = _dbContext.Address.Where(x => x.Id.Equals(AddressId) && x.UserId == UserId).SingleOrDefault();
            if (address == null)
            {
                return new OperationType() { Status = false, Message = "Address Id not exists!", Code = 400 };
            }
            _dbContext.Address.Remove(address);
            _dbContext.SaveChanges();
            var response = new OperationType()
            {
                Status = true,
                Code = 200,
                Message = "Address Deleted successfully",
            };
            return response;
        }

        public OperationType RemovePhoto(int UserId, int PhotoId, string Token)
        {
            var user = _dbContext.Users.Where(x => x.Id.Equals(UserId) && x.IsDelete == false && x.Type == Constants.TYPE_VENDOR).SingleOrDefault();
            if (user == null)
            {
                return new OperationType() { Status = false, Message = "User Id not exists!", Code = 400 };
            }
            if (string.IsNullOrEmpty(Token) || user.Token != Token)
            {
                return new OperationType() { Status = false, Message = "Unauthorized!", Code = 401 };
            }
            var photo = _dbContext.Photos.Where(x => x.Id.Equals(PhotoId) && x.UserId == UserId).SingleOrDefault();
            if (photo == null)
            {
                return new OperationType() { Status = false, Message = "Photo Id not exists!", Code = 400 };
            }
            _dbContext.Photos.Remove(photo);
            _dbContext.SaveChanges();
            var response = new OperationType()
            {
                Status = true,
                Code = 200,
                Message = "Photo Deleted successfully",
            };
            return response;
        }

        public OperationType RemoveMenu(int UserId, int MenuId, string Token)
        {
            var user = _dbContext.Users.Where(x => x.Id.Equals(UserId) && x.IsDelete == false && x.Type == Constants.TYPE_VENDOR).SingleOrDefault();
            if (user == null)
            {
                return new OperationType() { Status = false, Message = "User Id not exists!", Code = 400 };
            }
            if (string.IsNullOrEmpty(Token) || user.Token != Token)
            {
                return new OperationType() { Status = false, Message = "Unauthorized!", Code = 401 };
            }
            var menu = _dbContext.Products.Where(x => x.Id.Equals(MenuId) && x.UserId == UserId).SingleOrDefault();
            if (menu == null)
            {
                return new OperationType() { Status = false, Message = "Menu Id not exists!", Code = 400 };
            }
            _dbContext.Products.Remove(menu);
            _dbContext.SaveChanges();
            var response = new OperationType()
            {
                Status = true,
                Code = 200,
                Message = "Menu Deleted successfully",
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
            var users = _dbContext.Users.ToList();
            var addresses = _dbContext.Address.ToList();
            var products = _dbContext.Products.ToList();
            var photos = _dbContext.Photos.ToList();
            var tokens = _dbContext.Tokens.ToList();

            _dbContext.Tokens.RemoveRange(tokens);
            _dbContext.Photos.RemoveRange(photos);
            _dbContext.Products.RemoveRange(products);
            _dbContext.Address.RemoveRange(addresses);
            _dbContext.Users.RemoveRange(users);

            _dbContext.SaveChanges();

            return new OperationType()
            {
                Status = true,
                Message = "All users have been successfully deleted!",
                Code = 200,
                Data = new { users = users }
            };
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

        private void InitVenderData(VendorResponse vendorResponse, int Id)
        {
            var addresse = _dbContext.Address.Where(x => x.UserId == Id).ToList();
            var products = _dbContext.Products.Where(x => x.UserId == Id).ToList();
            var photo = _dbContext.Photos.Where(x => x.UserId == Id).ToList();
            vendorResponse.AddressList = addresse;
            vendorResponse.ProductList = products;
            vendorResponse.PhotoList = photo;
        }

    }
}
