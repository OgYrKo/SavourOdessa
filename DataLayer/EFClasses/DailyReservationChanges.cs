using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EFClasses
{
    public class DailyReservationChanges
    {
        public DateOnly Day { get; set; }
        public int NumberOfReservations { get; set; }
        public double ChangePercentage { get; set; }
    }
}
