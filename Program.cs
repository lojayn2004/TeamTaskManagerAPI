using TeamTaskManager;
using TeamTaskManager.Extensions;
using TeamTaskManager.Hubs;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.DbServiceRegister();
builder.RegisterServiceClasses();
builder.RegisterAuth();


builder.Services.AddSignalR();

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
