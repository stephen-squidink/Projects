using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using ExiaProject.com.game.camera;

namespace ExiaProject.com.game.objects
{
    class SkyDome
    {
        Model _skyModel;
        Texture2D _skyMap;

        public SkyDome(Model skyModel, Texture2D skyMap, Effect effect)
        {
            _skyModel = skyModel;
            _skyMap = skyMap;

            _skyModel.Meshes[0].MeshParts[0].Effect = effect;
        }

        public void initialize(GraphicsDevice device)
        {

        }

        public void update(GameTime gameTime)
        {

        }

        public void render(GraphicsDevice device,ViewTransformations viewTrans)
        {
            device.RenderState.DepthBufferWriteEnable = false;

            Matrix[] modelTransforms = new Matrix[_skyModel.Bones.Count];
            _skyModel.CopyAbsoluteBoneTransformsTo(modelTransforms);

            Matrix wMatrix = Matrix.CreateTranslation(0, -0.3f, 0) * Matrix.CreateScale(100) * Matrix.CreateTranslation(viewTrans.cameraPosition);
            foreach (ModelMesh mesh in _skyModel.Meshes)
            {
                foreach (Effect currentEffect in mesh.Effects)
                {
                    Matrix worldMatrix = modelTransforms[mesh.ParentBone.Index] * wMatrix;
                    currentEffect.CurrentTechnique = currentEffect.Techniques["SkyDome"];
                    currentEffect.Parameters["xWorld"].SetValue(worldMatrix);
                    currentEffect.Parameters["xView"].SetValue(viewTrans.viewMatrix);
                    currentEffect.Parameters["xProjection"].SetValue(viewTrans.projectionMatrix);
                    currentEffect.Parameters["xTexture"].SetValue(_skyMap);
                    currentEffect.Parameters["xEnableLighting"].SetValue(false);
                }
                mesh.Draw();
            }
            device.RenderState.DepthBufferWriteEnable = true;
        }
    }
}
