using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework;

namespace GDLibrary
{
    public class VertexData<T> : IVertexData where T : struct, IVertexType
    {
        #region Variables
        private T[] vertices;
        private PrimitiveType primitiveType;
        private int primitiveCount;
        #endregion

        #region Properties
        public T[] Vertices
        {
            get
            {
                return vertices;
            }
            set
            {
                vertices = value;
            }
        }
        public PrimitiveType PrimitiveType
        {
            get
            {
                return primitiveType;
            }
            set
            {
                primitiveType = value;
            }
        }
        public int PrimitiveCount
        {
            get
            {
                return primitiveCount;
            }
            set
            {
                primitiveCount = value;
            }
        }
        #endregion

        public VertexData(T[] vertices, PrimitiveType primitiveType, int primitiveCount)
        {
            this.vertices = vertices;
            this.primitiveType = primitiveType;
            this.primitiveCount = primitiveCount;
        }

        public virtual void Draw(GameTime gameTime, Effect effect)
        {
            effect.GraphicsDevice.DrawUserPrimitives<T>(primitiveType, this.vertices, 0, primitiveCount);
        }

        public virtual object Clone()
        {
            return new VertexData<T>(this.Vertices, this.primitiveType, this.primitiveCount);
        }
    }
}
