using System;
using System.Collections.Generic;
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

    }
}
