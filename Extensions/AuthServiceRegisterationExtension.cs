using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using TeamTaskManager.Dtos.Auth;

namespace TeamTaskManager.Extensions
{
    public static class AuthServiceRegisterationExtension
    {
        public static WebApplicationBuilder RegisterAuth(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<JwtConfigurations>(builder.Configuration.GetSection("JWT"));

           

            builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
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

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("ManagerOnly", policy =>
                    policy.RequireRole("Manager"));


                options.AddPolicy("EmployeeOnly", policy =>
                    policy.RequireRole("Employee"));
            });



            return builder;


        }
    }
}
