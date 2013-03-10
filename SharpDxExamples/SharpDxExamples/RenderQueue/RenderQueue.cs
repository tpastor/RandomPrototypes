using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EngineTestes.RQueue
{
    public class RenderQueueProcessor
    {
        RenderQueueElementComparer renderQueueElementComparer = new RenderQueueElementComparer();
        List<RenderQueueElement> elements = new List<RenderQueueElement>();

        public void Process()
        {
            if (elements.Count == 0)
                return;

            elements.Sort(renderQueueElementComparer);
            RenderQueueElement last = elements[0];
            last.InitMaterial();
            last.Draw();

            for (int i = 1; i < elements.Count; i++)
            {
                var el = elements[i];
                if (el.Id.materialid != last.Id.materialid)
                {
                    last.EndMaterial();
                    el.InitMaterial();
                }
                el.Draw();
                last = el;
            }
            last.EndMaterial();

            elements.Clear();
        }
    }
}
