using Bank.Api.Controllers;
using Bank.Core.Entity;
using Bank.Core.Interface;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

namespace UnitTest.UserController_Test
{
    public class Password_Test
    {
        private IConfiguration _config;
        private UserController _controller;
        private User _user;

        private static readonly List<RefMaster> _dummyEmailRefs = new List<RefMaster>() {
            new RefMaster() {
                MASTER_GROUP = "EMAIL",
                MASTER_CODE = "MAIL_FROM",
                VALUE="VALUE kaka"
            },
            new RefMaster() {
                MASTER_GROUP = "EMAIL",
                MASTER_CODE = "MAIL_SUBJECT_RESET",
                VALUE="RESET PASSWORD"
            },
            new RefMaster() {
                MASTER_GROUP = "EMAIL",
                MASTER_CODE = "MAIL_SIGNATURE",
                VALUE="VALUE kaka"
            },
            new RefMaster()
            {
                MASTER_GROUP = "EMAIL",
                MASTER_CODE = "MAIL_BODY_TEMPLATE_PATH",
                VALUE="VALUE kaka"
            },
            new RefMaster()
            {
                MASTER_GROUP = "EMAIL",
                MASTER_CODE = "SMTP_SSL",
                VALUE="VALUE kaka"
            },
            new RefMaster()
            {
                MASTER_GROUP = "EMAIL",
                MASTER_CODE = "SMTP_DEFAULT_CREDENTIALS",
                VALUE="VALUE kaka"
            },
            new RefMaster()
            {
                MASTER_GROUP = "EMAIL",
                MASTER_CODE = "MAILKIT_SECURE_POCKET_OPTION",
                VALUE="VALUE kaka"
            },
            new RefMaster()
            {
                MASTER_GROUP = "EMAIL",
                MASTER_CODE = "SMTP_SERVER",
                VALUE="VALUE kaka"
            },
            new RefMaster()
            {
                MASTER_GROUP = "EMAIL",
                MASTER_CODE = "SMTP_PORT",
                VALUE = 1233 + ""
            },
            new RefMaster()
            {
                MASTER_GROUP = "EMAIL",
                MASTER_CODE = "MAIL_FROM_PASSWORD",
                VALUE="VALUE kaka"
            },
            new RefMaster()
            {
                MASTER_GROUP = "EMAIL",
                MASTER_CODE = "MAIL_LINK",
                VALUE="VALUE kaka"
            }
        };

        [SetUp]
        public void SetUp()
        {
            _user = GetDummyUser()[1];

            _config = A.Fake<IConfiguration>();
        }

        #region Request Forgot password
        // TODO: mock about sending email here....

        [TestCase("dummy1@dummy.com")]
        [TestCase("dummy2@dummy.com")]
        [TestCase("dummy3@dummy.com")]
        public void EmailValid_ReturnTrue(string email)
        {
            var repo = A.Fake<IRepository>();

            A.CallTo(() => repo.Update(_user)).DoesNothing();
            A.CallTo(() => repo.List<User>(null)).Returns(GetDummyUser());
            A.CallTo(() => repo.List<RefMaster>(null)).Returns(_dummyEmailRefs);
            _controller = new UserController(repo, _config);

            Assert.IsEmpty(_controller.IsEmailValid(new ForgotPassword { EMAIL = email }));
        }

        [Test]
        public void EmailExists_ReturnTrue()
        {
            var repo = A.Fake<IRepository>();


            A.CallTo(() => repo.List<User>(null)).Returns(GetDummyUser());
            A.CallTo(() => repo.List<RefMaster>(null)).Returns(_dummyEmailRefs);
            _controller = new UserController(repo, _config);

            Assert.IsEmpty(_controller.IsEmailExists(_user));
        }

        [TestCase("dum@dumm")]
        [TestCase("du.dummy.com")]
        [TestCase("aa@.com")]
        public void EmailValid_ReturnFalse(string email)
        {
            var repo = A.Fake<IRepository>();

            A.CallTo(() => repo.List<User>(null)).Returns(GetDummyUser());
            A.CallTo(() => repo.List<RefMaster>(null)).Returns(_dummyEmailRefs);
            _controller = new UserController(repo, _config);

            Assert.IsNotEmpty(_controller.IsEmailValid(new ForgotPassword { EMAIL = email }));
        }

        [TestCase("bebelac@gmail.com")]
        [TestCase("saiful.setiawan@mitrais.com")]
        [TestCase("hendra@gmail.com")]
        [TestCase("reginanana16@gmail.com")]
        [TestCase("toshiro89@gmail.com")]
        public void EmailExists_ReturnFalse(string email)
        {
            var repo = A.Fake<IRepository>();
            _user.EMAIL = email;

            A.CallTo(() => repo.List<User>(null)).Returns(GetDummyUser());
            A.CallTo(() => repo.List<RefMaster>(null)).Returns(_dummyEmailRefs);
            _controller = new UserController(repo, _config);

            Assert.IsEmpty(_controller.IsEmailExists(_user));
        }
        #endregion
        #region Change password
        [Test]
        public void ChangePassword_Valid()
        {
            var repo = A.Fake<IRepository>();
            ChangePassword change = new ChangePassword()
            {
                USERNAME = "dummyUser",
                PASSWORD = "DUMMY_DUMMY",
                NEW_PASSWORD = "newDummyPassword",
                MODE = "change",
                REFF = "",
                TOKEN = "this is not a token"
            };

            A.CallTo(() => repo.Update(_user)).DoesNothing();
            A.CallTo(() => repo.List<User>(null)).Returns(GetDummyUser());

            _controller = new UserController(repo, _config);
            Assert.IsInstanceOf<OkObjectResult>(_controller.ChangePassword(change));
        }

        [TestCase("dummyUser", "DUMMY_DUMMY", "newDummyPassword", "change", "", "")]
        [TestCase("", "DUMMY_DUMMY", "newDummyPassword", "change", "", "this is not a token")]
        [TestCase("dummyUser", "", "newDummyPassword", "change", "", "this is not a token")]
        [TestCase("dummyUser", "DUMMY_DUMMY", "", "change", "", "this is not a token")]
        [TestCase("dummyUser", "DUMMY_DUMMY", "newDummyPassword", "create", "", "this is not a token")]
        [TestCase("user_kosong", "DUMMY_DUMMY", "newDummyPassword", "change", "", "this is not a token")]
        public void ChangePassword_ReturnFalse(string username, string password, string newPassword, string mode, string reff, string token)
        {
            ChangePassword changePassword = new ChangePassword { USERNAME = username, PASSWORD = password, NEW_PASSWORD = newPassword, MODE = mode, REFF = reff, TOKEN = token };
            var repo = A.Fake<IRepository>();

            A.CallTo(() => repo.Update(_user)).DoesNothing();
            A.CallTo(() => repo.List<User>(null)).Returns(GetDummyUser());

            _controller = new UserController(repo, _config);
            Assert.Throws<InvalidDataException>(() => _controller.ChangePassword(changePassword));
        }
        #endregion

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
                    USER_TYPE = "user",
                    CHANGE_PASSWORD_TOKEN="this is not a token"
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
