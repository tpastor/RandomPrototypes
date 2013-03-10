using EngineTestes.RQueue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpDxExamples
{
    class Program
    {
        static void Main(string[] args)
        {

            long id = 0;
            id = 3 << 5;

            var st = Convert.ToString((long)id, 2);
            Console.WriteLine(st);

            RenderQueueId ri = new RenderQueueId(1, 0, 0, 0, 0, 1, 4);            
            ri.GenerateId(true);
            Console.WriteLine(ri);
            

        }
    }
}

