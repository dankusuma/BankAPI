using Bank.Api.Controllers;
using Bank.Core.Entity;
using Bank.Core.Interface;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTest.UserController_Test
{
    public class Add_Test
    {

        [Test]
        public void AddUser_Success()
        {
            var config = A.Fake<IConfiguration>();
            var repo = A.Fake<IRepository>();
            var user = new User
            {
                USERNAME = "tambahanDummy",
                NAME = "DUMMY DUMMY",
                PASSWORD = "DUMMY_DUMMY",
                MOTHER_MAIDEN_NAME = "MaidenName",
                ADDRESS = "DUMMY ADDRESS",
                KELURAHAN = "DUMMY LURAH",
                KECAMATAN = "DUMMY KECAMATAN",
                KABUPATEN_KOTA = "DUMMY KOTA",
                PROVINCE = "DUMMY PROVINCE",
                BIRTH_PLACE = "DUMMY CITY",
                EMAIL = "dummy13@dummy.com",
                GENDER = 'M',
                JOB = "PNS",
                PHONE = "081234567810",
                NIK = "1234567891012132",
                MARITAL_STATUS = "Lajang",
                FOTO_KTP_SELFIE = "DUMMY KTP LINK",
                VIDEO = "DUMMY VIDEO LINK",
                USER_TYPE = "user"
            };

            A.CallTo(() => repo.Add(A<User>.Ignored));
            A.CallTo(() => repo.List<User>(null)).Returns(GetDummyUser());

            var controller = new UserController(repo, config);
            var result = controller.Add(user);

            Assert.IsInstanceOf<OkObjectResult>(result);
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
