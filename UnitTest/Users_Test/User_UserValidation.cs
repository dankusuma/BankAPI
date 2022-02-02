using Bank.Core.Entity;
using NUnit.Framework;
using System;

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
            {
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
                EMAIL = "dummy@dummy.com",
                GENDER = 'M',
                JOB = "PNS",
                PHONE = "08123456789",
                NIK = "1234567891012131",
                MARITAL_STATUS = "Lajang",
                FOTO_KTP_SELFIE = "DUMMY KTP LINK",
                VIDEO = "DUMMY VIDEO LINK",
                USER_TYPE = "user",
            };
        }

        [Test]
        public void Validate_Success()
        {
            Assert.AreEqual("Success", _user.dataValidation());
        }

        [TestCase("", "dummyName", "dummyPassword", "dummyMaiden", "dummyAddress", "dummyKelurahan", "dummyKecamatan", "dummyKabupaten", "dummyProvince", "dummyKota", 'M', "lajang", "PNS", "user", "dummy@dummy.com", "08123456789", "dummyUsername", "dummy_ktp_link", "dummy_video_link")]
        [TestCase("1234567891012131", "", "dummyPassword", "dummyMaiden", "dummyAddress", "dummyKelurahan", "dummyKecamatan", "dummyKabupaten", "dummyProvince", "dummyKota", 'M', "lajang", "PNS", "user", "dummy@dummy.com", "08123456789", "dummyUsername", "dummy_ktp_link", "dummy_video_link")]
        [TestCase("1234567891012131", "dummyName", "", "dummyMaiden", "dummyAddress", "dummyKelurahan", "dummyKecamatan", "dummyKabupaten", "dummyProvince", "dummyKota", 'M', "lajang", "PNS", "user", "dummy@dummy.com", "08123456789", "dummyUsername", "dummy_ktp_link", "dummy_video_link")]
        [TestCase("1234567891012131", "dummyName", "dummyPassword", "", "dummyAddress", "dummyKelurahan", "dummyKecamatan", "dummyKabupaten", "dummyProvince", "dummyKota", 'M', "lajang", "PNS", "user", "dummy@dummy.com", "08123456789", "dummyUsername", "dummy_ktp_link", "dummy_video_link")]
        [TestCase("1234567891012131", "dummyName", "dummyPassword", "dummyMaiden", "dummyAddress", "", "dummyKecamatan", "dummyKabupaten", "dummyProvince", "dummyKota", 'M', "lajang", "PNS", "user", "dummy@dummy.com", "08123456789", "dummyUsername", "dummy_ktp_link", "dummy_video_link")]
        [TestCase("1234567891012131", "dummyName", "dummyPassword", "dummyMaiden", "dummyAddress", "dummyKelurahan", "", "dummyKabupaten", "dummyProvince", "dummyKota", 'M', "lajang", "PNS", "user", "dummy@dummy.com", "08123456789", "dummyUsername", "dummy_ktp_link", "dummy_video_link")]
        [TestCase("1234567891012131", "dummyName", "dummyPassword", "dummyMaiden", "dummyAddress", "dummyKelurahan", "dummyKecamatan", "", "dummyProvince", "dummyKota", 'M', "lajang", "PNS", "user", "dummy@dummy.com", "08123456789", "dummyUsername", "dummy_ktp_link", "dummy_video_link")]
        [TestCase("1234567891012131", "dummyName", "dummyPassword", "dummyMaiden", "dummyAddress", "dummyKelurahan", "dummyKecamatan", "dummyKabupaten", "", "dummyKota", 'M', "lajang", "PNS", "user", "dummy@dummy.com", "08123456789", "dummyUsername", "dummy_ktp_link", "dummy_video_link")]
        [TestCase("1234567891012131", "dummyName", "dummyPassword", "dummyMaiden", "dummyAddress", "dummyKelurahan", "dummyKecamatan", "dummyKabupaten", "dummyProvince", "", 'M', "lajang", "PNS", "user", "dummy@dummy.com", "08123456789", "dummyUsername", "dummy_ktp_link", "dummy_video_link")]
        [TestCase("1234567891012131", "dummyName", "dummyPassword", "dummyMaiden", "dummyAddress", "dummyKelurahan", "dummyKecamatan", "dummyKabupaten", "dummyProvince", "dummyKota", 'M', "", "PNS", "user", "dummy@dummy.com", "08123456789", "dummyUsername", "dummy_ktp_link", "dummy_video_link")]
        [TestCase("1234567891012131", "dummyName", "dummyPassword", "dummyMaiden", "dummyAddress", "dummyKelurahan", "dummyKecamatan", "dummyKabupaten", "dummyProvince", "dummyKota", 'M', "lajang", "", "user", "dummy@dummy.com", "08123456789", "dummyUsername", "dummy_ktp_link", "dummy_video_link")]
        [TestCase("1234567891012131", "dummyName", "dummyPassword", "dummyMaiden", "dummyAddress", "dummyKelurahan", "dummyKecamatan", "dummyKabupaten", "dummyProvince", "dummyKota", 'M', "lajang", "PNS", "user", "dummymy.com", "08123456789", "dummyUsername", "dummy_ktp_link", "dummy_video_link")]
        [TestCase("1234567891012131", "dummyName", "dummyPassword", "dummyMaiden", "dummyAddress", "dummyKelurahan", "dummyKecamatan", "dummyKabupaten", "dummyProvince", "dummyKota", 'M', "lajang", "PNS", "user", "dummy@dummy.com", "", "dummyUsername", "dummy_ktp_link", "dummy_video_link")]
        [TestCase("1234567891012131", "dummyName", "dummyPassword", "dummyMaiden", "dummyAddress", "dummyKelurahan", "dummyKecamatan", "dummyKabupaten", "dummyProvince", "dummyKota", 'M', "lajang", "PNS", "user", "dummy@dummy.com", "08123456789", "", "dummy_ktp_link", "dummy_video_link")]
        [TestCase("1234567891012131", "dummyName", "dummyPassword", "dummyMaiden", "dummyAddress", "dummyKelurahan", "dummyKecamatan", "dummyKabupaten", "dummyProvince", "dummyKota", 'M', "lajang", "PNS", "user", "dummy@dummy.com", "08123456789", "dummyUsername", "", "dummy_video_link")]
        [TestCase("1234567891012131", "dummyName", "dummyPassword", "dummyMaiden", "dummyAddress", "dummyKelurahan", "dummyKecamatan", "dummyKabupaten", "dummyProvince", "dummyKota", 'M', "lajang", "PNS", "user", "dummy@dummy.com", "08123456789", "dummyUsername", "dummy_ktp_link", "")]
        [TestCase("1234567891012131", "dummyName", "dummyPassword", "dummyMaiden", "addressnya harus lebih dari 200 baris, jadi ini saya ngarang isinya biar bisa dua ratus isinya.... kurang seratus nih, enaknya diisi apa ya? sekarang kurang enam puluhan... sudah kurang dua puluh lima lagi dan sekarang sudah cukup!", "dummyKelurahan", "dummyKecamatan", "dummyKabupaten", "dummyProvince", "dummyKota", 'M', "lajang", "PNS", "user", "dummy@dummy.com", "08123456789", "dummyUsername", "dummy_ktp_link", "dummy_video_link")]
        [TestCase("1234567891012131", "dummyName", "dummyPassword", "dummyMaiden", "", "dummyKelurahan", "dummyKecamatan", "dummyKabupaten", "dummyProvince", "dummyKota", 'M', "lajang", "PNS", "user", "dummy@dummy.com", "08123456789", "dummyUsername", "dummy_ktp_link", "dummy_video_link")]
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

        [TestCase("dummy@dummy.dummy")]
        [TestCase("saya.punya@email.disini")]
        [TestCase("saiful.setiawan@mitrais.com")]
        public void Validate_EmailValidator_ReturnTrue(string email)
        {
            Assert.IsTrue(_user.IsEmailValid(email));
        }

        [TestCase("dummy.dummyummy")]
        [TestCase("ini.email_saya")]
        [TestCase("saiful_cvsetiawan")]
        public void Validate_EmailValidator_ReturnFalse(string email)
        {
            Assert.IsFalse(_user.IsEmailValid(email));
        }
    }
}
