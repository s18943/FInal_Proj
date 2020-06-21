using AdvertApi.DTOs.Requests;
using AdvertApi.DTOs.Responses;
using AdvertApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace AdvertApi.Services
{
    public interface ICompanyDbService
    {
        public AddClientResponse AddClient(AddClientRequest request);
        public LoginClientResponse LoginClient(LoginClientRequest request);
        public TokenUpdateResponse UpdateToken(TokenUpdateRequest request);
        public ICollection<GetCampaignsResponse> GetCampaigns();
        public NewCampaignResponse NewCampaign(NewCampaignRequest request);
    }
}
