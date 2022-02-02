using Bank.Api.Controllers;
using Bank.Core.Entity;
using Bank.Core.Interface;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace UnitTest.UserController_Test
{
    public class Password_Test
    {
        private IConfiguration _config;
        public static IRepository _repo;

        private UserController _controller;
        private User _user;
        private List<User> _users;
        private List<RefMaster> _dummyEmailRefs;
        private ChangePassword _chgPass;

        [SetUp]
        public void SetUp()
        {
            _config = A.Fake<IConfiguration>();
            _repo = A.Fake<IRepository>();
            _user = GetDummyUser()[1];
            _users = GetDummyUser();
            _dummyEmailRefs = GetDummyEmailRefs();

            A.CallTo(() => _repo.List<User>(null)).Returns(_users);
            A.CallTo(() => _repo.List<RefMaster>(null)).Returns(_dummyEmailRefs);

            _controller = new UserController(_repo, _config);
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

            Assert.IsEmpty(_controller.IsEmailValid(new ForgotPassword { EMAIL = email }));
        }

        [Test]
        public void EmailExists_ReturnTrue()
        {
            var repo = A.Fake<IRepository>();

            Assert.IsEmpty(_controller.IsEmailExists(_user));
        }

        [TestCase("dum@dumm")]
        [TestCase("du.dummy.com")]
        [TestCase("aa@.com")]
        public void EmailValid_ReturnFalse(string email)
        {
            var repo = A.Fake<IRepository>();

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

            Assert.IsEmpty(_controller.IsEmailExists(_user));
        }
        #endregion
        #region Change password
        [TestCase("change", "userValid", "")]
        [TestCase("create", "userValid", "84286024a3d8f0411be7321ac3e4fe46d89d01fd")]
        public void ChangePassword_Valid(string mode, string username, string token)
        {
            User user = new();
            var repo = A.Fake<IRepository>();
            ChangePassword change = new ChangePassword()
            {
                USERNAME = username,
                PASSWORD = "newDummyPassword",
                MODE = mode,
                REFF = "",
                TOKEN = token
            };

            A.CallTo(() => repo.Update(_user)).DoesNothing();

            Assert.IsInstanceOf<OkObjectResult>(_controller.ChangePassword(change));
        }

        [TestCase("change", "Username not registered", "dummyUserX", "", "")]
        [TestCase("change", "Please provide new password", "userNewPassword", "", "")]
        [TestCase("change", "New password must be different from the old one.", "userOldNewPassSame", "135791", "")]
        [TestCase("create", "Username not registered", "dummyUserX", "", "")]
        [TestCase("create", "Please provide new password", "userNewPassword", "", "")]
        [TestCase("create", "Token expired", "userTokenExpired", "newPassword", "")]
        [TestCase("create", "Invalid token.\nUserToken:{0}, new token: {1}", "userInvalidToken", "newPassword", "84286024a3d8f0411be7321ac3e4fe46d89d01fe")]
        [TestCase("create", "New password must be different from the old one.", "userOldNewPassSame", "135791", "84286024a3d8f0411be7321ac3e4fe46d89d01fd")]
        public void ChangePassword_ReturnFalse(string mode, string expected, string username, string password, string token)
        {
            _chgPass = new()
            {
                USERNAME = username,
                PASSWORD = password,
                MODE = mode,
                TOKEN = token
            };

            A.CallTo(() => _repo.Update(_user)).DoesNothing();

            var res = _controller.ChangePassword(_chgPass) as ObjectResult;

            string result = res.Value.ToString();

            if (result.Split('.')[0] == "Invalid token")
            {
                var userToken = result.Split(':')[1].Substring(0, 40);
                expected = string.Format(expected, userToken, token);
            }

            Assert.AreEqual(result, expected);
        }
        #endregion

        private List<User> GetDummyUser()
        {
            List<User> dummy = new()
            {
                new User
                {
                    USERNAME = "userValid",
                    NAME = "DUMMY DUMMY",
                    PASSWORD = "DUMMY_DUMMY",
                    MOTHER_MAIDEN_NAME = "MaidenName",
                    ADDRESS = "DUMMY ADDRESS",
                    KELURAHAN = "DUMMY LURAH",
                    KECAMATAN = "DUMMY KECAMATAN",
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
                    CHANGE_PASSWORD_TOKEN = "84286024a3d8f0411be7321ac3e4fe46d89d01fd",
                    CHANGE_PASSWORD_TOKEN_EXPIRATION = DateTime.Now.AddMinutes(15).ToString("yyyyMMddHHmm")
                },
                new User
                {
                    USERNAME = "userNewPassword",
                    NAME = "DUMMY DUMMY",
                    PASSWORD = "DUMMY_DUMMY",
                    MOTHER_MAIDEN_NAME = "MaidenName",
                    ADDRESS = "DUMMY ADDRESS",
                    KELURAHAN = "DUMMY LURAH",
                    KECAMATAN = "DUMMY KECAMATAN",
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
                new User
                {
                    USERNAME = "userTokenExpired",
                    NAME = "DUMMY KEDUA KAKA",
                    PASSWORD = "DUMMY_DUMMY",
                    MOTHER_MAIDEN_NAME = "MaidenName",
                    ADDRESS = "DUMMY ADDRESS",
                    KELURAHAN = "DUMMY LURAH",
                    KECAMATAN = "DUMMY KECAMATAN",
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
                    USER_TYPE = "user",
                    CHANGE_PASSWORD_TOKEN = "84286024a3d8f0411be7321ac3e4fe46d89d01fd",
                    CHANGE_PASSWORD_TOKEN_EXPIRATION = "202201170145"
                },
                new User
                {
                    USERNAME = "userInvalidToken",
                    NAME = "DUMMY KETIGA",
                    PASSWORD = "DUMMY_DUMMY",
                    MOTHER_MAIDEN_NAME = "MaidenName",
                    ADDRESS = "DUMMY ADDRESS",
                    KELURAHAN = "DUMMY LURAH",
                    KECAMATAN = "DUMMY KECAMATAN",
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
                    USER_TYPE = "user",
                    CHANGE_PASSWORD_TOKEN = "84286024a3d8f0411be7321ac3e4fe46d89d01fd",
                    CHANGE_PASSWORD_TOKEN_EXPIRATION = DateTime.Now.AddMinutes(15).ToString("yyyyMMddHHmm")
                },
                new User
                {
                    USERNAME = "userOldNewPassSame",
                    NAME = "DUMMY KETIGA",
                    PASSWORD = "35ab943da018a5dcc5e1b30a77ca7cc6dd37ce8c",
                    MOTHER_MAIDEN_NAME = "MaidenName",
                    ADDRESS = "DUMMY ADDRESS",
                    KELURAHAN = "DUMMY LURAH",
                    KECAMATAN = "DUMMY KECAMATAN",
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
                    USER_TYPE = "user",
                    CHANGE_PASSWORD_TOKEN = "84286024a3d8f0411be7321ac3e4fe46d89d01fd",
                    CHANGE_PASSWORD_TOKEN_EXPIRATION = DateTime.Now.AddMinutes(15).ToString("yyyyMMddHHmm")
                },
            };

            return dummy;
        }

        private List<RefMaster> GetDummyEmailRefs()
        {
            List<RefMaster> dummy = new()
            {
                new RefMaster()
                {
                    MASTER_GROUP = "EMAIL",
                    MASTER_CODE = "MAIL_FROM",
                    VALUE = "VALUE kaka"
                },
                new RefMaster()
                {
                    MASTER_GROUP = "EMAIL",
                    MASTER_CODE = "MAIL_SUBJECT_RESET",
                    VALUE = "RESET PASSWORD"
                },
                new RefMaster()
                {
                    MASTER_GROUP = "EMAIL",
                    MASTER_CODE = "MAIL_SIGNATURE",
                    VALUE = "VALUE kaka"
                },
                new RefMaster()
                {
                    MASTER_GROUP = "EMAIL",
                    MASTER_CODE = "MAIL_BODY_TEMPLATE_PATH",
                    VALUE = "VALUE kaka"
                },
                new RefMaster()
                {
                    MASTER_GROUP = "EMAIL",
                    MASTER_CODE = "SMTP_SSL",
                    VALUE = "VALUE kaka"
                },
                new RefMaster()
                {
                    MASTER_GROUP = "EMAIL",
                    MASTER_CODE = "SMTP_DEFAULT_CREDENTIALS",
                    VALUE = "VALUE kaka"
                },
                new RefMaster()
                {
                    MASTER_GROUP = "EMAIL",
                    MASTER_CODE = "MAILKIT_SECURE_POCKET_OPTION",
                    VALUE = "VALUE kaka"
                },
                new RefMaster()
                {
                    MASTER_GROUP = "EMAIL",
                    MASTER_CODE = "SMTP_SERVER",
                    VALUE = "VALUE kaka"
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
                    VALUE = "VALUE kaka"
                },
                new RefMaster()
                {
                    MASTER_GROUP = "EMAIL",
                    MASTER_CODE = "MAIL_LINK",
                    VALUE = "VALUE kaka"
                }
            };

            return dummy;
        }
    }
}
