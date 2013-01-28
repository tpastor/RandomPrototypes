using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PloobsEngine.Cameras;
using PloobsEngine.Light;
using PloobsEngine.Material;
using PloobsEngine.Modelo;
using PloobsEngine.Modelo2D;
using PloobsEngine.Physics;
using PloobsEngine.Physics.Bepu;
using PloobsEngine.SceneControl;
using PloobsEngine.Utils;
using Settlers.Ploobs.Material;
using Settlers.Ploobs.Object;

namespace ProjectTemplate
{
    /// <summary>
    /// Basic Deferred Scene
    /// </summary>
    public class TemplateDeferredScreen : IScene
    {
        /// <summary>
        /// Sets the world and render technich.
        /// </summary>
        /// <param name="renderTech">The render tech.</param>
        /// <param name="world">The world.</param>
        protected override void SetWorldAndRenderTechnich(out IRenderTechnic renderTech, out IWorld world)
        {
            ///create the world using bepu as physic api and a simple culler implementation
            ///IT DOES NOT USE PARTICLE SYSTEMS (see the complete constructor, see the ParticleDemo to know how to add particle support)
            world = new IWorld(new BepuPhysicWorld(), new SimpleCuller());

            ///Create the deferred description
            DeferredRenderTechnicInitDescription desc = DeferredRenderTechnicInitDescription.Default();
            ///Some custom parameter, this one allow light saturation. (and also is a pre requisite to use hdr)
            desc.UseFloatingBufferForLightMap = true;
            ///set background color, default is black
            desc.BackGroundColor = Color.CornflowerBlue;
            ///create the deferred technich
            renderTech = new DeferredRenderTechnic(desc);
        }

        /// <summary>
        /// Load content for the screen.
        /// </summary>
        /// <param name="GraphicInfo"></param>
        /// <param name="factory"></param>
        /// <param name="contentManager"></param>
        protected override void LoadContent(PloobsEngine.Engine.GraphicInfo GraphicInfo, PloobsEngine.Engine.GraphicFactory factory, IContentManager contentManager)
        {
            ///must be called before all
            base.LoadContent(GraphicInfo, factory, contentManager);

            ///Uncoment to Add an object
            /////Create a simple object
            /////Geomtric Info and textures (this model automaticaly loads the texture)
            //SimpleModel simpleModel = new SimpleModel(factory, "Model FILEPATH GOES HERE", "Diffuse Texture FILEPATH GOES HERE -- Use only if it is not embeded in the Model file");            
            /////Physic info (position, rotation and scale are set here)
            //TriangleMeshObject tmesh = new TriangleMeshObject(simpleModel, Vector3.Zero, Matrix.Identity, Vector3.One, MaterialDescription.DefaultBepuMaterial());
            /////Shader info (must be a deferred type)
            //DeferredNormalShader shader = new DeferredNormalShader();
            /////Material info (must be a deferred type also)
            //DeferredMaterial fmaterial = new DeferredMaterial(shader);
            /////The object itself
            //IObject obj = new IObject(fmaterial, simpleModel, tmesh);
            /////Add to the world
            //this.World.AddObject(obj);

            {
                SimpleModel simpleModel = new SimpleModel(factory, "Model//block");
                simpleModel.SetTexture(factory.CreateTexture2DColor(1, 1, Color.Red), TextureType.DIFFUSE);
                BoxObject tmesh = new BoxObject(Vector3.Zero, 1, 1, 1, 10, new Vector3(500, 5, 500), Matrix.Identity, MaterialDescription.DefaultBepuMaterial());
                tmesh.isMotionLess = true;
                ForwardXNABasicShader shader = new ForwardXNABasicShader(ForwardXNABasicShaderDescription.Default());
                ForwardMaterial fmaterial = new ForwardMaterial(shader);
                IObject obj = new IObject(fmaterial, simpleModel, tmesh);
                this.World.AddObject(obj);
            }

            {
                SimpleModel simpleModel = new SimpleModel(factory, "Model//block");
                simpleModel.SetTexture(factory.CreateTexture2DColor(1, 1, Color.Yellow), TextureType.DIFFUSE);
                GhostObject go = new GhostObject(Vector3.Zero, Matrix.Identity, new Vector3(1, 100, 1));
                ForwardXNABasicShader shader = new ForwardXNABasicShader(ForwardXNABasicShaderDescription.Default());
                ForwardMaterial fmaterial = new ForwardMaterial(shader);
                BaseObject obj = new BaseObject(fmaterial, simpleModel, go);
                obj.Name = "TORRE1";
                this.World.AddObject(obj);
                //entityFactory.CreateShooterTower(obj, 3, 2, 100);
            }

            {
                Texture2D tex = factory.GetTexture2D("Textures//spriteteste");
                SpriteAnimated SpriteAnimated = new PloobsEngine.Modelo2D.SpriteAnimated(tex, 9, 8);
                SpriteAnimated.PlayCurrentAnimation();

                CharacterObject Characterobj = new CharacterObject(new Vector3(25, 15, 25), Matrix.Identity, 10, 5, 10, 1f, Vector3.One, 5);
                Sprite3DShader Sprite3DShader = new Sprite3DShader(Vector2.One);
                ForwardMaterial fmaterial = new ForwardMaterial(Sprite3DShader);
                ISpriteObject obj = new ISpriteObject(fmaterial, SpriteAnimated, Characterobj);
                obj.Name = "Enemy";
                this.World.AddObject(obj);
                obj.OnUpdate += new OnUpdate(obj_OnUpdate);

            }

            ///Add some directional lights to completely iluminate the world
            #region Lights
            DirectionalLightPE ld1 = new DirectionalLightPE(Vector3.Left, Color.White);
            DirectionalLightPE ld2 = new DirectionalLightPE(Vector3.Right, Color.White);
            DirectionalLightPE ld3 = new DirectionalLightPE(Vector3.Backward, Color.White);
            DirectionalLightPE ld4 = new DirectionalLightPE(Vector3.Forward, Color.White);
            DirectionalLightPE ld5 = new DirectionalLightPE(Vector3.Down, Color.White);
            float li = 0.4f;
            ld1.LightIntensity = li;
            ld2.LightIntensity = li;
            ld3.LightIntensity = li;
            ld4.LightIntensity = li;
            ld5.LightIntensity = li;
            this.World.AddLight(ld1);
            this.World.AddLight(ld2);
            this.World.AddLight(ld3);
            this.World.AddLight(ld4);
            this.World.AddLight(ld5);
            #endregion

            ///Add a AA post effect
            this.RenderTechnic.AddPostEffect(new AntiAliasingPostEffect());

            ///add a camera
            this.World.CameraManager.AddCamera(new CameraFirstPerson(GraphicInfo));
        }

        /// <summary>
        /// This is called when the screen should draw itself.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="render"></param>
        protected override void Draw(GameTime gameTime, RenderHelper render)
        {
            ///must be called before
            base.Draw(gameTime, render);

            ///Draw some text to the screen
            render.RenderTextComplete("Demo: Basic Screen Deferred", new Vector2(GraphicInfo.Viewport.Width - 315, 15), Color.White, Matrix.Identity);
        }


        Keys frente = Keys.G;
        Keys tras = Keys.T;
        Keys direita = Keys.F;
        Keys esquerda = Keys.H;
        Keys pulo = Keys.R;
        void obj_OnUpdate(IObject obj, GameTime gt, ICamera cam)
        {
            SpriteAnimated SpriteAnimated = (obj as ISpriteObject).SpriteAnimated;
            ISpriteObject ISpriteObject = (ISpriteObject)obj;
            KeyboardState keyboardInput = Keyboard.GetState();

            Vector2 totalMovement = Vector2.Zero;
            Vector2 mv = VectorUtils.ToVector2(ISpriteObject.CharacterObject.FaceVector);
            Vector2 lado = VectorUtils.Perpendicular2DNormalized(mv);

            ///TO SLIDE MOVEMENT USE
            //totalMovement += lado;
            //totalMovement -= lado;

            if (keyboardInput.IsKeyDown(frente))
            {
                totalMovement -= mv;
            }
            if (keyboardInput.IsKeyDown(tras))
            {
                totalMovement += mv;
            }
            if (keyboardInput.IsKeyDown(esquerda))
            {
                ISpriteObject.RotateCharacter(-1);
            }
            if (keyboardInput.IsKeyDown(direita))
            {
                ISpriteObject.RotateCharacter(1);
            }
            if (totalMovement == Vector2.Zero)
            {
                ISpriteObject.MoveCharacter(Vector2.Zero);
                SpriteAnimated.PauseCurrentAnimation();
            }
            else
            {
                ISpriteObject.MoveCharacter(Vector2.Normalize(totalMovement));
                SpriteAnimated.PlayCurrentAnimation();
            }

            //Jumping
            if (keyboardInput.IsKeyDown(pulo))
            {
                ISpriteObject.CharacterObject.Jump();
            }
        }

    }
}

