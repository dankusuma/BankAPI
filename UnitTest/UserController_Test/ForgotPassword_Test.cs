using Bank.Api.Controllers;
using Bank.Core.Entity;
using Bank.Core.Interface;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.UserController_Test
{
    [TestFixture]
    public class ForgotPassword_Test
    {
        private ForgotPassword _forgotPassword;
        public static IConfiguration _config;
        public static IRepository _repo;
        private UserController controller;

        [SetUp]
        public void SetUp()
        {
            _config = A.Fake<IConfiguration>();
            _repo = A.Fake<IRepository>();

            controller = new UserController(_repo, _config);

            _forgotPassword = new()
            {
                EMAIL = "dummy1@dummy.com"
            };
        }

        [Test]
        public void ForgotPasswordTest()
        {
            A.CallTo(() => _repo.List<User>(null)).Returns(GetDummyUser());
            A.CallTo(() => _repo.List<RefMaster>(null)).Returns(GetDummyRefMaster());
            var result = controller.ForgotPassword(_forgotPassword) as OkObjectResult;
            
            // Assert
            Assert.AreEqual(200, result.StatusCode);
        }

        private List<RefMaster> GetDummyRefMaster()
        {
            List<RefMaster> dummy = new List<RefMaster>()
            {
                new RefMaster {
                    MASTER_GROUP = "EMAIL",
                    MASTER_CODE = "MAIL_FROM", //
                    VALUE = "toshiro89@gmail.com", //
                    MASTER_CODE_DESCRIPTION = "", //
                    IS_ACTIVE = true, //
                },
                new RefMaster {
                    MASTER_GROUP = "EMAIL",
                    MASTER_CODE = "MAIL_FROM_PASSWORD", //
                    VALUE = "uudflhwdhwvvkhfv", //
                    MASTER_CODE_DESCRIPTION = "", //
                    IS_ACTIVE = true, //
                },
                new RefMaster {
                    MASTER_GROUP = "EMAIL",
                    MASTER_CODE = "MAIL_SUBJECT_RESET", //
                    VALUE = "Reset Password", //
                    MASTER_CODE_DESCRIPTION = "", //
                    IS_ACTIVE = true, //
                },
                new RefMaster {
                    MASTER_GROUP = "EMAIL",
                    MASTER_CODE = "MAIL_BODY_TEMPLATE_PATH", //
                    VALUE = "/Views/Templates/ChangePassword.cshtml", //
                    MASTER_CODE_DESCRIPTION = "", //
                    IS_ACTIVE = true, //
                },
                new RefMaster {
                    MASTER_GROUP = "EMAIL",
                    MASTER_CODE = "MAIL_LINK", //
                    VALUE = "https://159.223.84.230/ChangePassword?mode=create&username={0}&token={1}&reff={2}", //
                    MASTER_CODE_DESCRIPTION = "", //
                    IS_ACTIVE = true, //
                },
                new RefMaster {
                    MASTER_GROUP = "EMAIL",
                    MASTER_CODE = "MAIL_SIGNATURE", //
                    VALUE = "Bank App", //
                    MASTER_CODE_DESCRIPTION = "", //
                    IS_ACTIVE = true, //
                },
                new RefMaster {
                    MASTER_GROUP = "EMAIL",
                    MASTER_CODE = "SMTP_SERVER", //
                    VALUE = "smtp.gmail.com", //
                    MASTER_CODE_DESCRIPTION = "", //
                    IS_ACTIVE = true, //
                },
                new RefMaster {
                    MASTER_GROUP = "EMAIL",
                    MASTER_CODE = "SMTP_PORT", //
                    VALUE = "465", //
                    MASTER_CODE_DESCRIPTION = "", //
                    IS_ACTIVE = true, //
                },
                new RefMaster {
                    MASTER_GROUP = "EMAIL",
                    MASTER_CODE = "SMTP_SSL", //
                    MASTER_CODE_DESCRIPTION = "", //
                    VALUE = "TRUE", //
                    IS_ACTIVE = true, //
                },
                new RefMaster {
                    MASTER_GROUP = "EMAIL",
                    MASTER_CODE = "SMTP_DEFAULT_CREDENTIALS", //
                    VALUE = "FALSE", //
                    MASTER_CODE_DESCRIPTION = "", //
                    IS_ACTIVE = true, //
                }
            };
            return dummy;
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
