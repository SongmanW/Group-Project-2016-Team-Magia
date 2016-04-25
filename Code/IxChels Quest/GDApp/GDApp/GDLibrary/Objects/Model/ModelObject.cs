using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDLibrary
{
    public class ModelObject : DrawnActor
    {
        #region Fields
        private Texture2D texture;
        private Model model;

        //each mesh in the model has a bone transform which represent the transformation necessary to position it in 3D design program e.g. 3DS Max
        private Matrix[] transforms;
        #endregion

        #region Properties 
        //add...
        public Texture2D Texture
        {
            get
            {
                return this.texture;
            }
        }
        public Model Model
        {
            get
            {
                return this.model;
            }
        }
        #endregion

        public ModelObject(string id, ObjectType objectType, Transform3D transform, Effect effect, Texture2D texture, Model model, Color color, float alpha)
            : base(id, objectType, transform, effect, color, alpha)
        {
            this.texture = texture;
            this.model = model;

            //load bone transforms and copy transfroms to transform array (transforms)
            this.transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(this.transforms);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (ModelMesh mesh in model.Meshes)
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
                    be.World = transforms[mesh.ParentBone.Index] * this.Transform3D.World;

                    if (texture != null)
                    {
                        be.TextureEnabled = true;
                        be.Texture = this.texture;
                    }else
                    {
                        be.TextureEnabled = true;
                    }
                }
                //Draw
                mesh.Draw();
            }
            base.Draw(gameTime);
        }

        public virtual object Clone()
        {
            return new ModelObject(this.ID, this.ObjectType, (Transform3D)this.Transform3D.Clone(), this.Effect.Clone(), texture, model, this.Color, this.Alpha);
        }
    }
}
