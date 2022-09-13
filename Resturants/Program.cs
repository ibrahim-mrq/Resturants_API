using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Resturants.Helper;
using Resturants.Middelwares;
using Resturants.Repositories.Interfaces;
using Resturants.Repositories.other;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<DBContext>(
    opt => opt.UseSqlServer(
        @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Resturants;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
        ));


AddAutoMapper(builder);
AddScoped(builder);
JwtBearer(builder);

builder.Services.Configure<ApiBehaviorOptions>(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthentication();
//app.UseAuthorization();
//app.UseMiddleware<AuthMiddelware>();

app.MapControllers();

app.Run();

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

void AddScoped(WebApplicationBuilder builder) {
    builder.Services.AddScoped<IUserRepository, UserRepository>();
}

void AddAutoMapper(WebApplicationBuilder builder) {
    builder.Services.AddAutoMapper(x => x.AddProfile<AutoMapperProfile>());
}
