using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArrayList
{
    public class List
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
    public class ListPoint : List<List>
    {

        public ListPoint()
        {

        }

    }
}
