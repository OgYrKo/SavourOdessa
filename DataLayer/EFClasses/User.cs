using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EFClasses
{
    public partial class User
    {
        public uint? Usesysid { get; set; }
        public string Usename { get; set; }
        public string Rolname { get; set; }
    }
}
