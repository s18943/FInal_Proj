using AdvertApi.DTOs.Responses;
using AdvertApi.Models;
using AdvertApi.Services;
using AdvertApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.UnitTests.Adverts
{
    [TestFixture]
    class AdvertGetCampaignsTests
    {
        [Test]
        public void GetCampaigns_Correct()
        {
            var dbLayer = new Mock<ICompanyDbService>();
            dbLayer.Setup(d => d.GetCampaigns()).Returns(new List<CampaignsResponse>()
            {
                new CampaignsResponse{Client = new Client{FirstName="Fname"},Campaign = new Campaign{PricePerSquareMeter=54}}
            });
            var cont = new CampaignsController(dbLayer.Object);

            var result = cont.GetCampaigns();

            Assert.IsNotNull(result);
            Assert.IsTrue(result is OkObjectResult);
        }
    }
}
