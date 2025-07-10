using System;
using Biddify.SignalR;
using DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Net.payOS;
using Repository;
using Repository.Impl;
using Service;
using Service.Impl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<BiddifyDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder
    .Services.AddIdentity<UserEntity, IdentityRole>(options =>
    {
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 8;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.User.RequireUniqueEmail = true;
        options.SignIn.RequireConfirmedAccount = false;
        options.SignIn.RequireConfirmedEmail = false;
        options.SignIn.RequireConfirmedPhoneNumber = false;
    })
    .AddEntityFrameworkStores<BiddifyDbContext>()
    .AddDefaultTokenProviders();
//DI PayOS key
var clientId = builder.Configuration["PayOS:ClientId"];
var apiKey = builder.Configuration["PayOS:ApiKey"];
var checksumKey = builder.Configuration["PayOS:ChecksumKey"];

builder.Services.AddSingleton(new PayOS(clientId, apiKey, checksumKey));

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login";
    options.LogoutPath = "/Logout";
    options.AccessDeniedPath = "/Error";
});

//Add SignalR service to the container
builder.Services.AddSignalR();

//Dependency Injection
builder.Services.AddScoped<IAuctionProductRepository, AuctionProductRepository>();
builder.Services.AddScoped<IBidRepository, BidRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddHttpClient<AIService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAuctionProductService, AuctionProductService>();
builder.Services.AddScoped<IBidService, BidService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<FAQService>();

builder.Services.AddRazorPages();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.MapHub<AuctionProductHub>("/AuctionProductHub");
app.MapHub<BidHub>("/BidHub");
app.MapHub<CommentHub>("/CommentHub");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
    
    app.MapRazorPages();
    app.MapControllers();
app.Run();
