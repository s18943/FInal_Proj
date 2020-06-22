using AdvertApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertApi.DTOs.Responses
{
    public class CampaignsResponse
    {
        public Campaign Campaign { get; set; }
        public Client Client { get; set; }
    }
}
