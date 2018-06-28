using System;
using System.Collections.Generic;
using System.Text;

namespace BugGroupByDatePart.Classes.Models
{
    public class ConsumptionPerMonth //: IComparable => needed when order done in memory
    {
        public class Author
        {
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public string Login { get; set; }
            public Boolean IsActive { get; set; }
        }
        public int Year { get; set; }
        public int Month { get; set; }
        public Author User { get; set; }
        public decimal Amount { get; set; }

        //public int CompareTo(object obj)
        //{
        //    throw new NotImplementedException("sorry.");
        //}
    }
}
