using Resturants.Helper;

namespace Resturants.Middelwares
{
    public class AuthMiddelware
    {
        private readonly RequestDelegate _next;

        public AuthMiddelware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task InvokeAsync(HttpContext context, DBContext _dBContext)
        {

            var Token = context.Request.Headers["token"].FirstOrDefault()?.Split(" ").Last();

           /* var result = _dBContext.tokens.Where(x =>
                   x.IsActive == true
                   && x.Token == Token
                   && x.Expires == DateTime.Now
                   )
                .FirstOrDefault().ToString();*/

            if (Token != null)
            {
            /*    if (result == Token) 
                {
                
                }*/
                context.Response.StatusCode = 401;
                await context.Response.WriteAsJsonAsync(new
                {
                    status = false,
                    message = "You are festic",
                    code = 401
                });
                return;
            }
            else
            {
                await _next(context);
            }

            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new
            {
                status = false,
                message = Token,
                code = 401
            });
            return;


            /*    // return un aut
                if (Token  == null)
                    {
                        await _next(context);
                    }
                    else
                    {
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsJsonAsync(new
                        {
                            status = false,
                            message = Token,
                            code = 401
                        });
                        return;

                    }*/

        }

        /* public async Task Invoke(HttpContext context, DBContext dataContext)
         {
             var token = context.Response.Headers.ContainsKey("token").ToString();
             //     var token = context.Request.Headers["token"].FirstOrDefault()?.Split(" ").Last();



             if (token != null)
             {
                 context.Response.StatusCode = 401;
                 await context.Response.WriteAsJsonAsync(new
                 {
                     status = false,
                     message = "Unauthorized",
                     code = 401
                 });
                 return;
             }
             else
             {
                 await _next(context);
             }
         }*/
    }
}
