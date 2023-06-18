using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2ASG
{
    class Order
    {
        // properties
        public int OrderNo { get; set; }
        public DateTime OrderDateTime { get; set; }
        public double Amount { get; set; }
        public string Status { get; set; }
        public List<Ticket> TicketList { get; set; }  // list of tickets ordered (1 or more)
            = new List<Ticket>();

        // constructors

        // non-parameterized
        public Order() { }

        // parameterized
        public Order(int oNo, DateTime oDateTime)
        {
            OrderNo = oNo;
            OrderDateTime = oDateTime;
        }

        // methods

        // add a ticket to the order
        public void AddTicket(Ticket ticket)
        {
            TicketList.Add(ticket);
        }

        // basic ToString() method
        public override string ToString()
        {
            string output = string.Format("Order No.: {0}\tOrder Date Time: {1}\tAmount: {2}\tStatus: {3}\tTicket List: {4}", OrderNo, OrderDateTime, Amount, Status, TicketList);
            return output;
        }
    }
}
