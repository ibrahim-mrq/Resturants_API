using AutoMapper;
using Resturants.DTO.Requests;
using Resturants.Helper;
using Resturants.Models;
using Resturants.Repositories.Interfaces;

namespace Resturants.Repositories.other
{
    public class CartRepository : ICartRepository
    {
        private readonly DBContext _dbContext;

        public CartRepository(DBContext dBContext)
        {
            this._dbContext = dBContext;
        }


        public OperationType GetCart(string Token, int UserId)
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
                var cartProducts = _dbContext.CartProducts.Where(x => x.CartId == item.Id).ToList();
                item.CartProducts = cartProducts;
                item.TotleProduct = cartProducts.Count;

                foreach (var product in item.CartProducts)
                {
                    totlePrice += product.Quantity * product.Price;
                    item.TotlePrice += product.Quantity * product.Price;
                }

            }
            totlePrice = Math.Round(totlePrice, 2);


            var result = new OperationType()
            {
                Status = true,
                Code = 200,
                Message = "success",
                Data = new
                {
                    totleProduct = list.Count,
                    totlePrice,
                    carts = list,
                }
            };
            return result;
        }

        public OperationType AddToCart(string Token, int UserId, int cartId, CartRequest CartRequest)
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
            var newProduct = new CartProduct()
            {
                ParentId = product.Id,
                CartId = cartId,
                Quantity = CartRequest.Quantity,
                Price = product.Price,
                ProductName = product.Name,
                ProductDescription = product.Description,
                ProductPhoto = product.Photo,
            };
            var cart = _dbContext.Carts.Where(x => x.UserId == UserId && x.Id == cartId).SingleOrDefault();
            if (cart != null)
            {
                _dbContext.CartProducts.Add(newProduct);
                cart.CustomerName ??= CartRequest.CustomerName.Trim();
                _dbContext.Carts.Update(cart);
                _dbContext.SaveChanges();
                return new OperationType()
                {
                    Status = true,
                    Message = "update cart product !",
                    Code = 200,
                    Data = new { cart }
                };
            }
            var newCart = new Cart()
            {
                Id = newProduct.Id,
                CartProducts = new List<CartProduct>() { newProduct },
                CustomerName = CartRequest.CustomerName == null ? "non" : CartRequest.CustomerName,
                TotleProduct = 0,
                TotlePrice = 0,
                UserId = UserId,
            };

            _dbContext.Carts.Add(newCart);
            _dbContext.SaveChanges();

            var result = new OperationType()
            {
                Status = true,
                Code = 200,
                Message = "add to cart success",
                Data = new { cart = newCart }
            };
            return result;
        }

        public OperationType RemoveProductFromCart(string Token, int UserId, int CartId, int ProductId)
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
            var cart = _dbContext.Carts.Where(x => x.Id == CartId).SingleOrDefault();
            if (cart == null)
            {
                return new OperationType() { Status = false, Message = "Product Id in cart not exists!", Code = 400 };
            }
            var product = _dbContext.CartProducts.Where(x => x.CartId == CartId && x.Id == ProductId).SingleOrDefault();
            if (product == null)
            {
                return new OperationType() { Status = false, Message = "Product Id in cart not exists!", Code = 400 };
            }
            _dbContext.CartProducts.Remove(product);
            _dbContext.SaveChanges();
            var result = new OperationType()
            {
                Status = true,
                Code = 200,
                Message = "Product removed form cart success!"
            };
            return result;
        }

        /* 
           public OperationType UpdateProductInCart(string Token, int UserId, int CartId, int Quantity)
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
               var cart = _dbContext.Carts.Where(x => x.Id.Equals(CartId)).SingleOrDefault();
               if (cart == null)
               {
                   return new OperationType() { Status = false, Message = "Product Id in cart not exists!", Code = 400 };
               }
               cart.Quantity = Quantity;
               _dbContext.Carts.Update(cart);
               _dbContext.SaveChanges();
               var result = new OperationType()
               {
                   Status = true,
                   Code = 200,
                   Message = "Update Product in cart success!",
                   Data = new { carts = _map.Map<CartResponse>(cart) }
               };
               return result;
           }*/


        public OperationType ClearAllCart()
        {

            var Cart = _dbContext.Carts.ToList();
            var CartProducts = _dbContext.CartProducts.ToList();
            _dbContext.Carts.RemoveRange(Cart);
            _dbContext.CartProducts.RemoveRange(CartProducts);
            _dbContext.SaveChanges();
            return new OperationType()
            {
                Status = true,
                Message = "cart have been successfully deleted!",
                Code = 200
            };
        }

        public OperationType DeleteCartById(int CartId)
        {

            var Cart = _dbContext.Carts.Where(x => x.Id == CartId).FirstOrDefault();
            if (Cart == null)
            {
                return new OperationType()
                {
                    Status = false,
                    Message = "cart id not exiest!",
                    Code = 400
                };
            }
            var CartProducts = _dbContext.CartProducts.Where(x => x.CartId == CartId).ToList();
            _dbContext.Carts.Remove(Cart);
            _dbContext.CartProducts.RemoveRange(CartProducts);
            _dbContext.SaveChanges();
            return new OperationType()
            {
                Status = true,
                Message = $"CartId:{CartId} have been successfully deleted!",
                Code = 200
            };
        }

    }
}
