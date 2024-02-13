using CozyCub;
using CozyCub.Mappings;
using CozyCub.Models.Wishlist;
using CozyCub.Payments.Orders;
using CozyCub.Services.Auth;
using CozyCub.Services.CartServices;
using CozyCub.Services.Category_services;
using CozyCub.Services.ProductService;
using CozyCub.Services.UserServices;
using CozyCub.Services.WishList;
using CozyCub.Services.WishList_Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//DbContext as a service
builder.Services.AddScoped<ApplicationDbContext>();

//AutoMapper as a service
builder.Services.AddAutoMapper(typeof(AppMapper));

//Service for Dependency injection 
builder.Services.AddScoped<IAuthService, AuthService>();

//User service
builder.Services.AddScoped<IUserService, UserService>();

//Product Service 
builder.Services.AddScoped<IProductService, ProductServices>();

//Category
builder.Services.AddScoped<ICategoryService, CategoryService>();

//Cart
builder.Services.AddScoped<ICartServices, CartServices>();

//WishList
builder.Services.AddScoped<IWishListService, WishListService>();

//Order
builder.Services.AddScoped<IOrderService, OrderService>();



//Configuring services
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))

                        };
                    });
//Cart as a service
//builder.Services.AddScoped<ICartServices,CartServices>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
