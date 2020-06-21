using Newtonsoft.Json;

namespace AdvertApi.DTOs.Responses
{
    public class TokenUpdateResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
