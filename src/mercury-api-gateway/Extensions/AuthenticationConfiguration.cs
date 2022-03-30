using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using mercury_api_gateway.Models.settings;

namespace mercury_api_gateway.Extensions
{
    public static class AuthenticationConfiguration
    {
        public static AuthenticationBuilder ConfigureAuthentication(this IServiceCollection services, Auth0Settings auth0Settings, IWebHostEnvironment webHostEnvironment)
            => services
                .AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.Authority = auth0Settings.Domain;
                    options.Audience = auth0Settings.Audience;

                    // If the access token does not have a `sub` claim, `User.Identity.Name` will be `null`. Map it to a different claim by setting the NameClaimType below.
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = auth0Settings.Domain,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = auth0Settings.Audience,
                        ValidateIssuerSigningKey = true,
                        NameClaimType = ClaimTypes.NameIdentifier
                    };

                    if (webHostEnvironment.IsDevelopment())
                    {
                        options.RequireHttpsMetadata = false;
                    }
                });
    }
}
