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
    public class CartRepository : ICartRepository
    {
        private readonly DBContext _dbContext;
        private readonly IMapper _map;

        public CartRepository(DBContext dBContext, IMapper map)
        {
            this._dbContext = dBContext;
            this._map = map;
        }




        public OperationType GetCart(int UserId, string Token)
        {
            var user = _dbContext.Users.Where(x => x.Id.Equals(UserId) && x.IsDelete == false && x.Type == Constants.TYPE_USER).SingleOrDefault();
            if (user == null)
            {
                return new OperationType() { Status = false, Message = "User Id not exists!", Code = 400 };
            }
            if (string.IsNullOrEmpty(Token) || user.Token != Token)
            {
                return new OperationType() { Status = false, Message = "Unauthorized!", Code = 401 };
            }
            var list = _dbContext.Carts.Where(x => x.UserId == UserId).ToList();
            var totlePrice = 0.0;
            foreach (var item in list)
            {
                totlePrice += item.Quantity * item.Price;
                // totlePrice = totlePrice + item.Price * list.Count();
            }

            var result = new OperationType()
            {
                Status = true,
                Code = 200,
                Message = "success",
                Data = new
                {
                    carts = list,
                    totlePrice = totlePrice,
                    totleProduct = list.Count(),

                }
            };
            return result;
        }

        public OperationType AddToCart(int UserId, string Token, CartRequest CartRequest)
        {
            var user = _dbContext.Users.Where(x => x.Id.Equals(UserId) && x.IsDelete == false && x.Type == Constants.TYPE_USER).SingleOrDefault();
            if (user == null)
            {
                return new OperationType() { Status = false, Message = "User Id not exists!", Code = 400 };
            }
            if (string.IsNullOrEmpty(Token) || user.Token != Token)
            {
                return new OperationType() { Status = false, Message = "Unauthorized!", Code = 401 };
            }
            var product = _dbContext.Products.Where(x => x.Id.Equals(CartRequest.ProductId)).SingleOrDefault();
            if (product == null)
            {
                return new OperationType() { Status = false, Message = "Product Id not exists!", Code = 400 };
            }

            var result = new OperationType()
            {
                Status = true,
                Code = 200,

            };

            var cart = _dbContext.Carts.Where(x => x.ProductId.Equals(CartRequest.ProductId)).SingleOrDefault();
            if (cart != null)
            {
                cart.Quantity = cart.Quantity + CartRequest.Quantity;
                //   CartRequest.Quantity = cart.Quantity + CartRequest.Quantity;
                var currentCart = _map.Map<Cart>(cart);
                _dbContext.Carts.Update(currentCart);
                _dbContext.SaveChanges();
                result.Message = "update cart success";
                result.Data = new { carts = _map.Map<CartResponse>(currentCart) };
                return result;
            }
            var carts = _map.Map<Cart>(CartRequest);
            carts.ProductDescription = product.Description;
            carts.ProductPhoto = product.Photo;
            carts.ProductName = product.Name;
            carts.Price = product.Price;
            carts.UserId = UserId;
            _dbContext.Carts.Add(carts);
            _dbContext.SaveChanges();
            result.Message = "add to cart success";
            result.Data = new { carts = _map.Map<CartResponse>(carts) };
            return result;
        }
    }
}
