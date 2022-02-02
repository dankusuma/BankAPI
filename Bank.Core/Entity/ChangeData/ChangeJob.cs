using System;
using System.Text.RegularExpressions;

namespace Bank.Core.Entity.ChangeData
{
    public class ChangeJob
    {
        public string USERNAME { get; set; }
        public string JOB { get; set; }


        public bool IsJobValid()
        {
            if (string.IsNullOrEmpty(JOB) || !Regex.IsMatch(JOB, @"^[a-zA-Z ]+$") || JOB.Length > 50)
            {
                throw new ArgumentException("Job is not valid!");
            }

            return true;
        }
    }
}