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
    public class Pin_Test
    {
        private Pin _pin;
        private User _user;
        public static IConfiguration _config;
        public static IRepository _repo;
        private UserController controller;

        [SetUp]
        public void SetUp()
        {
            _config = A.Fake<IConfiguration>();
            _repo = A.Fake<IRepository>();

            controller = new UserController(_repo, _config);

            /// Set User
            _user = new()
            {
                USERNAME = "dummyUser",
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
                BIRTH_DATE = new DateTime(2001, 03, 13), // 2001-03-13 00:00:00.000
                PIN = "309d64ed5d085f26c1998e4827d41c363b5a4e27"
            };

            /// Set Pin
            _pin = new()
            {
                TOKEN = "",
                USERNAME = "dummyUser",
                PIN = "890308",
                NEW_PIN = "",
                user = new User(),
                mode = "create",
            };
        }

        [TestCase("", "080389")]
        [TestCase("Invalid pin", null)]
        [TestCase("Invalid input. Only accept 6 digit numbers", "")]
        [TestCase("Only accept 6 digit numbers", "12345y")]
        [TestCase("Pin too short. Only accept 6 digit numbers", "123")]
        [TestCase("Pin too long. Only accept 6 digit numbers", "1234567")]
        [TestCase("Please do not use your Date of Birth (DOB)", "130301")]
        [TestCase("Please do not use your Date of Birth (DOB)", "010313")]
        [TestCase("Please DO NOT use \"123456\" or \"654321\" and the other general number as your pin", "123456")]
        [TestCase("Please DO NOT use \"123456\" or \"654321\" and the other general number as your pin", "654321")]
        public void CreatePINValidation(string result, string PIN)
        {
            /// Set Pin
            var pin = A.Fake<Pin>();
            pin.mode = "create";
            pin.PIN = PIN;
            pin.user = _user;

            var res = pin.PinValidation();

            Assert.AreEqual(result, res);
        }


        [TestCase("", "080389")]
        [TestCase("Invalid pin", null)]
        [TestCase("Invalid input. Only accept 6 digit numbers", "")]
        [TestCase("Only accept 6 digit numbers", "12345y")]
        [TestCase("Pin too short. Only accept 6 digit numbers", "123")]
        [TestCase("Pin too long. Only accept 6 digit numbers", "1234567")]
        [TestCase("Please do not use your Date of Birth (DOB)", "130301")]
        [TestCase("Please do not use your Date of Birth (DOB)", "010313")]
        [TestCase("Please DO NOT use \"123456\" or \"654321\" and the other general number as your pin", "123456")]
        [TestCase("Please DO NOT use \"123456\" or \"654321\" and the other general number as your pin", "654321")]
        [TestCase("New pin must be different from the old one.", "890308")]
        public void ChangePINValidation(string result, string NEW_PIN)
        {
            /// Set Pin
            var pin = A.Fake<Pin>();
            pin.mode = "change";
            pin.NEW_PIN = NEW_PIN;
            pin.user = _user;

            var res = pin.PinValidation();

            Assert.AreEqual(result, res);
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
                    USER_TYPE = "user",
                    BIRTH_DATE = new DateTime(2000, 03, 13) // 2000-03-13 00:00:00.000
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
