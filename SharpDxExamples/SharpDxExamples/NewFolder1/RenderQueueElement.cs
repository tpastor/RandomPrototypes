using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EngineTestes.RQueue
{
    public abstract class RenderQueueElement
    {
        public RenderQueueId Id
        {
            get;
            private set;
        }

        public abstract void Draw();
        
    }

    public class RenderQueueElementComparer : IComparer<RenderQueueElement>
    {

        #region IComparer<RenderQueueElement> Members

        public int Compare(RenderQueueElement x, RenderQueueElement y)
        {
            if (x.Id.CachedId > y.Id.CachedId)
            {
                return 1;
            }
            else if (x.Id.CachedId == y.Id.CachedId)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }
        #endregion
    }


}
