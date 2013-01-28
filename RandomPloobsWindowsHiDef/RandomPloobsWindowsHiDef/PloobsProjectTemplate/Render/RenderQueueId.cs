using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PloobsEngine.Modelo;
using PloobsEngine.Material;

namespace EngineTestes.RQueue
{
    public class RenderQueueId
    {
        public override string ToString()
        {
            return Convert.ToString(id, 2);
        }

        /*
         * 2 Fullscreen layer. Are we drawing to the game layer, a fullscreen effect layer, or the HUD?
           3 Viewport. There could be multiple viewports for e.g. multiplayer splitscreen, for mirrors or portals in the scene, etc.
           3 Viewport layer. Each viewport could have their own skybox layer, world layer, effects layer, HUD layer.
           2 Translucency. Typically we want to sort opaque geometry and normal, additive, and subtractive translucent geometry into separate groups.
           6 Extra
           24 Depth sorting. We want to sort translucent geometry back-to-front for proper draw ordering and perhaps opaque geometry front-to-back to aid z-culling.
           24 Material. We want to sort by material to minimize state settings (textures, shaders, constants). A material might have multiple passes.
         * =64
         * */
        long id;

        int fullscreenLayer;
        int viewPort;
        int viewportLayer; 
        int translucency;
        int depthSorting;
        int materialid;
        int extra;

        public void Create(int fullscreenLayer, int viewPort, int viewportLayer, int translucency, int extra, int depthSorting, int material)
        {
            this.fullscreenLayer = fullscreenLayer;
            this.viewportLayer = viewportLayer;
            this.viewPort = viewPort;
            this.depthSorting = depthSorting;
            this.materialid = material;
            this.extra = extra;
        }

        public long CachedId
        {
            get
            {
                return id;
            }
        }

        public long GenerateId(bool flipMaterialWithSorting = false)
        {
            if (flipMaterialWithSorting)
            {
                id = fullscreenLayer << 64
                    | viewPort << 62
                    | viewportLayer << 59
                    | translucency << 56
                    | extra << 54
                    | materialid << 48
                    | depthSorting << 24
                    ;
            }
            else
            {
                id = fullscreenLayer << 64
                    | viewPort << 62
                    | viewportLayer << 59
                    | translucency << 56
                    | extra << 54
                    | depthSorting << 48
                    | materialid << 24
                    ;                
            }
            return id;
        }        
    }
}
