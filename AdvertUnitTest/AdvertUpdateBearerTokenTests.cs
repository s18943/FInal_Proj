using AdvertApi.DTOs.Requests;
using AdvertApi.DTOs.Responses;
using AdvertApi.Services;
using AdvertApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace Tests.UnitTests.Adverts
{
    [TestFixture]
    class AdvertUpdateBearerTokenTests
    {
        [Test]
        public void RenewBearerTokenMethod_RequestComplete_Correct()
        {
            var dbLayer = new Mock<ICompanyDbService>();
            dbLayer.Setup(e => e.UpdateToken(new TokenUpdateRequest { RefreshToken = "Up!" }))
                .Returns(new TokenUpdateResponse { RefreshToken = "tok", AccessToken = "enUp!" });
            var cont = new ClientsController(dbLayer.Object);

            var result = cont.UpdateTokenClient(new TokenUpdateRequest { RefreshToken = "Up!" });

            Assert.IsNotNull(result);
            Assert.IsTrue(result is OkObjectResult);
        }       
    }
}
