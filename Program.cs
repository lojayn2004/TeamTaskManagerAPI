using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TeamTaskManager;
using TeamTaskManager.Data;
using TeamTaskManager.Domain;
using TeamTaskManager.Dtos.Auth;
using TeamTaskManager.Hubs;
using TeamTaskManager.MappingProfiles;
using TeamTaskManager.Repositories.Contracts;
using TeamTaskManager.Repositories.Implementations;
using TeamTaskManager.Services.Contracts;
using TeamTaskManager.Services.Implementations;
using TeamTaskManager.Services.ServicesAbstractions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<JwtConfigurations>(builder.Configuration.GetSection("JWT"));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()  
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddSignalR();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IProjectRepo, ProjectRepo>();

builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ITaskRepo, TaskRepo>();
builder.Services.AddScoped<IUserTaskService, UserTaskService>();

builder.Services.AddScoped<INotificationRepo, NotificationRepo>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
        };
    });


builder.Services.AddAutoMapper(cfg => { }, typeof(ProjectProfile));

builder.Services.AddAuthorization();

var app = builder.Build();


await app.SeedDBAsync();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapHub<NotificationHub>("/notificationHub");



app.MapControllers();

app.Run();
