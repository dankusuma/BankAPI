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

        public string PinValidation()
        {
            string yrs = user.BIRTH_DATE.Year.ToString().Substring(2, 2);
            string mon = user.BIRTH_DATE.Month.ToString();
            string day = user.BIRTH_DATE.Day.ToString();

            string dob1 = day + mon + yrs;
            string dob2 = yrs + mon + day;


            if (mode == "create")
            {
                if (PIN == null) return "Invalid pin";
                if (PIN == "") return "Invalid input. Only accept 6 digit numbers";
                if (!PIN.All(char.IsNumber)) return "Only accept 6 digit numbers";
                if (PIN.Length < 6) return "Pin too short. Only accept 6 digit numbers";
                if (PIN.Length > 6) return "Pin too long. Only accept 6 digit numbers";
                if (PIN == dob1 || PIN == dob2) return "Please do not use your Date of Birth (DOB)";
                if (PIN is "123456" or "654321") return "Please DO NOT use \"123456\" or \"654321\" and the other general number as your pin";
            }

            if (mode == "change")
            {
                if (NEW_PIN == null) return "Invalid pin";
                if (NEW_PIN == "") return "Invalid input. Only accept 6 digit numbers";
                if (!NEW_PIN.All(char.IsNumber)) return "Only accept 6 digit numbers";
                if (NEW_PIN.Length < 6) return "Pin too short. Only accept 6 digit numbers";
                if (NEW_PIN.Length > 6) return "Pin too long. Only accept 6 digit numbers";
                if (NEW_PIN == dob1 || NEW_PIN == dob2) return "Please do not use your Date of Birth (DOB)";
                if (NEW_PIN is "123456" or "654321") return "Please DO NOT use \"123456\" or \"654321\" and the other general number as your pin";
                if (NEW_PIN == PIN) return "New pin must be different from the old one.";
            }

            return "";
        }

        public string HashPIN(string text)
        {
            var sha1 = System.Security.Cryptography.SHA1.Create();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(text));

            return string.Concat(hash.Select(b => b.ToString("x2")));
        }
    }
}
