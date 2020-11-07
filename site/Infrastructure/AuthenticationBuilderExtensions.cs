using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace site.Infrastructure
{
    public static class AuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddAuth0(this AuthenticationBuilder builder, IConfiguration configuration)
        {
            builder.AddOpenIdConnect("Auth0", options =>
            {
                // Set the authority to your Auth0 domain
                options.Authority = $"https://{configuration["Auth0:Domain"]}";

                // Configure the Auth0 Client ID and Client Secret
                options.ClientId = configuration["Auth0:ClientId"];
                options.ClientSecret = configuration["Auth0:ClientSecret"];

                // Set response type to code
                options.ResponseType = "code";

                // Configure the scope
                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");

                // Set the callback path, so Auth0 will call back to http://localhost:3000/callback
                // Also ensure that you have added the URL as an Allowed Callback URL in your Auth0 dashboard
                options.CallbackPath = new PathString("/callback");

                // Configure the Claims Issuer to be Auth0
                options.ClaimsIssuer = "Auth0";

                options.Events = new OpenIdConnectEvents
                {
                    // handle the logout redirection
                    OnRedirectToIdentityProviderForSignOut = (context) =>
                    {
                        var logoutUri =
                            $"https://{configuration["Auth0:Domain"]}/v2/logout?client_id={configuration["Auth0:ClientId"]}";

                        var postLogoutUri = context.Properties.RedirectUri;
                        if (!string.IsNullOrEmpty(postLogoutUri))
                        {
                            if (postLogoutUri.StartsWith("/"))
                            {
                                // transform to absolute
                                var request = context.Request;
                                postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase +
                                                postLogoutUri;
                            }

                            logoutUri += $"&returnTo={Uri.EscapeDataString(postLogoutUri)}";
                        }

                        context.Response.Redirect(logoutUri);
                        context.HandleResponse();

                        return Task.CompletedTask;
                    }
                    //,
                    //OnUserInformationReceived = (context) =>
                    //{
                    //    var idToken = context.ProtocolMessage.IdToken;

                    //    context.Principal.AddIdentity(new ClaimsIdentity(
                    //        new[]
                    //        {
                    //            new Claim("id_token", idToken)
                    //        }
                    //    ));

                    //    var jwtToken = new JwtSecurityToken(idToken);
                    //    context.Principal.AddIdentity(new ClaimsIdentity(
                    //        jwtToken.Claims,
                    //        "jwt",
                    //        context.Options.TokenValidationParameters.NameClaimType,
                    //        context.Options.TokenValidationParameters.RoleClaimType
                    //    ));

                    //    return Task.CompletedTask;
                    //}
                };
            });

            return builder;
        }
    }
}