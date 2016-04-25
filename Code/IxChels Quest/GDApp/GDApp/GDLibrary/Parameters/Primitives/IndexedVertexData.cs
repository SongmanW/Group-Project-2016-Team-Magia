using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GDLibrary
{
    public class IndexedVertexData<T> : BufferedVertexData<T> where T : struct, IVertexType
    {
        #region Variables
        private short[] indices;
        private IndexBuffer indexBuffer;
        private GraphicsDevice graphicsDevice;
        #endregion

        #region Properties
        public IndexBuffer IndexBuffer
        {
            get
            {
                return indexBuffer;
            }
            set
            {
                indexBuffer = value;
            }
        }
        #endregion

        public IndexedVertexData(GraphicsDevice graphicsDevice, T[] vertices, PrimitiveType primitiveType, int primitiveCount)
            : base(graphicsDevice, vertices, primitiveType, primitiveCount)
        {
            //remove duplicate vertices and set indices
            GetUniqueIndices(vertices);
            this.graphicsDevice = graphicsDevice;
            this.indexBuffer = new IndexBuffer(graphicsDevice, IndexElementSize.SixteenBits, indices.Length, BufferUsage.None);
            this.indexBuffer.SetData<short>(this.indices);
        }

        public void GetUniqueIndices(T[] vertices)
        {
            //unique vertices
            List<T> vertexList = new List<T>();
            //unique indices
            List<short> indicesList = new List<short>();
            //dictionary of vertex, index pairs
            Dictionary<T, short> indexDictionary = new Dictionary<T, short>();

            short index = 0;
            short uniqueIndex = 0;

            for (int i = 0; i < vertices.Length; i++)
            {
                T vertex = vertices[i];
                if (indexDictionary.TryGetValue(vertex, out index)) //vertex is not unique
                {
                    //since this vertex already exists, we should simple add its index to the indices list
                    indicesList.Add(index);
                }
                else //vertex is unique
                {
                    //add vertex to unique list
                    vertexList.Add(vertex);
                    //add vertex-index pair to the dictionary
                    indexDictionary.Add(vertex, uniqueIndex);
                    //add the new unique index
                    indicesList.Add(uniqueIndex);
                    //increment the unique index count
                    uniqueIndex++;
                }
            }

            //since the GFX card uses arrays we need to convert from a list to an array
            this.Vertices = vertexList.ToArray();
            this.indices = indicesList.ToArray();
        }

        public override void Draw(GameTime gameTime, Effect effect)
        {
            effect.GraphicsDevice.Indices = this.indexBuffer;
            effect.GraphicsDevice.SetVertexBuffer(this.VertexBuffer);

            effect.GraphicsDevice.DrawIndexedPrimitives(this.PrimitiveType, 0, 0,
                this.Vertices.Length, 0, this.PrimitiveCount);
    
        }

        public new object Clone()
        {
            return new IndexedVertexData<T>(this.graphicsDevice, this.Vertices, this.PrimitiveType, this.PrimitiveCount);
        }
    }
}
