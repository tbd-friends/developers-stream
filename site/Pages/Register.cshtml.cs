using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace site.Pages
{
    public class RegisterModel : PageModel
    {
        public IConfiguration Configuration { get; set; }
        public IManagementConnection ManagementConnection { get; set; }
        public IAuthenticationConnection AuthenticationConnection { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Source { get; set; }

        public RegisterModel(IConfiguration configuration,
            IManagementConnection managementConnection,
            IAuthenticationConnection authenticationConnection)
        {
            Configuration = configuration;
            ManagementConnection = managementConnection;
            AuthenticationConnection = authenticationConnection;
        }

        public async Task OnGet()
        {
            if (HttpContext.User.IsInRole("Streamer"))
            {
                Redirect(Source ?? "/");
            }

            var client = new AuthenticationApiClient(Configuration["Auth0:Domain"], AuthenticationConnection);

            var token = await client.GetTokenAsync(
                new ClientCredentialsTokenRequest
                {
                    Audience = Configuration["Auth0-Management:Audience"],
                    ClientId = Configuration["Auth0-Management:ClientId"],
                    ClientSecret = Configuration["Auth0-Management:ClientSecret"]
                });

            var management = new ManagementApiClient(
                token.AccessToken,
                Configuration["Auth0:Domain"],
                ManagementConnection);

            var claim = HttpContext.User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier);

            var rolePaged = await management.Roles.GetAllAsync(new GetRolesRequest { NameFilter = "Streamer" });

            await management.Roles.AssignUsersAsync(rolePaged.Single().Id, new AssignUsersRequest
            {
                Users = new[] { claim.Value }
            });

            await HttpContext.ChallengeAsync("Auth0", new AuthenticationProperties
            {
                RedirectUri = Source ?? "/"
            });
        }
    }
}
