using BookStore.API.Data;
using BookStore.API.Helpers;
using BookStore.API.Models;
using BookStore.API.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 3. Add services to the container. with Json Newtonsof
builder.Services.AddControllers().AddNewtonsoftJson();

// 3. For Jsonpatch
//builder.Services.AddControllers(options =>
//{
//    options.InputFormatters.Insert(0, MyJPIF.GetJsonPatchInputFormatter());
//});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// 1. Add services 
builder.Services.AddTransient<IBookRepository, BookRepository>();
// 2. Add the db context 
builder.Services.AddDbContext<BookStoreContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("BookStoreDB")));
// 4. Add auto mapper 
builder.Services.AddAutoMapper(typeof(ApplicationMapper).Assembly);
// 5. Add Identity Core 
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<BookStoreContext>()
    .AddDefaultTokenProviders();
// 6. Add the account repository
builder.Services.AddTransient<IAccountRepository, AccountRepository>();
// 8. configure token for JWT 
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.SaveToken = true;
    option.RequireHttpsMetadata = false;
    option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 7.Use Authorization
app.UseAuthorization();

app.UseAuthorization();

app.MapControllers();

app.Run();
