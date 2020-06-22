using AdvertApi.DTOs.Requests;
using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Tests.UnitTests.Adverts
{
    [TestFixture]
    class AdvertAddClientsTests
    {
        [Test]
        public void AddClientMethod_IncompleteRequest_Incorrect()
        {
            AddClientRequest req = new AddClientRequest { Email = "MyMail@advert.com", Phone = "223010200", Login = "Chelik", Password = "ddqd~45YE" };
            var context = new ValidationContext(req, null, null);
            var results = new List<ValidationResult>();

            var isModelStateValid = Validator.TryValidateObject(req, context, results, true);

            Assert.IsFalse(isModelStateValid);
            Assert.IsTrue(results.Count == 2);
        } 
        [Test]
        public void AddClientMethod_CompleteRequest_Correct()
        {
            AddClientRequest req = new AddClientRequest { FirstName = "Micha", LastName = "Kosterny", Email = "MyMail@advert.com", Phone = "223010200", Login = "Chelik", Password = "ddqd~45YE" };
            var context = new ValidationContext(req, null, null);
            var results = new List<ValidationResult>();

            var isModelStateValid = Validator.TryValidateObject(req, context, results, true);

            Assert.IsTrue(isModelStateValid);
            Assert.IsTrue(results.Count == 0);
        }
        [Test]
        public void AddClientMethod_PasswordTooShort_Incorrect()
        {
            AddClientRequest req = new AddClientRequest { FirstName = "Micha", LastName = "Kosterny", Email = "MyMail@advert.com", Phone = "223010200", Login = "Chelik", Password = "qwronfu" };
            var context = new ValidationContext(req, null, null);
            var results = new List<ValidationResult>();

            var isModelStateValid = Validator.TryValidateObject(req, context, results, true);

            Assert.IsFalse(isModelStateValid);
            Assert.IsTrue(results.Count == 1);
            Assert.IsTrue(results[0].MemberNames.ElementAt(0) == nameof(AddClientRequest.Password));
        }
       
    }
}
