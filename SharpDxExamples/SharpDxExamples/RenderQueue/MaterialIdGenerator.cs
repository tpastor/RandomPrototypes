using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EngineTestes.RQueue
{
    public static class MaterialIdGenerator
    {
        static int id = 0;
        static object sync = new object();

        public static int GenerateId()
        {
            lock (sync)
            {
                System.Diagnostics.Debug.Assert(id < 16777216 /*Math.Pow(2,24)*/);
                return id++;
            }
        }
    }
}
