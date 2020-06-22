using AdvertApi.Controllers;
using AdvertApi.DTOs.Requests;
using AdvertApi.DTOs.Responses;
using AdvertApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Tests.UnitTests.Adverts
{
    [TestFixture]
    class AdvertLoginClientTests
    {
        [Test]
        public void LoginClientMethod_ClientExists_Correct()
        {
            var dbLayer = new Mock<ICompanyDbService>();
            dbLayer.Setup(e => e.LoginClient(new AdvertApi.DTOs.Requests.LoginClientRequest { Login = "Micha", Password = "ddqd~45YE" }))
                .Returns(new LoginClientResponse { RefreshToken = "updateeee)", AccessToken = "GotAccess" });
            var cont = new ClientsController(dbLayer.Object);

            var result = cont.LoginClient(new AdvertApi.DTOs.Requests.LoginClientRequest { Login = "Micha", Password = "ddqd~45YE" });
            
            Assert.IsNotNull(result);
            Assert.IsTrue(result is OkObjectResult);
        }
        [Test]
        public void LoginClientMethod_CompleteRequest_Correct()
        {
            LoginClientRequest req = new LoginClientRequest { Login = "Micha", Password = "ddqd~45YE" };
            var context = new ValidationContext(req, null, null);
            var results = new List<ValidationResult>();

            var isModelStateValid = Validator.TryValidateObject(req, context, results, true);

            Assert.IsTrue(isModelStateValid);
            Assert.IsTrue(results.Count == 0);
        }
        [Test]
        public void LoginClientMethod_PasswordTooShort_Correct()
        {
            LoginClientRequest req = new LoginClientRequest { Login = "admin", Password = "admin" };
            var context = new ValidationContext(req, null, null);
            var results = new List<ValidationResult>();

            var isModelStateValid = Validator.TryValidateObject(req, context, results, true);

            Assert.IsFalse(isModelStateValid);
            Assert.IsTrue(results.Count == 1);
            Assert.IsTrue(results[0].MemberNames.ElementAt(0) == nameof(AddClientRequest.Password));
        }
        [Test]
        public void LoginClientMethod_NoPassword_Incorrect()
        {
            LoginClientRequest req = new LoginClientRequest { Login = "Micha" };
            var context = new ValidationContext(req, null, null);
            var results = new List<ValidationResult>();

            var isModelStateValid = Validator.TryValidateObject(req, context, results, true);

            Assert.IsFalse(isModelStateValid);
            Assert.IsTrue(results.Count == 1);
            Assert.IsTrue(results[0].MemberNames.ElementAt(0) == nameof(AddClientRequest.Password));
        }
        
    }
}
