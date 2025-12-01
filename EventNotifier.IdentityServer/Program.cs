using EventNotifier.IdentityServer.Data.Models;
using EventNotifier.IdentityServer.Data;
using Microsoft.AspNetCore.Identity;
using EventNotifier.IdentityServer;
using System.Text;
using EventNotifier.IdentityServer.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<IdenityServerDbContext>(options =>
    options.UseSqlServer(connectionString!));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<IdenityServerDbContext>().AddDefaultTokenProviders();

var applicationSettingsConfiguration = builder.Configuration.GetSection("ApplicationSettings");
builder.Services.Configure<AppSettings>(applicationSettingsConfiguration);

var appSettings = applicationSettingsConfiguration.Get<AppSettings>();
var key = Encoding.ASCII.GetBytes(appSettings.Secret);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
     .AddJwtBearer(x =>
     {
         x.RequireHttpsMetadata = false;
         x.SaveToken = true;
         x.TokenValidationParameters = new TokenValidationParameters
         {
             ValidateIssuerSigningKey = true,
             IssuerSigningKey = new SymmetricSecurityKey(key),
             ValidateIssuer = false,
             ValidateAudience = false
         };
     });

builder.Services.ConfigureApplicationCookie(o => {
    o.ExpireTimeSpan = TimeSpan.FromDays(5);
    o.SlidingExpiration = true;
});

builder.Services.Configure<DataProtectionTokenProviderOptions>(o =>
       o.TokenLifespan = TimeSpan.FromHours(3));

builder.Services.AddAuthorization();
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCors();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors(options => options
 .WithOrigins(appSettings.ClientUrl,
              appSettings.EventNotifierUrl)
              .AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.ApplyMigrations();
app.Run();
