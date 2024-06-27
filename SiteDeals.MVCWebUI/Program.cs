using Autofac;

using Microsoft.EntityFrameworkCore;
using SiteDeals.Core.Model;
using SiteDeals.MVCWebUI.AutoFac_Module;
using SiteDeals.Repository;
using SiteDeals.Repository.Repositories;
using SiteDeals.Service.Mapping;
using SiteDeals.Service.Service;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.AspNetCore.Identity;
using SiteDeals.MVCWebUI.Data;
using SiteDeals.MVCWebUI.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using SiteDeals.MVCWebUI.Services;
using Microsoft.AspNetCore.HttpOverrides;
using SiteDeals.MVCWebUI.CustomValidations;
using MassTransit;
using Autofac.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("SiteDealsMVCWebUIContextConnection") ?? throw new InvalidOperationException("Connection string 'SiteDealsMVCWebUIContextConnection' not found.");

builder.Services.AddDbContext<SiteDealsMVCWebUIContext>(options =>
    options.UseSqlServer(connectionString)); ;

builder.Services.AddIdentity<SiteDealsMVCWebUIUser,SiteDealsMVCWebIRole>(options => { 
    options.User.RequireUniqueEmail = true; 
    options.User.AllowedUserNameCharacters = "abcçdefgðhýijklmnoçpqrsþtuüvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 -._+@";
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
}).AddPasswordValidator<CustomPasswordValidator>()
.AddUserValidator<CustomUserValidator>()
.AddErrorDescriber<CustomIdentityErrorDescriber>()
    .AddEntityFrameworkStores<SiteDealsMVCWebUIContext>().AddDefaultUI().AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
//builder.Services.AddAutoMapper(typeof(MapProfile));


builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(connectionString), ServiceLifetime.Transient);
//builder.Services.AddScoped<UserRepository>();

builder.Services.AddTransient<IEmailSender, EmailSender>();
CookieBuilder cookieBuilder = new CookieBuilder();

cookieBuilder.Name = "SiteDeals";
cookieBuilder.HttpOnly = false;
cookieBuilder.SameSite = SameSiteMode.Lax;
cookieBuilder.SecurePolicy = CookieSecurePolicy.SameAsRequest;
builder.Services.ConfigureApplicationCookie(opts =>
{
    opts.LoginPath = new PathString("/User/Login");
    opts.LogoutPath = new PathString("/User/Logout");
    opts.Cookie = cookieBuilder;
    opts.SlidingExpiration = true;
    opts.ExpireTimeSpan = System.TimeSpan.FromDays(60);
    opts.AccessDeniedPath = new PathString("/Product/Index");

});
builder.Services.AddMvc();

//builder.Services.AddDbContextFactory<AppDbContext>(z => z.UseSqlServer(connectionString));

//builder.Services.AddHttpClient<ProductService>(opt =>
//{

//    opt.BaseAddress = new Uri(builder.Configuration["BaseUrl"]);

//});

//builder.Services.AddMassTransit(x =>
//{
//    //x.AddConsumer<ValueEnteredEventConsumer>();

//    x.UsingRabbitMq((context, cfg) =>
//    {
//        cfg.Host("78.135.106.202", "/", h =>
//        {
//            h.Username("admin");
//            h.Password("sanane");
//        });
//        cfg.ConfigureEndpoints(context);
//    });

//    x.SetKebabCaseEndpointNameFormatter();

//});

builder.Host.UseServiceProviderFactory
    (new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new RepositoryServiceModule());
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor |
    ForwardedHeaders.XForwardedProto
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); ;

app.UseAuthorization();
app.MapRazorPages();





app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
