using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2ASG
{
    class Adult: Ticket
    {
        // properties
        public bool PopcornOffer { get; set; }

        // constructors

        // non-parameterized
        public Adult() : base() { }

        // parameterized
        public Adult(Screening screening, bool popcornOffer) : base(screening)
        {
            PopcornOffer = popcornOffer;
        }

        // methods

        // calculate price of Adult ticket based on pricing scheme
        public override double CalculatePrice()
        {
            double price;
            DateTime sDateTime = Screening.ScreeningDateTime;
            string sType = Screening.ScreeningType;
            int day = (int)sDateTime.DayOfWeek;  // get day of week of screening date time
            if (day != 0 && day <= 4)
            {
                if (sType == "3D")
                {
                    price = 11.00;
                }
                else
                {
                    price = 8.50;
                }
            }
            else
            {
                if (sType == "3D")
                {
                    price = 14.00;
                }
                else
                {
                    price = 12.50;
                }
            }
            if (PopcornOffer)  // add $3.00 if customer wants to buy the popcorn set
            {
                price += 3.00;
            }

            return price;
        }

        // basic ToString() method
        public override string ToString()
        {
            string output = string.Format("{0}\tPopcorn Offer: {1}", base.ToString(), PopcornOffer);
            return output;
        }
    }
}
