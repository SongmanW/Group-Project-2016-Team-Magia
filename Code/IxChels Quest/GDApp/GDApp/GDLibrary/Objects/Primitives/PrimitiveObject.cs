using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GDApp;

namespace GDLibrary
{
    //to do...
    public class PrimitiveObject : DrawnActor
    {

        #region Fields
        private VertexPositionColor[] vertices;
        private BasicEffect effect;
        private PrimitiveType primitiveType;
        private int primitiveCount;
        #endregion

        #region Properties
        //add...
        #endregion

        public PrimitiveObject(string id, ObjectType objectType,
            Transform3D transform, VertexPositionColor[] vertices,
            BasicEffect effect, PrimitiveType primitiveType, int primitiveCount)
            : base(id, objectType, transform)
        {
          
            this.vertices = vertices;
            this.effect = effect;
            this.primitiveType = primitiveType;
            this.primitiveCount = primitiveCount;
        }

        public override void Draw(GameTime gameTime)
        {
            this.effect.World = this.Transform3D.World;
            this.effect.View = game.ActiveCamera.View;
            this.effect.Projection = game.ActiveCamera.ProjectionParameters.Projection;

            //load the GFX card with the W, V, and P values
            this.effect.CurrentTechnique.Passes[0].Apply();

            //send the vertices to the shader to be rendered
            this.effect.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(this.primitiveType, 
                this.vertices, 0, this.primitiveCount);
        }

        public object Clone()
        {
            return new PrimitiveObject(this.ID, this.ObjectType,
                (Transform3D)this.Transform3D.Clone(), this.vertices,
                this.effect, this.primitiveType, this.primitiveCount);
        }
    }
}
