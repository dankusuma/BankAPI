using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Core.Entity
{
    public class User : BaseEntity
    {
        public string USERNAME { get; set; } //
        public string PASSWORD { get; set; } //
        public string PIN { get; set; }
        public string NAME { get; set; } //
        public string ADDRESS { get; set; } //
        public string PHONE { get; set; } //
        public string FOTO_KTP_SELFIE { get; set; }
        public string VIDEO { get; set; }
        public bool IS_VALIDATE { get; set; }
        public DateTime LOGIN_HOLD { get; set; }
        public int LOGIN_FAILED { get; set; }
        public string USER_TYPE { get; set; }
        public string BIRTH_PLACE { get; set; } //
        public string MOTHER_MAIDEN_NAME { get; set; } //
        public string KELURAHAN { get; set; } //
        public string KABUPATEN_KOTA { get; set; } //
        public char GENDER { get; set; } //
        public string JOB { get; set; } //
        public string BIRTH_DATE { get; set; } //
        public string KECAMATAN { get; set; } //
        public string PROVINCE { get; set; } //
        public string MARITAL_STATUS { get; set; } //
        public string EMAIL { get; set; } //
        public string NIK { get; set; } //

        public string dataValidation()
        {
            string failed = "error400";
            var emailValidation = new EmailAddressAttribute();
            if (!USERNAME.All(char.IsLetter) && USERNAME.Length > 20)
            {
                return failed;
            }
            if (!NAME.All(char.IsLetter) && NAME.Length > 100)
            {
                return failed;
            }
            if (ADDRESS.Length > 200)
            {
                return failed;
            }
            if (!PHONE.All(char.IsDigit) && PHONE.Length > 20)
            {
                return failed;
            }
            if (FOTO_KTP_SELFIE.Length > 30)
            {
                return failed;
            }
            if (VIDEO.Length > 30)
            {
                return failed;
            }
            if (USER_TYPE.Length > 20)
            {
                return failed;
            }
            if (!NIK.All(char.IsDigit) && NIK.Length > 20)
            {
                return failed;
            }
            if (!BIRTH_PLACE.All(char.IsLetter) && BIRTH_PLACE.Length > 30)
            {
                return failed;
            }
            if (!MOTHER_MAIDEN_NAME.All(char.IsLetter) && MOTHER_MAIDEN_NAME.Length > 100)
            {
                return failed;
            }
            if (!KELURAHAN.All(char.IsLetter) && KELURAHAN.Length > 50)
            {
                return failed;
            }
            if (!KABUPATEN_KOTA.All(char.IsLetter) && KABUPATEN_KOTA.Length > 50)
            {
                return failed;
            }
            if (!JOB.All(char.IsLetter) && JOB.Length > 50)
            {
                return failed;
            }
            if (!KECAMATAN.All(char.IsLetter) && KECAMATAN.Length > 50)
            {
                return failed;
            }
            if (!PROVINCE.All(char.IsLetter) && PROVINCE.Length > 50)
            {
                return failed;
            }
            if (!MARITAL_STATUS.All(char.IsLetter) && MARITAL_STATUS.Length > 50)
            {
                return failed;
            }
            if (!emailValidation.IsValid(EMAIL) && EMAIL.Length > 100)
            {
                return failed;
            }
            return "Success";
        }
        public void HashPassword()
        {
            var sha1 = System.Security.Cryptography.SHA1.Create();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(PASSWORD));
            PASSWORD = string.Concat(hash.Select(b => b.ToString("x2")));

        }

        public bool VerifyPassword(string _password)
        {
            bool result = false;
            var sha1 = System.Security.Cryptography.SHA1.Create();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(_password));
            _password = string.Concat(hash.Select(b => b.ToString("x2")));
            if (_password == PASSWORD)
            {
                result = true;
            }

            return result;

        }

        public void HashPin()
        {
            var sha1 = System.Security.Cryptography.SHA1.Create();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(PIN));
            PIN = string.Concat(hash.Select(b => b.ToString("x2")));

        }

        public bool VerifyPin(string _pin)
        {
            bool result = false;
            var sha1 = System.Security.Cryptography.SHA1.Create();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(_pin));
            _pin = string.Concat(hash.Select(b => b.ToString("x2")));
            if (_pin == PASSWORD)
            {
                result = true;
            }

            return result;

        }

    }
}
