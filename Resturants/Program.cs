using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Resturants.Helper;
using Resturants.Repositories.Interfaces;
using Resturants.Repositories.other;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


AddDataBase(builder);
AddAutoMapper(builder);
AddScoped(builder);
JwtBearer(builder);

builder.Services.Configure<ApiBehaviorOptions>(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthentication();
//app.UseAuthorization();
//app.UseMiddleware<AuthMiddelware>();

app.UseStaticFiles();
app.MapControllers();

void JwtBearer(WebApplicationBuilder builder)
{
    var keyByte = Encoding.ASCII.GetBytes("LZImjD2eUbUxhxjIdyOJuYT4FjWhKSJy");
    builder.Services.AddAuthentication(op => op.DefaultScheme = JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(op =>
        {
            op.SaveToken = true;
            op.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(keyByte),
                ValidateIssuerSigningKey = true,
                ValidateAudience = false,
                ValidateIssuer = false,
            };
        });
}

void AddScoped(WebApplicationBuilder builder)
{
    builder.Services.AddScoped<IUserRepository, UserRepository>();
}

void AddAutoMapper(WebApplicationBuilder builder)
{
    builder.Services.AddAutoMapper(x => x.AddProfile<AutoMapperProfile>());
}

void AddDataBase(WebApplicationBuilder builder)
{
    builder.Services.AddDbContext<DBContext>(
        opt => opt.UseSqlServer(
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Resturants;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
            ));
}


app.Run();