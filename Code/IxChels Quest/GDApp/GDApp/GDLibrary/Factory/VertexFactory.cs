using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GDLibrary
{

    public static class VertexFactory
    {
        //Dictionary
        //Key - Circle, radius, angle
        //private static Dictionary<string, VertexPositionColor[]> dictionary;




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

        public static VertexPositionColor[] GetSpiralVertices(int radius, int angleInDegrees,
                            float verticalIncrement, out int primitiveCount)
        {
            VertexPositionColor[] vertices = GetCircleVertices(radius, angleInDegrees, out primitiveCount);

            for(int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Position.Z = verticalIncrement * i;
            }

            return vertices;
        }

        public static VertexPositionColor[] GetCircleVertices(int radius, 
                            int angleInDegrees, out int primitiveCount)
        {
            primitiveCount = 360 / angleInDegrees;
            VertexPositionColor[] vertices = new VertexPositionColor[primitiveCount + 1];

            Vector3 position = Vector3.Zero;
            float angleInRadians = MathHelper.ToRadians(angleInDegrees);

            for (int i = 0; i <= primitiveCount; i++)
            {
                position.X = (float)(radius * Math.Cos(i * angleInRadians));
                position.Y = (float)(radius * Math.Sin(i * angleInRadians));

                vertices[i] = new VertexPositionColor(position, Color.White);
            }
            return vertices;
        }








    }
}
