using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Core.Entity
{
    public class UserViewModel
    {
        public string Username { get; set; }
        public bool PinStatus { get; set; }
        public bool IsValidate { get; set; }
        public bool IsActive { get; set; }
    }
}
