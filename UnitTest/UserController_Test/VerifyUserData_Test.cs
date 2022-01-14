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
        public void IsEmailDuplicate_ReturnOk()
        {
            var config = A.Fake<IConfiguration>();
            var repo = A.Fake<IRepository>();

            A.CallTo(() => repo.List<User>(null))
                .Returns(GetDummyUser());

            var controller = new UserController(repo, config);
            var validate = new Validate();
            validate.EMAIL = "hendradummy@gmail.com";
            var result = controller.isEmailDuplicate(validate);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public void IsEmailDuplicate_ReturnBadRequest()
        {
            var config = A.Fake<IConfiguration>();
            var repo = A.Fake<IRepository>();

            A.CallTo(() => repo.List<User>(null))
                .Returns(GetDummyUser());

            var controller = new UserController(repo, config);
            var validate = new Validate();
            validate.EMAIL = "dummy1@dummy.com";
            var result = controller.isEmailDuplicate(validate);
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public void IsUsernameDuplicate_ReturnOk()
        {
            var config = A.Fake<IConfiguration>();
            var repo = A.Fake<IRepository>();

            A.CallTo(() => repo.List<User>(null))
                .Returns(GetDummyUser());

            var controller = new UserController(repo, config);
            var validate = new Validate();
            validate.USERNAME = "hendratest";
            var result = controller.isUserDuplicate(validate);
            Assert.IsInstanceOf<OkResult>(result);
        }
        [Test]
        public void IsUsernameDuplicate_ReturnBadRequest()
        {
            var config = A.Fake<IConfiguration>();
            var repo = A.Fake<IRepository>();

            A.CallTo(() => repo.List<User>(null))
                .Returns(GetDummyUser());

            var controller = new UserController(repo, config);
            var validate = new Validate();
            validate.USERNAME = "dummyUser";
            var result = controller.isUserDuplicate(validate);
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public void IsNIKDuplicate_ReturnOk()
        {
            var config = A.Fake<IConfiguration>();
            var repo = A.Fake<IRepository>();

            A.CallTo(() => repo.List<User>(null))
                .Returns(GetDummyUser());

            var controller = new UserController(repo, config);
            var validate = new Validate();
            validate.NIK = "1111111111111111";
            var result = controller.isNIKDuplicate(validate);
            Assert.IsInstanceOf<OkResult>(result);
        }
        [Test]
        public void IsNIKDuplicate_ReturnBadRequest()
        {
            var config = A.Fake<IConfiguration>();
            var repo = A.Fake<IRepository>();

            A.CallTo(() => repo.List<User>(null))
                .Returns(GetDummyUser());

            var controller = new UserController(repo, config);
            var validate = new Validate();
            validate.NIK = "1234567891012131";
            var result = controller.isNIKDuplicate(validate);
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public void IsPhoneDuplicate_ReturnOk()
        {
            var config = A.Fake<IConfiguration>();
            var repo = A.Fake<IRepository>();

            A.CallTo(() => repo.List<User>(null))
                .Returns(GetDummyUser());

            var controller = new UserController(repo, config);
            var validate = new Validate();
            validate.PHONE = "0811111111";
            var result = controller.isPhoneDuplicate(validate);
            Assert.IsInstanceOf<OkResult>(result);
        }
        [Test]
        public void IsPhoneDuplicate_ReturnBadRequest()
        {
            var config = A.Fake<IConfiguration>();
            var repo = A.Fake<IRepository>();

            A.CallTo(() => repo.List<User>(null))
                .Returns(GetDummyUser());

            var controller = new UserController(repo, config);
            var validate = new Validate();
            validate.PHONE = "08123456789";
            var result = controller.isPhoneDuplicate(validate);
            Assert.IsInstanceOf<BadRequestResult>(result);
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
