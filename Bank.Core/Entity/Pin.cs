using System.Linq;
using System.Text;

namespace Bank.Core.Entity
{
    public class Pin
    {
        private string _pin;
        private string _pinHash = "";
        private int _length = 0;
        private bool _isPinCreated = false;

        public string USERNAME { get; set; }
        public string PIN
        {
            get
            {
                return _pin;
            }
            set
            {
                _length = value == null ? 0 : value.Length;
                _pin = value;

                if (_pin != null)
                {
                    var sha1 = System.Security.Cryptography.SHA1.Create();
                    var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(_pin));

                    _pinHash = string.Concat(hash.Select(b => b.ToString("x2")));
                }

                if (value != null || value != "") _isPinCreated = true;
                else _isPinCreated = false;
            }
        }
        public string PIN_HASH
        {
            get
            {
                return _pinHash;
            }
        }
        public int LENGTH
        {
            get
            {
                return _length;
            }
        }
        public bool IS_PIN_CREATED

        {
            get
            {
                return _isPinCreated;
            }
        }
    }
}