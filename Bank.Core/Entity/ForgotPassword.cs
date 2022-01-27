using System.Text.RegularExpressions;

namespace Bank.Core.Entity
{
    public class ForgotPasswordModel
    {
        private string _email;
        private bool _isValid = false;

        public string EMAIL
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;

                /// Set valid status
                Regex regex = new(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.CultureInvariant | RegexOptions.Singleline);
                _isValid = regex.IsMatch(_email);
            }
        }

        public bool IS_EMAIL_VALID
        {
            get
            {
                return _isValid;
            }
        }
    }
}
