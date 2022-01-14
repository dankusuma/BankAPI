using Bank.Api.Controllers;
using Bank.Core;
using Bank.Core.Entity;
using Bank.Core.Interface;
using FakeItEasy;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace UnitTest.UserController_Test
{
    public class Authenticate_Test
    {
        private User _user;
        private LoginModel _login;

        private IConfiguration _config;

        private List<RefMaster> _refMasterDummyList = new List<RefMaster>() {
            new RefMaster(){
                MASTER_GROUP = "SETTING",
                MASTER_CODE="MAX_LOGIN",
                VALUE = "10"
            },
            new RefMaster(){
                MASTER_GROUP = "SETTING",
                MASTER_CODE="MAX_HOLD",
                VALUE = "15"
            }

        };

        [SetUp]
        public void SetUp()
        {
            _config = A.Fake<IConfiguration>();

            _user = GetDummyUser()[0];
            _login = new LoginModel()
            {
                USERNAME = "dummyUser",
                PASSWORD = "DUMMY_DUMMY"
            };
        }

        [Test]
        public void VerifyUser_ReturnTrue()
        {
            var repo = A.Fake<IRepository>();
            var controller = new UserController(repo, _config);


            A.CallTo(() => repo.Update(_user)).DoesNothing();
            A.CallTo(() => repo.List<User>(null)).Returns(GetDummyHashedUser());
            A.CallTo(() => repo.List<RefMaster>(null)).Returns(_refMasterDummyList);
            var result = controller.VerifyUser(_login.USERNAME, _login.PASSWORD);

            Assert.NotNull(result);
        }

        [Test]
        public void Verify_SuspendedUser()
        {
            var repo = A.Fake<IRepository>();
            var controller = new UserController(repo, _config);

            DateTime dummyTime = DateTime.Now.AddHours(1);
            var users = GetDummyHashedUser();
            foreach (User u in users) u.LOGIN_HOLD = dummyTime;

            A.CallTo(() => repo.Update(_user)).DoesNothing();
            A.CallTo(() => repo.List<User>(null)).Returns(users);
            A.CallTo(() => repo.List<RefMaster>(null)).Returns(_refMasterDummyList);

            Assert.Throws<MethodAccessException>(() => controller.VerifyUser(_login.USERNAME, _login.PASSWORD));
        }

        [TestCase("", "")]
        [TestCase("", "dummy")]
        [TestCase("dummy", "")]
        [TestCase("dummy", "dummy")]
        public void VerifyUser_ReturnFalse(string us, string psw)
        {
            var repo = A.Fake<IRepository>();
            var controller = new UserController(repo, _config);

            var login = new LoginModel { USERNAME = us, PASSWORD = psw };

            A.CallTo(() => repo.Update(_user)).DoesNothing();
            A.CallTo(() => repo.List<User>(null)).Returns(GetDummyHashedUser());
            A.CallTo(() => repo.List<RefMaster>(null)).Returns(_refMasterDummyList);

            Assert.Throws<UnauthorizedAccessException>(() => controller.VerifyUser(login.USERNAME, login.PASSWORD));
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

        private List<User> GetDummyHashedUser()
        {
            var users = GetDummyUser();

            users.ForEach(x => x.HashPassword());

            return users;
        }
    }
}
