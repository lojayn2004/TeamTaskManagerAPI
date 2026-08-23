using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TeamTaskManager.Dtos.Auth;

namespace TeamTaskManager.Extensions
{
    public static class AuthServiceRegisterationExtension
    {
        public static WebApplicationBuilder RegisterAuth(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<JwtConfigurations>(builder.Configuration.GetSection("JWT"));


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
            builder.Services.AddAuthorization();

            return builder;


        }
    }
}
