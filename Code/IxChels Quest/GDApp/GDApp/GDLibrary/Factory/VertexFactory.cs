using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GDLibrary
{
    public class VertexFactory
    {
        //Dictionary

        public static VertexPositionColorTexture[] GetTextureQuadVertices()
        {
            float halfLength = 0.5f;

            Vector3 topLeft = new Vector3(-halfLength, halfLength, 0);
            Vector3 topRight = new Vector3(halfLength, halfLength, 0);
            Vector3 bottomLeft = new Vector3(-halfLength, -halfLength, 0);
            Vector3 bottomRight = new Vector3(halfLength, -halfLength, 0);

            //quad coplanar with the XY-plane (i.e. forward facing normal along UnitZ)
            VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[4];
            vertices[0] = new VertexPositionColorTexture(topLeft, Color.White, Vector2.Zero);
            vertices[1] = new VertexPositionColorTexture(topRight, Color.White, Vector2.UnitX);
            vertices[2] = new VertexPositionColorTexture(bottomLeft, Color.White, Vector2.UnitY);
            vertices[3] = new VertexPositionColorTexture(bottomRight, Color.White, Vector2.One);

            return vertices;
        }
    }
}
