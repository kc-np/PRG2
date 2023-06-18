using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2ASG
{
    class Cinema
    {
        // properties
        public string Name { get; set; }
        public int HallNo { get; set; }
        public int Capacity { get; set; }

        // constructors

        // non-parameterized
        public Cinema() { }

        // parameterized
        public Cinema(string name, int hallNo, int capacity)
        {
            Name = name;
            HallNo = hallNo;
            Capacity = capacity;
        }

        // methods

        // basic ToString() method
        public override string ToString()
        {
            string output = string.Format("Name: {0}\tHall No.: {1}\tCapacity: {2}", Name, HallNo, Capacity);
            return output;
        }
    }
}
