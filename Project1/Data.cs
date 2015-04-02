using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;

namespace Project1
{
    public class Data : DataContext
        {
            public Data(String connectionString)
                : base(connectionString)
            {

            }
            public Table<Cycle> Cycle
            {
                get
                {
                    return this.GetTable<Cycle>();
                }
            }
            public Table<Rubro> Rubro
            {
                get
                {
                    return this.GetTable<Rubro>();
                }
            }
            public Table<User> User
            {
                get
                {
                    return this.GetTable<User>();
                }
            }
        }
}
