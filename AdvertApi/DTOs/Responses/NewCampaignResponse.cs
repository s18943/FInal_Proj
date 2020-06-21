using AdvertApi.Models;

namespace AdvertApi.DTOs.Responses
{
    public class NewCampaignResponse
    {
        public Campaign Campaign { get; set; }
        public Banner Banner1 { get; set; }
        public Banner Banner2 { get; set; }
    }
}
