using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EngineTestes.RQueue
{
    public class RenderQueue
    {
        RenderQueueElementComparer renderQueueElementComparer = new RenderQueueElementComparer();
        List<RenderQueueElement> elements = new List<RenderQueueElement>();

        public void Process()
        {
            elements.Sort(renderQueueElementComparer);
            ///
            ///
            ///
            elements.Clear();
        }
    }
}
