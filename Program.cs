using System.Security.Claims;
using Microsoft.OpenApi.Models;
using TeamTaskManager;
using TeamTaskManager.Extensions;
using TeamTaskManager.Hubs;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your JWT token}"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.DbServiceRegister();
builder.RegisterAuth();
builder.RegisterServiceClasses();


builder.Services.AddSignalR();

var app = builder.Build();


await app.SeedDBAsync();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.Use(async (context, next) =>
//{
//    Console.WriteLine("\n========== REQUEST DEBUG ==========");

//    var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
//    Console.WriteLine($"📨 Authorization Header: {authHeader ?? "NULL"}");

//    // Decode JWT token if present
//    if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
//    {
//        var token = authHeader.Substring("Bearer ".Length).Trim();
//        Console.WriteLine("\n🔐 JWT Token Decoded:");

//        try
//        {
//            // Split the token into parts
//            var parts = token.Split('.');
//            if (parts.Length == 3)
//            {
//                // Decode the header (first part)
//                var headerJson = DecodeBase64Url(parts[0]);
//                Console.WriteLine($"📋 Header: {headerJson}");

//                // Decode the payload (second part) - this contains the claims
//                var payloadJson = DecodeBase64Url(parts[1]);
//                Console.WriteLine($"📋 Payload: {payloadJson}");

//                // Parse and display specific claims in a readable format
//                var payload = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(payloadJson);
//                if (payload != null)
//                {
//                    Console.WriteLine("\n📌 Claims:");
//                    foreach (var claim in payload)
//                    {
//                        Console.WriteLine($"  • {claim.Key}: {claim.Value}");
//                    }
//                }
//            }
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"❌ Error decoding token: {ex.Message}");
//        }
//    }

//    await next();

//    Console.WriteLine($"✅ After next() - Authenticated: {context.User?.Identity?.IsAuthenticated}");
//    Console.WriteLine($"✅ User roles: {string.Join(", ", context.User?.Claims.Where(c => c.Type == ClaimTypes.Role || c.Type == "Role").Select(c => c.Value) ?? Enumerable.Empty<string>())}");
//    Console.WriteLine("====================================\n");
//});

//// Helper method to decode Base64Url format used in JWT
//static string DecodeBase64Url(string base64Url)
//{
//    // Replace URL-safe characters
//    var base64 = base64Url.Replace('-', '+').Replace('_', '/');

//    // Add padding if necessary
//    switch (base64.Length % 4)
//    {
//        case 2: base64 += "=="; break;
//        case 3: base64 += "="; break;
//    }

//    var bytes = Convert.FromBase64String(base64);
//    return System.Text.Encoding.UTF8.GetString(bytes);
//}



app.UseAuthentication();
app.UseAuthorization();



app.MapHub<NotificationHub>("/notificationHub");
app.MapControllers();

app.Run();
