using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GDApp;

namespace GDLibrary
{
    //to do...
    public class PrimitiveObject : DrawnActor
    {

        #region Fields
        private IVertexData vertexData;
        private Effect effect;
        #endregion

        #region Properties
        public IVertexData VertexData
        {
            get
            {
                return this.vertexData;
            }
            set
            {
                this.vertexData = value;
            }
        }
        #endregion

        public PrimitiveObject(string id, ObjectType objectType,
            Transform3D transform, IVertexData vertexData, Effect effect, Color color, float alpha)
            : base(id, objectType, transform, effect, color, alpha)
        {
            this.vertexData = vertexData;
            this.effect = effect;
        }

        public object Clone()
        {
            return new PrimitiveObject(this.ID, this.ObjectType, (Transform3D)this.Transform3D.Clone(), this.vertexData, this.Effect, this.Color, this.Alpha);
        }
    }
}
