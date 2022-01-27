using System;
using System.Linq;
using System.Text;

namespace Bank.Core.Entity
{
    public class ChangePassword
    {
        private string _password = "";
        private bool _isExpired = true;
        private string _passwordHash = "";

        public ChangePassword(string mode, string username, string password, string token, string reff)
        {
            MODE = mode;
            USERNAME = username;
            PASSWORD = password;
            TOKEN = token;
            REFF = reff;
        }
        public string MODE { get; set; }
        public string USERNAME { get; set; }
        public string PASSWORD
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;

                var sha1 = System.Security.Cryptography.SHA1.Create();
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(_password));

                _passwordHash = string.Concat(hash.Select(b => b.ToString("x2")));
            }
        }
        public string TOKEN { get; set; }
        public string REFF
        {
            set
            {
                int yrs = int.Parse(value.Substring(0, 4));
                int mon = int.Parse(value.Substring(4, 2));
                int day = int.Parse(value.Substring(6, 2));
                int hrs = int.Parse(value.Substring(8, 2));
                int mnt = int.Parse(value.Substring(10, 2));
                DateTime dt = new DateTime(yrs, mon, day, hrs, mnt, 0);
                if (DateTime.Now > dt) _isExpired = true;
                else _isExpired = false;
            }
        }
        public bool IS_TOKEN_EXPIRED
        {
            get
            {
                return _isExpired;
            }
        }
        public string PASSWORD_HASH
        {
            get
            {
                return _passwordHash;
            }
        }
    }
}