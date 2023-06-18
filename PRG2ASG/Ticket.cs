using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2ASG
{
    abstract class Ticket
    {
        // properties
        public Screening Screening { get; set; }

        // constructors

        // non-parameterized
        public Ticket() { }

        // parameterized
        public Ticket(Screening screening)
        {
            Screening = screening;
        }

        // methods

        // calculate price of ticket based on pricing scheme
        public abstract double CalculatePrice();

        // basic ToString() method
        public override string ToString()
        {
            string output = string.Format("Screening: {0}", Screening);
            return output;
        }
    }
}
