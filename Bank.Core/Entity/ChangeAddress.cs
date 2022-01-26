using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bank.Core.Entity
{
    public class ChangeAddress
    {
        public string TOKEN { get; set; }
        public string USERNAME { get; set; }
        public string ADDRESS { get; set; }
        public string PROVINCE { get; set; }
        public string KABUPATEN_KOTA { get; set; }
        public string KECAMATAN { get; set; }
        public string KELURAHAN { get; set; }
    }
}
