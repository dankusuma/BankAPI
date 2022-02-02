using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bank.Core.Entity.ChangeData
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

        public bool IsAddressValid()
        {
            if (string.IsNullOrEmpty(ADDRESS) || ADDRESS.Length >= 200)
            {
                throw new ArgumentException("Alamat is too long");
            }
            if (string.IsNullOrEmpty(KELURAHAN) || !Regex.IsMatch(KELURAHAN, @"^[a-zA-Z ]+$") || KELURAHAN.Length > 50)
            {
                throw new ArgumentException("Kelurahan is not valid!");
            }
            if (string.IsNullOrEmpty(KABUPATEN_KOTA) || !Regex.IsMatch(KABUPATEN_KOTA, @"^[a-zA-Z ]+$") || KABUPATEN_KOTA.Length > 50)
            {
                throw new ArgumentException("Kabupaten is not valid!");
            }
            if (string.IsNullOrEmpty(KECAMATAN) || !Regex.IsMatch(KECAMATAN, @"^[a-zA-Z ]+$") || KECAMATAN.Length > 50)
            {
                throw new ArgumentException("Kecamatan is not valid!");
            }
            if (string.IsNullOrEmpty(PROVINCE) || !Regex.IsMatch(PROVINCE, @"^[a-zA-Z ]+$") || PROVINCE.Length > 50)
            {
                throw new ArgumentException("Province is not valid!");
            }

            return true;
        }
    }
}
