using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataLayer.EFClasses
{
    public class Schedule
    {
        [Column("start_time")]
        public TimeSpan? Start { get; set; }
        [Column("end_time")]
        public TimeSpan? End { get; set; }
        [NotMapped]
        public DateTime CurrentDate { get; set; }

        public void Deconstruct(out DateTime? start, out DateTime? end)
        {
            start = Start==null?null:CurrentDate.Add((TimeSpan)Start);
            end = End == null?null : CurrentDate.Add((TimeSpan)End);
        }
    }
}
