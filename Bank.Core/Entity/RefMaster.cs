using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Core.Entity
{
    public class RefMaster : BaseEntity
    {
        public string MASTER_GROUP { get; set; } //

        public string MASTER_CODE_DESCRIPTION { get; set; } //

        public string VALUE { get; set; } //

        public string MASTER_CODE { get; set; } //

        public bool IS_ACTIVE { get; set; } //
    }
}

