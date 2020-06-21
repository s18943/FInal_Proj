using AdvertApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertApi.Services
{
    public class CalculateAreaService : ICalculationsService
    {
        public List<Banner> calculateArea(List<Building> buildings,Campaign campaign,decimal pricePerSquareMeter)
        {
            int howManyBuildings = buildings.Count();

            Banner banner1 = new Banner { };
            Banner banner2 = new Banner { };

            for (int i = 1; i < howManyBuildings - 1; i++)
            {
                decimal height1Max = 0;
                decimal height2Max = 0;
                Banner tmpBanner1 = new Banner { Name = 1, IdCampaign = campaign.IdCampaign };
                Banner tmpBanner2 = new Banner { Name = 2, IdCampaign = campaign.IdCampaign };
                for (int j = 0; j < howManyBuildings; j++)
                {
                    var tmpBuilding = buildings.ElementAt(j);

                    if (j < i && tmpBuilding.Height > height1Max)
                        height1Max = tmpBuilding.Height;

                    if (j == i)
                    {
                        tmpBanner1.Area = height1Max * i;
                        tmpBanner1.Price = tmpBanner1.Area * pricePerSquareMeter;
                    }

                    if (j >= i && tmpBuilding.Height > height2Max)
                        height2Max = tmpBuilding.Height;
                }
                tmpBanner2.Area = height2Max * (howManyBuildings - i);
                tmpBanner2.Price = tmpBanner2.Area * pricePerSquareMeter;
                if ((banner1.Area == 0 && banner2.Area == 0) || (tmpBanner1.Area + tmpBanner2.Area < banner1.Area + banner2.Area))
                {
                    banner1 = tmpBanner1;
                    banner2 = tmpBanner2;
                }
            }
            return new List<Banner> { banner1, banner2 };
        }
    }
}
