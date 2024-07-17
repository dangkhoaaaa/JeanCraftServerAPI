using Microsoft.IdentityModel.Tokens;
﻿using Microsoft.IdentityModel.Tokens;
using System.Text;
using JeanCraftLibrary.Repositories;
using JeanCraftServerAPI.Services;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using JeanCraftLibrary.Entity;
using AutoMapper;
using JeanCraftLibrary.Mapper;
using JeanCraftLibrary.Repositories.Interface;
using JeanCraftServerAPI.Services.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using JeanCraftLibrary;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Add services to the container.
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IComponentTypeService, ComponentTypeService>();
builder.Services.AddScoped<IComponentService, ComponentService>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
builder.Services.AddScoped<ICartItemService, CartItemService>();

builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductInventoryService, ProductInventoryService>();
builder.Services.AddScoped<IDesignOneService, DesignOneService>();
builder.Services.AddScoped<IDesignTwoService, DesignTwoService>();
builder.Services.AddScoped<IDesignThreeService, DesignThreeService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IVnPayService, VnPayService>();
builder.Services.AddSingleton<IVnPayService, VnPayService>();
// Add repositories to the container
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductInventoryRepository, ProductInventoryRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();
builder.Services.AddScoped<IComponentTypeRepository, ComponentTypeRepository>();
builder.Services.AddScoped<IComponentRepsitory, ComponentRepsitory>();
builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
builder.Services.AddScoped<IDesignOneRepository, DesignOneRepository>();
builder.Services.AddScoped<IDesignTwoRepository, DesignTwoRepository>();
builder.Services.AddScoped<IDesignThreeRepository, DesignThreeRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<DbContext, JeanCraftContext>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddDbContext<JeanCraftContext>(options => options.UseSqlServer(connectionString));
// Register Mapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        } );
builder.Services.AddAutoMapper(typeof(AutoMapperProduct));
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";

Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<StartupBase>();
            // Use environment variable for port
            webBuilder.UseUrls($"http://*:{port}");
        });

//CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("283CZXVU883423WT34GFJ6458MN23878GH2378Y23RH2785Y34THREWJ")) // You need to replace this with your actual secret key
                    };
                });
builder.Services.AddControllers();
var app = builder.Build();
app.UseCors("AllowAll");
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
