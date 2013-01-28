using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bnoerj.AI.Steering;
using BehaviorTrees;

namespace Settlers.Ploobs.Object
{
    public class PloobsVehicle : SimpleVehicle
    {
        public PloobsVehicle(bool reset = false)
            : base(reset)
        {
            TaskResult = BehaviorTrees.TaskResult.Running;
        }

        public TaskResult TaskResult
        {
            get;
            protected set;
        }

        public virtual void Update(float currentTime, float elapsedTime) { }
    }
}
