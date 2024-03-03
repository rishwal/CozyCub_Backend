using CozyCub;
using CozyCub.JWT_Id;
using CozyCub.Mappings;
using CozyCub.Payments.Orders;
using CozyCub.Services.Auth;
using CozyCub.Services.CartServices;
using CozyCub.Services.Category_services;
using CozyCub.Services.JWT_Id;
using CozyCub.Services.ProductService;
using CozyCub.Services.UserServices;
using CozyCub.Services.WishList;
using CozyCub.Services.WishList_Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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

//JWt
builder.Services.AddScoped<IJwtService, JwtService>();


//Adding Authentication service for JWT

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

//configures CORS (Cross-Origin Resource Sharing) for React 
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});


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

app.UseCors("ReactPolicy");

app.UseStaticFiles();

//Authentication
app.UseAuthentication();

//Authorization
app.UseAuthorization();

app.MapControllers();

app.Run();
