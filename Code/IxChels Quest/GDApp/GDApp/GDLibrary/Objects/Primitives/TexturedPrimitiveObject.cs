using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GDApp;

namespace GDLibrary
{
    //to do...
    public class TexturedPrimitiveObject : PrimitiveObject
    {
        #region Fields
        private Texture2D texture;
        #endregion

        #region Properties
        public Texture2D Texture
        {
            get
            {
                return this.texture;
            }
            set
            {
                this.texture = value;
            }
        }
        #endregion

        public TexturedPrimitiveObject(string id, ObjectType objectType,
            Transform3D transform, IVertexData vertexData, Effect effect, Color color, float alpha, Texture2D texture)
            : base(id, objectType, transform, vertexData, effect, color, alpha)
        {
            this.texture = texture;
        }

        public new object Clone()
        {
            return new TexturedPrimitiveObject(this.ID, this.ObjectType,
                (Transform3D)this.Transform3D.Clone(), this.VertexData, this.Effect, this.Color, this.Alpha, this.texture);
        }
    }
}
