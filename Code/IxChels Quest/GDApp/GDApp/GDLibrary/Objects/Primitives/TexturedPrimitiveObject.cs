using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GDApp;

namespace GDLibrary
{
    //to do...
    public class TexturedPrimitiveObject : DrawnActor
    {
        #region Fields
        private VertexPositionColorTexture[] vertices;
        private BasicEffect effect;
        private PrimitiveType primitiveType;
        private int primitiveCount;
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
        //add...
        #endregion

        public TexturedPrimitiveObject(string id, ObjectType objectType,
            Transform3D transform, VertexPositionColorTexture[] vertices,
            BasicEffect effect, PrimitiveType primitiveType, int primitiveCount, Texture2D texture)
            : base(id, objectType, transform)
        {
            this.vertices = vertices;
            this.effect = effect;
            this.primitiveType = primitiveType;
            this.primitiveCount = primitiveCount;
            this.texture = texture;
        }

        public override void Draw(GameTime gameTime)
        {
            this.effect.World = this.Transform3D.World;
            this.effect.View = game.ActiveCamera.View;
            this.effect.Projection = game.ActiveCamera.ProjectionParameters.Projection;

            this.effect.Texture = this.texture;

            //load the GFX card with the W, V, and P values
            this.effect.CurrentTechnique.Passes[0].Apply();

            //send the vertices to the shader to be rendered
            this.effect.GraphicsDevice.DrawUserPrimitives<VertexPositionColorTexture>(this.primitiveType,
                this.vertices, 0, this.primitiveCount);
        }

        public object Clone()
        {
            return new TexturedPrimitiveObject(this.ID, this.ObjectType,
                (Transform3D)this.Transform3D.Clone(), this.vertices,
                this.effect, this.primitiveType, this.primitiveCount, this.texture);
        }
    }
}
