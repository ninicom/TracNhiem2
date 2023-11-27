using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TracNhiem2.Data;
using Microsoft.AspNetCore.Identity;
using TracNhiem2.Hubs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TracNhiem2Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TracNhiem2Context") ?? throw new InvalidOperationException("Connection string 'TracNhiem2Context' not found.")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<TracNhiem2Context>();

builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
//// đăng nhập:
//builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<TracNhiem2Context>().AddDefaultTokenProviders();
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//})


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.MapHub<ChatHub>("/ChatHub");
app.Run();
