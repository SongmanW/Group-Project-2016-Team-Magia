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
        private Matrix[] boneTransforms;
        #endregion

        #region Properties 
        //add...
        public Matrix[] BoneTransforms
        {
            get
            {
                return this.boneTransforms;
            }
            set
            {
                this.boneTransforms = value;
            }
        }
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

            if (this.model != null)
            {
                this.boneTransforms = new Matrix[model.Bones.Count];
                model.CopyAbsoluteBoneTransformsTo(this.boneTransforms);
            }
        }

        public virtual object Clone()
        {
            return new ModelObject(this.ID, this.ObjectType, (Transform3D)this.Transform3D.Clone(), this.Effect.Clone(), texture, model, this.Color, this.Alpha);
        }
    }
}
