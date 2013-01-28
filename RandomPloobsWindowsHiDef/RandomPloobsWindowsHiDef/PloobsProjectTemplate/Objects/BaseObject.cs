using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PloobsEngine.Modelo;
using PloobsEngine.Physics;
using PloobsEngine.SceneControl;
using PloobsEngine.Material;

namespace Settlers.Ploobs.Object
{
    public class BaseObject : IObject
    {
         /// <summary>
        /// Initializes a new instance of the <see cref="IObject"/> class.
        /// </summary>
        /// <param name="Material">The material.</param>
        /// <param name="Modelo">The modelo.</param>
        /// <param name="PhysicObject">The physic object.</param>
        public BaseObject(IMaterial Material, IModelo Modelo, IPhysicObject PhysicObject)
            : base(Material,Modelo,PhysicObject)
        {
            
        }

        protected BaseObject()
        {
        }

        public Artemis.Entity ArtemisEntity
        {
            get;
            set;
        }
    }
}
