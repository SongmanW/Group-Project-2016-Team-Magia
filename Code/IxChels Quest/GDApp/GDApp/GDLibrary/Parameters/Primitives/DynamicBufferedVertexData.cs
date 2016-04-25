using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework;

namespace GDLibrary
{
    public class DynamicBufferedVertexData<T> : VertexData<T> where T : struct, IVertexType
    {
        #region Variables
        private DynamicVertexBuffer vertexBuffer;
        private GraphicsDevice graphicsDevice;
        #endregion

        #region Properties
        public DynamicVertexBuffer VertexBuffer
        {
            get
            {
                return vertexBuffer;
            }
            set
            {
                vertexBuffer = value;

            }
        }
        #endregion

        public DynamicBufferedVertexData(GraphicsDevice graphicsDevice, T[] vertices, PrimitiveType primitiveType, int primitiveCount)
            : base(vertices, primitiveType, primitiveCount)
        {
            this.graphicsDevice = graphicsDevice;

            //reserve space on gfx will be CHANGED frequently
            this.vertexBuffer = new DynamicVertexBuffer(graphicsDevice, typeof(T), vertices.Length, BufferUsage.None);
            //add an event listener to reset the data if another game object access the graphics device and (potentially) resets buffer contents
            this.vertexBuffer.ContentLost += vertexBuffer_ContentLost;
            UpdateData();
        }

        public void SetData(T[] vertices)
        {
            this.Vertices = vertices;
            //set data on the reserved space
            this.vertexBuffer.SetData<T>(this.Vertices);
        }

        public void UpdateData()
        {
            //set data on the reserved space
            this.vertexBuffer.SetData<T>(this.Vertices);
        }

        void vertexBuffer_ContentLost(object sender, EventArgs e)
        {
            //set data on the reserved space
            this.vertexBuffer.SetData<T>(this.Vertices);
        }

        public override void Draw(GameTime gameTime, Effect effect)
        {
            //this is what we want GFX to draw
            effect.GraphicsDevice.SetVertexBuffer(this.vertexBuffer);

            //draw!
            effect.GraphicsDevice.DrawPrimitives(this.PrimitiveType, 0, this.PrimitiveCount);
        }

        public new object Clone()
        {
            return new DynamicBufferedVertexData<T>(this.graphicsDevice, this.Vertices, this.PrimitiveType, this.PrimitiveCount);
        }
    }
}
