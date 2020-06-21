using AdvertApi.Models;
using System.Collections.Generic;


namespace AdvertApi.Services
{
    public interface ICalculationsService
    {
        List<Banner> calculateArea(List<Building> buildings,Campaign campaign,decimal pricePerSquareMeter);
    }
}
