using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PloobsEngine.MessageSystem;
using PloobsEngine.Light;
using PloobsEngine.Material;
using PloobsEngine.Modelo;
using PloobsEngine.Physics;
using PloobsEngine.Cameras;
using System.Runtime.Serialization;
using PloobsEngine.Engine.Logger;
using PloobsEngine.SceneControl;
using PloobsEngine.Modelo2D;
using PloobsEngine.Utils;
using PloobsEngine.Physics.Bepu;
using BehaviorTrees;
using ProximityDatabase = Bnoerj.AI.Steering.IProximityDatabase<Bnoerj.AI.Steering.IVehicle>;

namespace Settlers.Ploobs.Object
{
    public class ISpriteObject : BaseObject, Entity
    {
        String[] anims ;

        public String[] Animations
        {
            get
            {
                return anims;
            }
        }

        public CharacterObject CharacterObject
        {
            get;
            private set;
        }

        PloobsVehicle steerMoverController;
        public PloobsVehicle SteerMoverController
        {
            set
            {
                steerMoverController = value;
                TaskResult = BehaviorTrees.TaskResult.Running;
            }
            get
            {
                return steerMoverController ;                
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IObject"/> class.
        /// </summary>
        /// <param name="Material">The material.</param>
        /// <param name="Modelo">The modelo.</param>
        /// <param name="PhysicObject">The physic object.</param>
        public ISpriteObject(IMaterial Material, SpriteAnimated Modelo, CharacterObject PhysicObject, float animationSpeed = 20)
        {
            System.Diagnostics.Debug.Assert(Modelo != null);
            System.Diagnostics.Debug.Assert(Material != null);
            System.Diagnostics.Debug.Assert(PhysicObject != null);

            this.CharacterObject = PhysicObject;
            this.Material = Material;
            this.Modelo = null;
            this.SpriteAnimated = Modelo;
            this.PhysicObject = PhysicObject;
            IObjectAttachment = new List<IObjectAttachment>();            
            Name = null;
            TaskResult = BehaviorTrees.TaskResult.Success;

            anims = new String[Modelo.NumberAnimations];
            for (int i = 0; i < Modelo.NumberAnimations ; i++)
            {
                String name = i.ToString();
                SpriteAnimated.AddAnimation(name, i + 1, Modelo.Frame, 0, true, animationSpeed);
                anims[i] = name;
            }
        }

        public SpriteAnimated SpriteAnimated
        {
            set;
            get;
        }

        Vector2 direction = Vector2.Zero;
        public void MoveCharacter(Vector2 direction)
        {
            this.direction = direction;
        }

        public void RotateCharacter(float amount)
        {
            CharacterObject.RotateYByAngleDegrees(amount);
        } 

        int pcurrent = 0;
        protected override void UpdateObject(GameTime gt, ICamera cam, IWorld world)
        {
            if (task != null && TaskResult != TaskResult.Success)
                TaskResult = task.Execute(this, gt);
            
            if (direction != Vector2.Zero)
            {
                CharacterObject.MoveToDirection(Vector2.Normalize(direction));
                SpriteAnimated.PlayCurrentAnimation();
                direction = Vector2.Zero;
            }
            else
            {
                SpriteAnimated.PauseCurrentAnimation();
                CharacterObject.MoveToDirection(Vector2.Zero);
            }                


            float angle = (float)VectorUtils.FindAngleBetweenTwoVectors2D(PhysicObject.FaceVector, Vector3.Normalize(cam.Position - CharacterObject.Position));
            angle = MathHelper.ToDegrees(angle);
            int p = (int)Math.Round(angle / 45f);
            if (p < 0)
                p = 8 + p;

            if (pcurrent != p)
            {
                SpriteAnimated.ChangeAnimation(anims[p], SpriteAnimated.GetCurrentFrameIndex(), SpriteAnimated.IsPlaying());
                pcurrent = p;                
            }

            SpriteAnimated.Update(gt);

            
            base.UpdateObject(gt, cam, world);
        }

        Node<ISpriteObject> behavior;
        Task<ISpriteObject> task;
        public Node<ISpriteObject> Behavior
        {
            get
            {
                return behavior;
            }

            set
            {
                behavior = value;
                task = behavior.Evaluate(this);
                TaskResult = TaskResult.Running;
            }
        }

        public TaskResult TaskResult
        {
            get;
            protected set;

        }    

        public override void CleanUp(PloobsEngine.Engine.GraphicFactory factory)
        {
            SpriteAnimated.CleanUp(factory);
            Material.CleanUp(factory);
            Modelo = null;
            Material = null;
            PhysicObject = null;
            IObjectAttachment.Clear();
            IObjectAttachment = null;
        }

    }
}
