using System;
using System.Collections.Generic;

namespace DemoWebAPI.Models
{
    public partial class Bank
    {
        public int BankId { get; set; }
        public int EmpId { get; set; }
        public string BankName { get; set; }

        public Employee Emp { get; set; }
    }
}
