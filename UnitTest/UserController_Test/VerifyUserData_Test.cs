using Bank.Api.Controllers;
using Bank.Core.Entity;
using Bank.Core.Interface;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System.Collections.Generic;

namespace UnitTest.UserController_Test
{
    public class VerifyUserData_Test
    {


        [Test]
        public void IsEmailDuplicate_ReturnFalse()
        {
            var config = A.Fake<IConfiguration>();
            var repo = A.Fake<IRepository>();
            var user = GetDummyUser()[0];

            A.CallTo(() => repo.List<User>(null))
                .Returns(GetDummyUser());

            var controller = new UserController(repo, config);

            var result = controller.isEmailDuplicate(user.EMAIL);
            Assert.IsInstanceOf<UnauthorizedObjectResult>(result);
        }

        [Test]
        public void IsEmailDuplicate_ReturnTrue()
        {
            var config = A.Fake<IConfiguration>();
            var repo = A.Fake<IRepository>();
            var email = "dummy@dummy.com";

            A.CallTo(() => repo.List<User>(null))
                .Returns(GetDummyUser());

            var controller = new UserController(repo, config);

            var result = controller.isEmailDuplicate(email);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void IsUsernameDuplicate_ReturnTrue()
        {
            var config = A.Fake<IConfiguration>();
            var repo = A.Fake<IRepository>();

            A.CallTo(() => repo.List<User>(null))
                .Returns(GetDummyUser());

            var controller = new UserController(repo, config);

            var result = controller.isUserDuplicate("Dummy Dummy");
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [TestCase("dummyUser")]
        [TestCase("dummyUser1")]
        [TestCase("dummyUser2")]
        public void IsUsernameDuplicate_ReturnFalse(string username)
        {
            var config = A.Fake<IConfiguration>();
            var repo = A.Fake<IRepository>();

            A.CallTo(() => repo.List<User>(null))
                .Returns(GetDummyUser());

            var controller = new UserController(repo, config);

            var result = controller.isUserDuplicate(username);
            Assert.IsInstanceOf<UnauthorizedObjectResult>(result);
        }
        private List<User> GetDummyUser()
        {
            List<User> dummy = new List<User>()
            {
                new User{
                    USERNAME = "dummyUser",
                    NAME = "DUMMY DUMMY",
                    PASSWORD = "DUMMY_DUMMY",
                    MOTHER_MAIDEN_NAME = "MaidenName",
                    ADDRESS = "DUMMY ADDRESS",
                    KELURAHAN = "DUMMY LURAH",
                    KECAMATAN="DUMMY KECAMATAN",
                    KABUPATEN_KOTA = "DUMMY KOTA",
                    PROVINCE = "DUMMY PROVINCE",
                    BIRTH_PLACE = "DUMMY CITY",
                    EMAIL = "dummy1@dummy.com",
                    GENDER = 'M',
                    JOB = "PNS",
                    PHONE = "08123456789",
                    NIK = "1234567891012131",
                    MARITAL_STATUS = "Lajang",
                    FOTO_KTP_SELFIE = "DUMMY KTP LINK",
                    VIDEO = "DUMMY VIDEO LINK",
                    USER_TYPE = "user"
                },
                new User{
                    USERNAME = "dummyUser1",
                    NAME = "DUMMY KEDUA KAKA",
                    PASSWORD = "DUMMY_DUMMY",
                    MOTHER_MAIDEN_NAME = "MaidenName",
                    ADDRESS = "DUMMY ADDRESS",
                    KELURAHAN = "DUMMY LURAH",
                    KECAMATAN="DUMMY KECAMATAN",
                    KABUPATEN_KOTA = "DUMMY KOTA",
                    PROVINCE = "DUMMY PROVINCE",
                    BIRTH_PLACE = "DUMMY CITY",
                    EMAIL = "dummy2@dummy.com",
                    GENDER = 'M',
                    JOB = "PNS",
                    PHONE = "08123456789",
                    NIK = "1234567891012131",
                    MARITAL_STATUS = "Lajang",
                    FOTO_KTP_SELFIE = "DUMMY KTP LINK",
                    VIDEO = "DUMMY VIDEO LINK",
                    USER_TYPE = "user"
                },
                new User{
                    USERNAME = "dummyUser2",
                    NAME = "DUMMY KETIGA",
                    PASSWORD = "DUMMY_DUMMY",
                    MOTHER_MAIDEN_NAME = "MaidenName",
                    ADDRESS = "DUMMY ADDRESS",
                    KELURAHAN = "DUMMY LURAH",
                    KECAMATAN="DUMMY KECAMATAN",
                    KABUPATEN_KOTA = "DUMMY KOTA",
                    PROVINCE = "DUMMY PROVINCE",
                    BIRTH_PLACE = "DUMMY CITY",
                    EMAIL = "dummy3@dummy.com",
                    GENDER = 'M',
                    JOB = "PNS",
                    PHONE = "08123456789",
                    NIK = "1234567891012131",
                    MARITAL_STATUS = "Lajang",
                    FOTO_KTP_SELFIE = "DUMMY KTP LINK",
                    VIDEO = "DUMMY VIDEO LINK",
                    USER_TYPE = "user"
                },
        };
            return dummy;
        }
    }
}
