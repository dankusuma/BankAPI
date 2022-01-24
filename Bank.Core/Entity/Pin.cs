using System.Linq;
using System.Text;

namespace Bank.Core.Entity
{
    public class Pin
    {
        public string TOKEN { get; set; }

        public string USERNAME { get; set; }

        public string PIN { get; set; } //

        public string NEW_PIN { get; set; } //

        public User user { get; set; }

        public string mode { get; set; }

        public string HashPIN(string text)
        {
            var sha1 = System.Security.Cryptography.SHA1.Create();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(text));

            var res = string.Concat(hash.Select(b => b.ToString("x2")));
            return res;
        }
    }
}
