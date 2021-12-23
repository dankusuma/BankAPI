using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public DateTime BIRTH_DATE { get; set; } //
        public string KECAMATAN { get; set; } //
        public string PROVINCE { get; set; } //
        public string MARITAL_STATUS { get; set; } //
        public string EMAIL { get; set; } //
        public string NIK { get; set; } //

        public string dataValidation()
        {
            var emailValidation = new EmailAddressAttribute();
            if (!USERNAME.All(char.IsLetter) || USERNAME.Length > 20)
            {
                return "Username not valid";
            }
            if (!Regex.IsMatch(NAME, @"^[a-zA-Z ]+$") || NAME.Length > 100)
            {
                return "Name not valid";
            }
            if (ADDRESS.Length > 200)
            {
                return "Address not valid";
            }
            if (!PHONE.All(char.IsDigit) || PHONE.Length > 20)
            {
                return "Phone not valid";
            }
            if (FOTO_KTP_SELFIE.Length > 30)
            {
                return "Foto KTP Selfie not valid";
            }
            if (VIDEO.Length > 30)
            {
                return "Video not valid";
            }
            if (USER_TYPE.Length > 20)
            {
                return "User Type not valid";
            }
            if (!NIK.All(char.IsDigit) || NIK.Length > 20)
            {
                return "NIK not valid";
            }

            if (!Regex.IsMatch(BIRTH_PLACE, @"^[a-zA-Z ]+$") || BIRTH_PLACE.Length > 30)
            {
                return "Birth Place not valid";
            }
            if (!Regex.IsMatch(MOTHER_MAIDEN_NAME, @"^[a-zA-Z ]+$") || MOTHER_MAIDEN_NAME.Length > 100)
            {
                return "Mother Maiden Name not valid";
            }
            if (!Regex.IsMatch(KELURAHAN, @"^[a-zA-Z ]+$") || KELURAHAN.Length > 50)
            {
                return "Kelurahan not valid";
            }
            if (!Regex.IsMatch(KABUPATEN_KOTA, @"^[a-zA-Z ]+$") || KABUPATEN_KOTA.Length > 50)
            {
                return "Kabupaten Kota not valid";
            }
            if (!Regex.IsMatch(JOB, @"^[a-zA-Z ]+$") || JOB.Length > 50)
            {
                return "Job not valid";
            }
            if (!Regex.IsMatch(KECAMATAN, @"^[a-zA-Z ]+$") || KECAMATAN.Length > 50)
            {
                return "Kecamatan not valid";
            }
            if (!Regex.IsMatch(PROVINCE, @"^[a-zA-Z ]+$") || PROVINCE.Length > 50)
            {
                return "Province not valid";
            }
            if (!Regex.IsMatch(MARITAL_STATUS, @"^[a-zA-Z ]+$") || MARITAL_STATUS.Length > 50)
            {
                return "Marital Status not valid";
            }
            if (!emailValidation.IsValid(EMAIL) || EMAIL.Length > 100)
            {
                return "Email not valid";
            }
            return "Success";
        }

        public string HashValue(string text)
        {
            var sha1 = System.Security.Cryptography.SHA1.Create();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(text));
            return string.Concat(hash.Select(b => b.ToString("x2")));
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
