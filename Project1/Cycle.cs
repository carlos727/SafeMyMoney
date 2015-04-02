using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace Project1
{
    [Table]
    public class Cycle
    {
        [Column(IsDbGenerated = true, IsPrimaryKey = true, CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int ID { get; set; }
        [Column(CanBeNull = false)]
        public int percentage { get; set; }
    }
}
