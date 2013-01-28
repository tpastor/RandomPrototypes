using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PloobsEngine.Material;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Settlers.Ploobs.Object;
using PloobsEngine.Modelo2D;

namespace Settlers.Ploobs.Material
{
    public class Sprite3DShader : IShader
    {
        public Sprite3DShader(Vector2 scale)
        {
            this.scale = scale;
        }
        Vector2 scale;
        public override MaterialType MaterialType
        {
            get { return PloobsEngine.Material.MaterialType.FORWARD; }
        }

        protected override void  Draw(GameTime gt, PloobsEngine.SceneControl.IObject obj, PloobsEngine.SceneControl.RenderHelper render, PloobsEngine.Cameras.ICamera cam, IList<PloobsEngine.Light.ILight> lights)
        {            
            SpriteAnimated sprite = (obj as ISpriteObject).SpriteAnimated;
            
            basicEffect.World = Matrix.CreateConstrainedBillboard(obj.PhysicObject.WorldMatrix.Translation, cam.Position, Vector3.Down, null, null);
            basicEffect.View =  cam.View;
            basicEffect.Projection = cam.Projection;
            
            Vector2 nscale = scale * new Vector2(1f/sprite.NumberAnimations, 1f/sprite.Frame);
            spriteBatch.Begin(0, null, SamplerState.AnisotropicClamp, DepthStencilState.DepthRead, RasterizerState.CullNone, basicEffect);
            spriteBatch.Draw(sprite.Texture, Vector2.Zero, sprite.SourceRectangle, Color.White, sprite.Rotation, sprite.Origin, nscale, SpriteEffects.None, 1);            
            spriteBatch.End();
            render.ResyncStates();

            base.Draw(gt, obj, render, cam, lights);
        }


        BasicEffect basicEffect;
        SpriteBatch spriteBatch;
        PloobsEngine.Engine.GraphicInfo ginfo;
        public override void Initialize(PloobsEngine.Engine.GraphicInfo ginfo, PloobsEngine.Engine.GraphicFactory factory, PloobsEngine.SceneControl.IObject obj)
        {
            this.ginfo = ginfo;
            spriteBatch = factory.GetSpriteBatch();
            basicEffect = factory.GetBasicEffect();
            basicEffect.TextureEnabled = true;
            base.Initialize(ginfo, factory, obj);
        }
        
    }
}
