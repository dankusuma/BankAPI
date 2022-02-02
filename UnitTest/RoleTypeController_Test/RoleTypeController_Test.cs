using Bank.Api.Controllers;
using Bank.Core.Entity;
using Bank.Core.Interface;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.RoleTypeController_Test
{
    [TestFixture]
    public class RoleTypeController_Test
    {
        private RoleType _role;
        public static IConfiguration _config;
        public static IRepository _repo;

        [SetUp]
        public void SetUp()
        {
            _config = A.Fake<IConfiguration>();
            _repo = A.Fake<IRepository>();

            _role = new()
            {
                ROLE = "dummy admin",
                DESCRIPTION = "dummy description"
            };
        }

        [Test]
        public void AddTest()
        {
            A.CallTo(() => _repo.Add(A<RoleType>.Ignored));
            //A.CallTo(() => _repo.List<RoleType>(null)).Returns(GetDummyList());

            var controller = new RoleTypeController(_repo);
            var result = controller.Add(_role) as OkResult;

            // Assert
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public void GetTest200()
        {
            A.CallTo(() => _repo.List<RoleType>(null)).Returns(GetDummyList());

            var controller = new RoleTypeController(_repo);
            var result = controller.Get() as OkObjectResult;

            // Assert
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public void GetTestIsNull()
        {
            A.CallTo(() => _repo.List<RoleType>(null)).Returns(GetDummyList());

            var controller = new RoleTypeController(_repo);
            var result = controller.Get() as OkObjectResult;

            // Assert
            Assert.NotNull(result.Value);
        }


        private List<RoleType> GetDummyList()
        {
            List<RoleType> dummy = new List<RoleType>()
            {
                new RoleType {
                ROLE = "dummy admin",
                DESCRIPTION = "dummy description"
                },
        };
            return dummy;
        }
    }
}
