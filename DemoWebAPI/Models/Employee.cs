using System;
using System.Collections.Generic;

namespace DemoWebAPI.Models
{
    public partial class Employee
    {
        public Employee()
        {
            Bank = new HashSet<Bank>();
        }
        
        public int EmpId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }

       public ICollection<Bank> Bank { get; set; }
    }
    public partial class EmployeeModel {
        public int EmpId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
    }
}
