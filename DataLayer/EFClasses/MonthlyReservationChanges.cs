using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EFClasses
{
    public class MonthlyReservationChanges
    {
        public int Month { get; set; }
        public int NumberOfReservations { get; set; }
        public double ChangePercentage { get; set; }
    }
}
