using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Core.Model
{
    public class User : BaseEntity
    {
        public int role_id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string phone { get; set; }

        public void HashPassword()
        {

            var sha1 = System.Security.Cryptography.SHA1.Create();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(password));
            password = string.Concat(hash.Select(b => b.ToString("x2")));

        }

        public bool VerifyPassword(string _password)
        {
            bool result = false;
            var sha1 = System.Security.Cryptography.SHA1.Create();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(_password));
            _password = string.Concat(hash.Select(b => b.ToString("x2")));
            if (_password == password)
            {
                result = true;
            }

            return result;

        }

    }
}
