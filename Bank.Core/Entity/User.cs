using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
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
        public string CHANGE_PASSWORD_TOKEN { get; set; }
        public string CHANGE_PASSWORD_TOKEN_EXPIRATION { get; set; }


        public string dataValidation()
        {
            var emailValidation = new EmailAddressAttribute();
            if (string.IsNullOrEmpty(USERNAME) || !USERNAME.All(char.IsLetter) || USERNAME.Length > 20)
            {
                throw new ArgumentException("User name is not valid!");
            }
            if (PASSWORD.Length < 8 || PASSWORD.Length > 50)
            {
                throw new ArgumentException("password is not valid!");
            }
            if (string.IsNullOrEmpty(NAME) || !Regex.IsMatch(NAME, @"^[a-zA-Z0-9 ]+$") || NAME.Length > 100)
            {
                throw new ArgumentException("Name is not valid! Name must under 100 characters");
            }
            if (string.IsNullOrEmpty(ADDRESS) || ADDRESS.Length > 200)
            {
                throw new ArgumentException("Address must under 200 characters");
            }
            if (string.IsNullOrEmpty(PHONE) || !PHONE.All(char.IsDigit) || PHONE.Length > 20)
            {
                throw new ArgumentException("Phone number must contain number only and under 20 digits");
            }
            if (string.IsNullOrEmpty(FOTO_KTP_SELFIE) || FOTO_KTP_SELFIE.Length > 125)
            {
                throw new ArgumentException("KTP link is not valid!");
            }
            if (string.IsNullOrEmpty(VIDEO) ||  VIDEO.Length > 125)
            {
                throw new ArgumentException("VIDEO link is not valid!");
            }
            if (string.IsNullOrEmpty(USER_TYPE) || USER_TYPE.Length > 20)
            { 
                throw new ArgumentException("user role is not valid!");
            }
            if (!NIK.All(char.IsDigit) || NIK.Length != 16)
            {
                throw new ArgumentException("NIK must 16 characters");
            }
            if (string.IsNullOrEmpty(BIRTH_PLACE) || !Regex.IsMatch(BIRTH_PLACE, @"^[a-zA-Z ]+$") || BIRTH_PLACE.Length > 30)
            {
                throw new ArgumentException("Birthplace is not valid!");
            }
            if (string.IsNullOrEmpty(MOTHER_MAIDEN_NAME) || !Regex.IsMatch(MOTHER_MAIDEN_NAME, @"^[a-zA-Z ]+$") || MOTHER_MAIDEN_NAME.Length > 100)
            {
                throw new ArgumentException("Mother name is not valid!");
            }
            if (string.IsNullOrEmpty(KELURAHAN) || !Regex.IsMatch(KELURAHAN, @"^[a-zA-Z ]+$") || KELURAHAN.Length > 50)
            {
                throw new ArgumentException("Kelurahan is not valid!");
            }
            if (string.IsNullOrEmpty(KABUPATEN_KOTA) || !Regex.IsMatch(KABUPATEN_KOTA, @"^[a-zA-Z ]+$") || KABUPATEN_KOTA.Length > 50)
            {
                throw new ArgumentException("Kabupaten is not valid!");
            }
            if (string.IsNullOrEmpty(JOB) || !Regex.IsMatch(JOB, @"^[a-zA-Z ]+$") || JOB.Length > 50)
            {
                throw new ArgumentException("Job is not valid!");
            }
            if (string.IsNullOrEmpty(KECAMATAN) || !Regex.IsMatch(KECAMATAN, @"^[a-zA-Z ]+$") || KECAMATAN.Length > 50)
            {
                throw new ArgumentException("Kecamatan is not valid!");
            }
            if (string.IsNullOrEmpty(PROVINCE) || !Regex.IsMatch(PROVINCE, @"^[a-zA-Z ]+$") || PROVINCE.Length > 50)
            {
                throw new ArgumentException("Province is not valid!");
            }
            if (string.IsNullOrEmpty(MARITAL_STATUS) || !Regex.IsMatch(MARITAL_STATUS, @"^[a-zA-Z ]+$") || MARITAL_STATUS.Length > 50)
            {
                throw new ArgumentException("Marital status is not valid!");
            }
            if (string.IsNullOrEmpty(EMAIL) || !IsEmailValid(EMAIL) || EMAIL.Length > 100)
            {
                throw new ArgumentException("Email is not valid!");
            }

            return "Success";
        }

        public string HashValue(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException("It should not be null!");
            }

            var sha1 = System.Security.Cryptography.SHA1.Create();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(text));
            return string.Concat(hash.Select(b => b.ToString("x2")));
        }

        public void HashPassword()
        {
            if (string.IsNullOrEmpty(PASSWORD))
            {
                throw new ArgumentNullException("It should not be null!");
            }

            var sha1 = System.Security.Cryptography.SHA1.Create();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(PASSWORD));
            PASSWORD = string.Concat(hash.Select(b => b.ToString("x2")));

        }

        public bool VerifyPassword(string _password)
        {
            var sha1 = System.Security.Cryptography.SHA1.Create();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(_password));
            string hashed = string.Concat(hash.Select(b => b.ToString("x2")));

            return hashed == PASSWORD;
        }

        public void HashPin()
        {
            if (string.IsNullOrEmpty(PIN))
            {
                throw new ArgumentNullException("It should not be null!");
            }

            var sha1 = System.Security.Cryptography.SHA1.Create();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(PIN));
            PIN = string.Concat(hash.Select(b => b.ToString("x2")));

            Console.WriteLine(PIN);
        }

        public bool VerifyPin(string _pin)
        {
            var sha1 = System.Security.Cryptography.SHA1.Create();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(_pin));
            string hashed = string.Concat(hash.Select(b => b.ToString("x2")));

            return hashed == PIN;
        }

        public bool IsEmailValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
