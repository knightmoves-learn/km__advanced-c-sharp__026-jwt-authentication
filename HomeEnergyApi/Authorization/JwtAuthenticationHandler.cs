using System.Text;
using System.Text.Encodings.Web;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HomeEnergyApi.Authorization
{
    public class JwtAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        // private readonly string _username;
        // private readonly string _password;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly string _secret;

        public JwtAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IConfiguration configuration) : base(options, logger, encoder, clock)
        {
            // _username = configuration["BasicAuth:Username"];
            // _password = configuration["BasicAuth:Password"];
            _issuer = configuration["Jwt:Issuer"];
            _audience = configuration["Jwt:Audience"];
            _secret = configuration["Jwt:Secret"];
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Missing Authorization header");
            }

            try
            {
                // var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                // var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                // var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);
                // var username = credentials[0];
                // var password = credentials[1];

                // if (username == _username && password == _password)
                // {
                //     var claims = new[]
                //     {
                //     new Claim(ClaimTypes.Name, username)
                // };

                //     var identity = new ClaimsIdentity(claims, Scheme.Name);
                //     var principal = new ClaimsPrincipal(identity);
                //     var ticket = new AuthenticationTicket(principal, Scheme.Name);

                //     return AuthenticateResult.Success(ticket);
                // }

                // return AuthenticateResult.Fail("Invalid username or password");

                var token = Request.Headers["Authorization"].ToString().Split(" ").Last();

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_secret);
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _issuer,
                    ValidAudience = _audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization header");
            }
        }
    }
}