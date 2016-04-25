using GDApp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GDLibrary
{
    public class ObjectManager : GenericDraweableManager<DrawnActor>
    {
        #region Fields
        private RasterizerState rasterizerState;
        private bool bDebugMode;
        #endregion

        #region Properties
        public bool IsDebugMode
        {
            get
            {
                return bDebugMode;
            }
            set
            {
                bDebugMode = value;
            }
        }
        #endregion

        public ObjectManager(Main game, int initialDrawSize, int initialRemoveSize, bool bDebugMode)
            : base(game, initialDrawSize, initialRemoveSize)
        {
            this.bDebugMode = bDebugMode;
            SetGraphicsStateObjects();

            #region Event Handling
            //add any class specific event handling methods here
            game.EventDispatcher.RotationStarted += HandleRotationStart;
            game.EventDispatcher.RotationEnd += HandleRotationEnd;
            #endregion
        }


        #region Event Handling
        //add any class specific event handling methods here   private void HandleRotationEnd(EventData data)
        private void HandleRotationEnd(EventData data)
        {
            ((RotatorController)(this.game.wall1).ControllerList[0]).Unset();
            ((RotatorController)(this.game.wall2).ControllerList[0]).Unset();
            this.game.playerActor.Unset();
        }

        private void HandleRotationStart(EventData data)
        {
            ((RotatorController)(this.game.wall1).ControllerList[0]).Set();
            ((RotatorController)(this.game.wall2).ControllerList[0]).Set();
            this.game.playerActor.Set();
            ((RotorController)this.game.rotator.ControllerList[1]).Rotate(90, 4000, ((RotationEventData)data).clockwise);
        }
        #endregion

        public override void SetGraphicsStateObjects()
        {
            this.rasterizerState = new RasterizerState();
            this.rasterizerState.FillMode = FillMode.Solid;

            //set to None for transparent objects
            this.rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
        }

        public override void Draw(GameTime gameTime)
        {
            DrawnActor gameObject = null;

            //if you want to see game around menu edges then disable this if()
            if (!this.IsPaused)
            {
                //Remember this code from our initial aliasing problems with the Sky box?
                //enable anti-aliasing along the edges of the quad i.e. to remove jagged edges to the primitive
                this.Game.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;

                //set the appropriate state e.g. wireframe, cull none?
                this.Game.GraphicsDevice.RasterizerState = this.rasterizerState;

                //enable alpha blending for transparent objects i.e. trees
                this.Game.GraphicsDevice.BlendState = BlendState.AlphaBlend;

                //disable to see what happens when we disable depth buffering - look at the boxes
                this.Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

                for (int i = 0; i < Size; i++)
                {
                    gameObject = this[i];

                    if (gameObject is AnimatedPlayerObject)
                    {
                        DrawObject(gameTime, gameObject as AnimatedPlayerObject);
                    }
                    else if (gameObject is ModelObject)
                    {
                        DrawObject(gameTime, gameObject as ModelObject);
                    }
                    else if (gameObject is BillboardPrimitiveObject)
                    {
                        DrawObject(gameTime, gameObject as BillboardPrimitiveObject);
                    }
                    else if (gameObject is TexturedPrimitiveObject)
                    {
                        DrawObject(gameTime, gameObject as TexturedPrimitiveObject);
                    }
                    else if (gameObject is PrimitiveObject)
                    {
                        DrawObject(gameTime, gameObject as PrimitiveObject);
                    }

                    if ((this.IsDebugMode) && ((gameObject is ZoneObject) || (gameObject is ModelObject)))
                        DebugDrawCollisionSkin(gameObject);
                }
            }
        }

        private void DrawObject(GameTime gameTime, AnimatedPlayerObject animatedPlayerObject)
        {
            Effect effect = animatedPlayerObject.Effect;

            Model model = animatedPlayerObject.Model;

            //an array of the current positions of the model meshes
            Matrix[] bones = animatedPlayerObject.AnimationPlayer.GetSkinTransforms();

            Matrix world = animatedPlayerObject.GetWorldMatrix();

            for (int i = 0; i < bones.Length; i++)
            {
                bones[i] *= world;
            }

            //remember we can move this code inside the first foreach loop below
            //and use mesh.Name to change the textures applied during the effect
            effect.CurrentTechnique = effect.Techniques["SimpleTexture"];
            effect.Parameters["DiffuseMapTexture"].SetValue(animatedPlayerObject.Texture);

            effect.Parameters["Bones"].SetValue(bones);
            effect.Parameters["View"].SetValue((this.Game as Main).CameraManager.ActiveCamera.View);
            effect.Parameters["Projection"].SetValue((this.Game as Main).CameraManager.ActiveCamera.ProjectionParameters.Projection);
            effect.Parameters["Alpha"].SetValue(animatedPlayerObject.Alpha);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;
                }
                mesh.Draw();
            }
        }

        //draw a model object 
        private void DrawObject(GameTime gameTime, ModelObject modelObject)
        {
            if (modelObject.Model != null)
            {
                foreach (ModelMesh mesh in modelObject.Model.Meshes)
                {
                    foreach (BasicEffect be in mesh.Effects)
                    {
                        //be.EnableDefaultLighting();

                        //uncomment and try this code
                        be.EmissiveColor = Color.DarkGray.ToVector3();
                        be.DirectionalLight0.DiffuseColor = Color.White.ToVector3();
                        be.DirectionalLight0.Direction = new Vector3(0, 0, -1);
                        be.DirectionalLight0.Enabled = true;
                        be.SpecularColor = Color.White.ToVector3();
                        be.SpecularPower = 0.1f;


                        be.Projection = game.CameraManager.ActiveCamera.ProjectionParameters.Projection;
                        be.View = game.CameraManager.ActiveCamera.View;
                        be.World = modelObject.BoneTransforms[mesh.ParentBone.Index] * modelObject.Transform3D.World;

                        if (modelObject.Texture != null)
                        {
                            be.TextureEnabled = true;
                            be.Texture = modelObject.Texture;
                        }
                        else
                        {
                            be.TextureEnabled = true;
                        }
                    }
                    //Draw
                    mesh.Draw();
                }
            }
        }

        //draw a NON-TEXTURED primitive i.e. vertices (and possibly indices) defined by the user
        private void DrawObject(GameTime gameTime, PrimitiveObject primitiveObject)
        {
            BasicEffect effect = primitiveObject.Effect as BasicEffect;

            //W, V, P, Apply, Draw
            effect.World = primitiveObject.GetWorldMatrix();
            effect.View = (this.Game as Main).CameraManager.ActiveCamera.View;
            effect.Projection = (this.Game as Main).CameraManager.ActiveCamera.ProjectionParameters.Projection;

            effect.DiffuseColor = primitiveObject.ColorAsVector3;
            effect.Alpha = primitiveObject.Alpha;

            effect.CurrentTechnique.Passes[0].Apply();

            primitiveObject.VertexData.Draw(gameTime, effect);
        }

        //draw a NON-TEXTURED primitive i.e. vertices (and possibly indices) defined by the user
        private void DrawObject(GameTime gameTime, TexturedPrimitiveObject texturedPrimitiveObject)
        {
            BasicEffect effect = texturedPrimitiveObject.Effect as BasicEffect;

            //W, V, P, Apply, Draw
            effect.World = texturedPrimitiveObject.GetWorldMatrix();
            effect.View = (this.Game as Main).CameraManager.ActiveCamera.View;
            effect.Projection = (this.Game as Main).CameraManager.ActiveCamera.ProjectionParameters.Projection;

            if (texturedPrimitiveObject.Texture != null) //e.g. VertexPositionColor vertices will have no UV coordinates - so no texture
                effect.Texture = texturedPrimitiveObject.Texture;

            effect.DiffuseColor = texturedPrimitiveObject.ColorAsVector3;
            effect.Alpha = texturedPrimitiveObject.Alpha;

            effect.CurrentTechnique.Passes[0].Apply();

            texturedPrimitiveObject.VertexData.Draw(gameTime, effect);
        }

        BillboardParameters billboardParameters;
        private void DrawObject(GameTime gameTime, BillboardPrimitiveObject billboardPrimitiveObject)
        {
            Effect effect = billboardPrimitiveObject.Effect;
            billboardParameters = billboardPrimitiveObject.BillboardParameters;

            //W, V, P, Apply, Draw
            effect.CurrentTechnique = effect.Techniques[billboardParameters.Technique];
            effect.Parameters["World"].SetValue(billboardPrimitiveObject.Transform3D.World);
            effect.Parameters["View"].SetValue((this.Game as Main).CameraManager.ActiveCamera.View);
            effect.Parameters["Projection"].SetValue((this.Game as Main).CameraManager.ActiveCamera.ProjectionParameters.Projection);
            effect.Parameters["Up"].SetValue(billboardPrimitiveObject.Transform3D.Up);
            effect.Parameters["DiffuseTexture"].SetValue(billboardPrimitiveObject.Texture);
            effect.Parameters["Alpha"].SetValue(billboardPrimitiveObject.Alpha);

            if (billboardParameters.BillboardType == BillboardType.Normal)
            {
                effect.Parameters["Right"].SetValue(billboardPrimitiveObject.Transform3D.Right);
            }

            if (billboardParameters.IsScrolling)
            {
                effect.Parameters["IsScrolling"].SetValue(billboardParameters.IsScrolling);
                effect.Parameters["scrollRate"].SetValue(billboardParameters.scrollValue);
            }
            else
            {
                effect.Parameters["IsScrolling"].SetValue(false);
            }

            if (billboardParameters.IsAnimated)
            {
                effect.Parameters["IsAnimated"].SetValue(billboardParameters.IsAnimated);
                effect.Parameters["InverseFrameCount"].SetValue(billboardParameters.inverseFrameCount);
                effect.Parameters["CurrentFrame"].SetValue(billboardParameters.currentFrame);
            }
            else
            {
                effect.Parameters["IsAnimated"].SetValue(false);
            }


            effect.CurrentTechnique.Passes[0].Apply();
            billboardPrimitiveObject.VertexData.Draw(gameTime, effect);
        }

        //debug method to draw collision skins for collidable objects and zone objects
        private void DebugDrawCollisionSkin(DrawnActor gameObject)
        {
            if (this.IsDebugMode)
            {
                if (gameObject is ZoneObject)
                {
                    ZoneObject zoneObject = gameObject as ZoneObject;
                    (this.Game as Main).PhysicsManager.DebugDrawer.DrawDebug(zoneObject.Body, zoneObject.Collision);
                }
                else if (gameObject is CollidableObject)
                {
                    CollidableObject collidableObject = gameObject as CollidableObject;
                    (this.Game as Main).PhysicsManager.DebugDrawer.DrawDebug(collidableObject.Body, collidableObject.Collision);
                }
            }
        }
    }

}
