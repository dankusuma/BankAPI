using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Core.Entity
{
    public class ChangePassword
    {
        public string TOKEN { get; set; }

        public string USERNAME { get; set; }

        public string PASSWORD { get; set; }

        public string NEW_PASSWORD { get; set; }

        public string MODE { get; set; }

        public string REFF { get; set; }
    }
}
