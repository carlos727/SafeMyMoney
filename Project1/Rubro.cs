using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace Project1
{
    [Table]
    public class Rubro
    {
        [Column(IsDbGenerated = true, IsPrimaryKey = true, CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int ID { get; set; }
        [Column(CanBeNull = false)]
        public string name { get; set; }
        [Column(CanBeNull = false)]
        public string type { get; set; }
        [Column(CanBeNull = false)]
        public int expected { get; set; }
        [Column(CanBeNull = false)]
        public int current { get; set; }
        [Column(CanBeNull = false)]
        public int cycle { get; set; }
    }
}
