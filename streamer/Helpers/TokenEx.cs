using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Newtonsoft.Json;
using streamer.Features.interfaces.services;
using streamer.Features.User;
using streamer.Security;

namespace streamer.Helpers
{
    public static class TokenEx
    {
        public static async Task<string> GenerateJwt(this ClaimsIdentity identity, IJwtFactory jwtFactory, string userName, JwtIssuerOptions jwtOptions, JsonSerializerSettings serializerSettings)
        {
            var response = new
            {
                id = identity.Claims.Single(c => c.Type == "id").Value,
                auth_token = await jwtFactory.GenerateEncodedToken(userName, identity),
                expires_in = (int)jwtOptions.ValidFor.TotalSeconds
            };

            return JsonConvert.SerializeObject(response, serializerSettings);
        }

        public static async Task<AccessToken> GenerateJwt2(this ClaimsIdentity identity, IJwtFactory jwtFactory, string userName, JwtIssuerOptions jwtOptions, JsonSerializerSettings serializerSettings)
        {
            var auth_token = await jwtFactory.GenerateEncodedToken(userName, identity);

            return auth_token;
        }
    }
}
