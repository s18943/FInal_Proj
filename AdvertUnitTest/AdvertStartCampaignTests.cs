using AdvertApi.DTOs.Requests;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tests.UnitTests.Adverts
{
    [TestFixture]
    class AdvertStartCampaignTests
    {
        [Test]
        public void AddCampaignMethod_CompleteRequest_Correct()
        {
            NewCampaignRequest req = new NewCampaignRequest 
            { 
                IdClient = 5,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(40),
                PricePerSquareMeter = 3, 
                FromIdBuilding = 1,
                ToIdBuilding = 4 
            };
            var context = new ValidationContext(req, null, null);
            var results = new List<ValidationResult>();

            var isModelStateValid = Validator.TryValidateObject(req, context, results, true);

            Assert.IsTrue(isModelStateValid);
            Assert.IsTrue(results.Count == 0);
        }

    }
}
