using Bank.Core.Entity;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Users_Test
{
    [TestFixture]
    public class User_UserValidation
    {
        private User _user;

        [SetUp]
        public void SetUp()
        {
            _user = new User()
        }

        [Test]
        public void Validate_Success()
        {
            Assert.AreEqual("Success", _user.dataValidation());
        }

        public void Validate_EmptyParameter(
            string nik, string name, string password, string maiden, 
            string adress, string kel, string kec, string kab, string prov,
            string birthPlace, char gender, string marital, string job, string userType,
            string email, string phone, string username,
            string ktp_link, string vid_link)
        {
            _user = new User
            {
                NIK = nik, NAME = name, PASSWORD = password, MOTHER_MAIDEN_NAME = maiden,
                EMAIL = email, PHONE = phone, USERNAME = username,
                ADDRESS = adress, KELURAHAN = kel, KABUPATEN_KOTA = kab, PROVINCE = prov, KECAMATAN = kec,
                BIRTH_DATE = new DateTime(1992, 12, 12), BIRTH_PLACE = birthPlace, GENDER = gender, MARITAL_STATUS = marital, JOB = job, USER_TYPE = userType,
                FOTO_KTP_SELFIE = ktp_link, VIDEO = vid_link,
            };

            Assert.Throws<ArgumentException>(() => _user.dataValidation());
        }
    }
}
