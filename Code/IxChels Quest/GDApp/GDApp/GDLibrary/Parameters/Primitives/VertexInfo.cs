using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GDLibrary
{
    public struct VertexBillboard : IVertexType
    {
        #region Variables
        public Vector3 position;
        public Vector4 texCoordAndOffset;
        #endregion

        public VertexBillboard(Vector3 position, Vector4 texCoordAndOffset)
        {
            this.position = position;
            this.texCoordAndOffset = texCoordAndOffset;
        }

        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration
        (
                new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                new VertexElement(12, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 0)
        );

        VertexDeclaration IVertexType.VertexDeclaration { get { return VertexDeclaration; } }
    }

    public class VertexInfo
    {
        //constant
        public static int ROUND_PRECISION_FLOAT = 3;

        /******************************************** Wireframe - Origin, Line, Circle, Quad, Cube & Billboard ********************************************/

        public static VertexPositionColor[] GetVerticesPositionColorLine(int sidelength, out PrimitiveType primitiveType,
                                                            out int primitiveCount)
        {
            primitiveType = PrimitiveType.LineList;
            primitiveCount = 1;

            VertexPositionColor[] vertices = new VertexPositionColor[2];

            float halfSideLength = sidelength / 2.0f;

            Vector3 left = new Vector3(-halfSideLength, 0, 0);
            Vector3 right = new Vector3(halfSideLength, 0, 0);

            vertices[0] = new VertexPositionColor(left, Color.White);
            vertices[1] = new VertexPositionColor(right, Color.White);

            return vertices;
        }

        public static VertexPositionColor[] GetVerticesPositionColorOriginHelper(out PrimitiveType primitiveType,
                                                            out int primitiveCount)
        {
            primitiveType = PrimitiveType.LineList;
            primitiveCount = 10;

            VertexPositionColor[] vertices = new VertexPositionColor[20];

            //x-axis
            vertices[0] = new VertexPositionColor(-Vector3.UnitX, Color.DarkRed);
            vertices[1] = new VertexPositionColor(Vector3.UnitX, Color.DarkRed);

            //y-axis
            vertices[2] = new VertexPositionColor(-Vector3.UnitY, Color.DarkGreen);
            vertices[3] = new VertexPositionColor(Vector3.UnitY, Color.DarkGreen);

            //z-axis
            vertices[4] = new VertexPositionColor(-Vector3.UnitZ, Color.DarkBlue);
            vertices[5] = new VertexPositionColor(Vector3.UnitZ, Color.DarkBlue);

            //to do - x-text , y-text, z-text
            //x label
            vertices[6] = new VertexPositionColor(new Vector3(1.1f, 0.1f, 0), Color.DarkRed);
            vertices[7] = new VertexPositionColor(new Vector3(1.3f, -0.1f, 0), Color.DarkRed);
            vertices[8] = new VertexPositionColor(new Vector3(1.3f, 0.1f, 0), Color.DarkRed);
            vertices[9] = new VertexPositionColor(new Vector3(1.1f, -0.1f, 0), Color.DarkRed);


            //y label
            vertices[10] = new VertexPositionColor(new Vector3(-0.1f, 1.3f, 0), Color.DarkGreen);
            vertices[11] = new VertexPositionColor(new Vector3(0, 1.2f, 0), Color.DarkGreen);
            vertices[12] = new VertexPositionColor(new Vector3(0.1f, 1.3f, 0), Color.DarkGreen);
            vertices[13] = new VertexPositionColor(new Vector3(-0.1f, 1.1f, 0), Color.DarkGreen);

            //z label
            vertices[14] = new VertexPositionColor(new Vector3(0, 0.1f, 1.1f), Color.DarkBlue);
            vertices[15] = new VertexPositionColor(new Vector3(0, 0.1f, 1.3f), Color.DarkBlue);
            vertices[16] = new VertexPositionColor(new Vector3(0, 0.1f, 1.1f), Color.DarkBlue);
            vertices[17] = new VertexPositionColor(new Vector3(0, -0.1f, 1.3f), Color.DarkBlue);
            vertices[18] = new VertexPositionColor(new Vector3(0, -0.1f, 1.3f), Color.DarkBlue);
            vertices[19] = new VertexPositionColor(new Vector3(0, -0.1f, 1.1f), Color.DarkBlue);


            return vertices;
        }

        //returns the vertices for a simple sphere (i.e. 3 circles) with a user-defined radius and sweep angle
        public static VertexPositionColor[] GetVerticesPositionColorSphere(int radius, 
            int sweepAngleInDegrees, out PrimitiveType primitiveType,
                                                            out int primitiveCount)
        {
            List<VertexPositionColor> vertexList = new List<VertexPositionColor>();

            //get vertices for each plane e.g. XY, XZ
            VertexPositionColor[] verticesSinglePlane = GetVerticesPositionColorCircle(radius, sweepAngleInDegrees, OrientationType.XYAxis);
            AddArrayElementsToList<VertexPositionColor>(verticesSinglePlane, vertexList);

            verticesSinglePlane = GetVerticesPositionColorCircle(radius, sweepAngleInDegrees, OrientationType.XZAxis);
            AddArrayElementsToList<VertexPositionColor>(verticesSinglePlane, vertexList);

            primitiveType = PrimitiveType.LineStrip;
            primitiveCount = vertexList.Count - 1;

            return vertexList.ToArray();

        }

        //Adds the contents of a list to an array
        public static void AddArrayElementsToList<T>(T[] array, List<T> list)
        {
            foreach (T obj in array)
                list.Add(obj);
        }

        //returns the vertices for a circle with a user-defined radius, sweep angle, and orientation
        public static VertexPositionColor[] GetVerticesPositionColorCircle(int radius,
            int sweepAngleInDegrees, OrientationType orientationType)
        {
            //sweep angle should also be >= 1 and a multiple of 360
            //if angle is not a multiple of 360 the circle will not close - remember we are drawing with a LineStrip
            if ((sweepAngleInDegrees < 1) || (360 % sweepAngleInDegrees != 0))
                return null;

            //number of segments forming the circle (e.g. for sweepAngleInDegrees == 90 we have 4 segments)
            int segmentCount = 360 / sweepAngleInDegrees;

            //segment angle
            float rads = MathHelper.ToRadians(sweepAngleInDegrees);

            //we need one more vertex to close the circle (e.g. 4 + 1 vertices to draw four lines)
            VertexPositionColor[] vertices = new VertexPositionColor[segmentCount + 1];

            float a, b;

            for (int i = 0; i < vertices.Length; i++)
            {
                //round the values so we dont end up with the two oordinate values very close to but not equals to 0
                a = (float)(radius * Math.Round(Math.Cos(rads * i), ROUND_PRECISION_FLOAT));
                b = (float)(radius * Math.Round(Math.Sin(rads * i), ROUND_PRECISION_FLOAT));

                if (orientationType == OrientationType.XYAxis)
                    vertices[i] = new VertexPositionColor(new Vector3(a, b, 0), Color.White);
                else if (orientationType == OrientationType.XZAxis)
                    vertices[i] = new VertexPositionColor(new Vector3(a, 0, b), Color.White);
                else
                    vertices[i] = new VertexPositionColor(new Vector3(0, a, b), Color.White);
            }

            return vertices;
        }

        public static VertexPositionColor[] GetVerticesPositionColorQuad(int sidelength, 
            out PrimitiveType primitiveType, out int primitiveCount)
        {
            primitiveType = PrimitiveType.TriangleStrip;
            primitiveCount = 2;

            VertexPositionColor[] vertices = new VertexPositionColor[4];

            float halfSideLength = sidelength / 2.0f;

            Vector3 topLeft = new Vector3(-halfSideLength, halfSideLength, 0);
            Vector3 topRight = new Vector3(halfSideLength, halfSideLength, 0);
            Vector3 bottomLeft = new Vector3(-halfSideLength, -halfSideLength, 0);
            Vector3 bottomRight = new Vector3(halfSideLength, -halfSideLength, 0);

            //quad coplanar with the XY-plane (i.e. forward facing normal along UnitZ)
            vertices[0] = new VertexPositionColor(topLeft, Color.White);
            vertices[1] = new VertexPositionColor(topRight, Color.White);
            vertices[2] = new VertexPositionColor(bottomLeft, Color.White);
            vertices[3] = new VertexPositionColor(bottomRight, Color.White);

            return vertices;
        }

        //returns the vertices for a billboard which has a custom vertex declaration
        public static VertexBillboard[] GetVertexBillboard(int sidelength)
        {
            VertexBillboard[] vertices = new VertexBillboard[4];
            float halfSideLength = sidelength / 2.0f;

             Vector2 uvTopLeft = new Vector2(0, 0);
            Vector2 uvTopRight = new Vector2(1, 0);
            Vector2 uvBottomLeft = new Vector2(0, 1);
            Vector2 uvBottomRight = new Vector2(1, 1);

            //quad coplanar with the XY-plane (i.e. forward facing normal along UnitZ)
            vertices[0] = new VertexBillboard(Vector3.Zero, new Vector4(uvTopLeft, -halfSideLength, halfSideLength));
            vertices[1] = new VertexBillboard(Vector3.Zero, new Vector4(uvTopRight, halfSideLength, halfSideLength));
            vertices[2] = new VertexBillboard(Vector3.Zero, new Vector4(uvBottomLeft, -halfSideLength, -halfSideLength));
            vertices[3] = new VertexBillboard(Vector3.Zero, new Vector4(uvBottomRight, halfSideLength, -halfSideLength));

            return vertices;
        }

        public static VertexPositionColor[] GetVerticesPositionColorCube(int sidelength)
        {
            VertexPositionColor[] vertices = new VertexPositionColor[36];

            float halfSideLength = sidelength / 2.0f;

            Vector3 topLeftFront = new Vector3(-halfSideLength, halfSideLength, halfSideLength);
            Vector3 topLeftBack = new Vector3(-halfSideLength, halfSideLength, -halfSideLength);
            Vector3 topRightFront = new Vector3(halfSideLength, halfSideLength, halfSideLength);
            Vector3 topRightBack = new Vector3(halfSideLength, halfSideLength, -halfSideLength);

            Vector3 bottomLeftFront = new Vector3(-halfSideLength, -halfSideLength, halfSideLength);
            Vector3 bottomLeftBack = new Vector3(-halfSideLength, -halfSideLength, -halfSideLength);
            Vector3 bottomRightFront = new Vector3(halfSideLength, -halfSideLength, halfSideLength);
            Vector3 bottomRightBack = new Vector3(halfSideLength, -halfSideLength, -halfSideLength);

            //top - 1 polygon for the top
            vertices[0] = new VertexPositionColor(topLeftFront, Color.White);
            vertices[1] = new VertexPositionColor(topLeftBack, Color.White);
            vertices[2] = new VertexPositionColor(topRightBack, Color.White);

            vertices[3] = new VertexPositionColor(topLeftFront, Color.White);
            vertices[4] = new VertexPositionColor(topRightBack, Color.White);
            vertices[5] = new VertexPositionColor(topRightFront, Color.White);

            //front
            vertices[6] = new VertexPositionColor(topLeftFront, Color.White);
            vertices[7] = new VertexPositionColor(topRightFront, Color.White);
            vertices[8] = new VertexPositionColor(bottomLeftFront, Color.White);

            vertices[9] = new VertexPositionColor(bottomLeftFront, Color.White);
            vertices[10] = new VertexPositionColor(topRightFront, Color.White);
            vertices[11] = new VertexPositionColor(bottomRightFront, Color.White);

            //back
            vertices[12] = new VertexPositionColor(bottomRightBack, Color.White);
            vertices[13] = new VertexPositionColor(topRightBack, Color.White);
            vertices[14] = new VertexPositionColor(topLeftBack, Color.White);

            vertices[15] = new VertexPositionColor(bottomRightBack, Color.White);
            vertices[16] = new VertexPositionColor(topLeftBack, Color.White);
            vertices[17] = new VertexPositionColor(bottomLeftBack, Color.White);

            //left 
            vertices[18] = new VertexPositionColor(topLeftBack, Color.White);
            vertices[19] = new VertexPositionColor(topLeftFront, Color.White);
            vertices[20] = new VertexPositionColor(bottomLeftFront, Color.White);

            vertices[21] = new VertexPositionColor(bottomLeftBack, Color.White);
            vertices[22] = new VertexPositionColor(topLeftBack, Color.White);
            vertices[23] = new VertexPositionColor(bottomLeftFront, Color.White);

            //right
            vertices[24] = new VertexPositionColor(bottomRightFront, Color.White);
            vertices[25] = new VertexPositionColor(topRightFront, Color.White);
            vertices[26] = new VertexPositionColor(bottomRightBack, Color.White);

            vertices[27] = new VertexPositionColor(topRightFront, Color.White);
            vertices[28] = new VertexPositionColor(topRightBack, Color.White);
            vertices[29] = new VertexPositionColor(bottomRightBack, Color.White);

            //bottom
            vertices[30] = new VertexPositionColor(bottomLeftFront, Color.White);
            vertices[31] = new VertexPositionColor(bottomRightFront, Color.White);
            vertices[32] = new VertexPositionColor(bottomRightBack, Color.White);

            vertices[33] = new VertexPositionColor(bottomLeftFront, Color.White);
            vertices[34] = new VertexPositionColor(bottomRightBack, Color.White);
            vertices[35] = new VertexPositionColor(bottomLeftBack, Color.White);

            return vertices;
        }


        /******************************************** Textured - Quad, Cube & Pyramid ********************************************/

        public static VertexPositionColorTexture[] GetVerticesPositionColorTextureQuad(int sidelength,
            out PrimitiveType primitiveType, out int primitiveCount)
        {
            primitiveType = PrimitiveType.TriangleStrip;
            primitiveCount = 2;

            VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[4];

            float halfSideLength = sidelength / 2.0f;

            Vector3 topLeft = new Vector3(-halfSideLength, halfSideLength, 0);
            Vector3 topRight = new Vector3(halfSideLength, halfSideLength, 0);
            Vector3 bottomLeft = new Vector3(-halfSideLength, -halfSideLength, 0);
            Vector3 bottomRight = new Vector3(halfSideLength, -halfSideLength, 0);

            //quad coplanar with the XY-plane (i.e. forward facing normal along UnitZ)
            vertices[0] = new VertexPositionColorTexture(topLeft, Color.White, Vector2.Zero);
            vertices[1] = new VertexPositionColorTexture(topRight, Color.White, Vector2.UnitX);
            vertices[2] = new VertexPositionColorTexture(bottomLeft, Color.White, Vector2.UnitY);
            vertices[3] = new VertexPositionColorTexture(bottomRight, Color.White, Vector2.One);

            return vertices;
        }


        public static VertexPositionColorTexture[] GetVerticesPositionTexturedCube(int sidelength)
        {
            VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[36];

            float halfSideLength = sidelength / 2.0f;

            Vector3 topLeftFront = new Vector3(-halfSideLength, halfSideLength, halfSideLength);
            Vector3 topLeftBack = new Vector3(-halfSideLength, halfSideLength, -halfSideLength);
            Vector3 topRightFront = new Vector3(halfSideLength, halfSideLength, halfSideLength);
            Vector3 topRightBack = new Vector3(halfSideLength, halfSideLength, -halfSideLength);

            Vector3 bottomLeftFront = new Vector3(-halfSideLength, -halfSideLength, halfSideLength);
            Vector3 bottomLeftBack = new Vector3(-halfSideLength, -halfSideLength, -halfSideLength);
            Vector3 bottomRightFront = new Vector3(halfSideLength, -halfSideLength, halfSideLength);
            Vector3 bottomRightBack = new Vector3(halfSideLength, -halfSideLength, -halfSideLength);

            //uv coordinates
            Vector2 uvTopLeft = new Vector2(0, 0);
            Vector2 uvTopRight = new Vector2(1, 0);
            Vector2 uvBottomLeft = new Vector2(0, 1);
            Vector2 uvBottomRight = new Vector2(1, 1);


            //top - 1 polygon for the top
            vertices[0] = new VertexPositionColorTexture(topLeftFront, Color.White, uvBottomLeft);
            vertices[1] = new VertexPositionColorTexture(topLeftBack, Color.White, uvTopLeft);
            vertices[2] = new VertexPositionColorTexture(topRightBack, Color.White, uvTopRight);

            vertices[3] = new VertexPositionColorTexture(topLeftFront, Color.White, uvBottomLeft);
            vertices[4] = new VertexPositionColorTexture(topRightBack, Color.White, uvTopRight);
            vertices[5] = new VertexPositionColorTexture(topRightFront, Color.White, uvBottomRight);

            //front
            vertices[6] = new VertexPositionColorTexture(topLeftFront, Color.White, uvBottomLeft);
            vertices[7] = new VertexPositionColorTexture(topRightFront, Color.White, uvBottomRight);
            vertices[8] = new VertexPositionColorTexture(bottomLeftFront, Color.White, uvTopLeft);

            vertices[9] = new VertexPositionColorTexture(bottomLeftFront, Color.White, uvTopLeft);
            vertices[10] = new VertexPositionColorTexture(topRightFront, Color.White, uvBottomRight);
            vertices[11] = new VertexPositionColorTexture(bottomRightFront, Color.White, uvTopRight);

            //back
            vertices[12] = new VertexPositionColorTexture(bottomRightBack, Color.White, uvBottomRight);
            vertices[13] = new VertexPositionColorTexture(topRightBack, Color.White, uvTopRight);
            vertices[14] = new VertexPositionColorTexture(topLeftBack, Color.White, uvTopLeft);

            vertices[15] = new VertexPositionColorTexture(bottomRightBack, Color.White, uvBottomRight);
            vertices[16] = new VertexPositionColorTexture(topLeftBack, Color.White, uvTopLeft);
            vertices[17] = new VertexPositionColorTexture(bottomLeftBack, Color.White, uvBottomLeft);

            //left 
            vertices[18] = new VertexPositionColorTexture(topLeftBack, Color.White, uvTopLeft);
            vertices[19] = new VertexPositionColorTexture(topLeftFront, Color.White, uvTopRight);
            vertices[20] = new VertexPositionColorTexture(bottomLeftFront, Color.White, uvBottomRight);

            vertices[21] = new VertexPositionColorTexture(bottomLeftBack, Color.White, uvBottomLeft);
            vertices[22] = new VertexPositionColorTexture(topLeftBack, Color.White, uvTopLeft);
            vertices[23] = new VertexPositionColorTexture(bottomLeftFront, Color.White, uvBottomRight);

            //right
            vertices[24] = new VertexPositionColorTexture(bottomRightFront, Color.White, uvBottomLeft);
            vertices[25] = new VertexPositionColorTexture(topRightFront, Color.White, uvTopLeft);
            vertices[26] = new VertexPositionColorTexture(bottomRightBack, Color.White, uvBottomRight);

            vertices[27] = new VertexPositionColorTexture(topRightFront, Color.White, uvTopLeft);
            vertices[28] = new VertexPositionColorTexture(topRightBack, Color.White, uvTopRight);
            vertices[29] = new VertexPositionColorTexture(bottomRightBack, Color.White, uvBottomRight);

            //bottom
            vertices[30] = new VertexPositionColorTexture(bottomLeftFront, Color.White, uvTopLeft);
            vertices[31] = new VertexPositionColorTexture(bottomRightFront, Color.White, uvTopRight);
            vertices[32] = new VertexPositionColorTexture(bottomRightBack, Color.White, uvBottomRight);

            vertices[33] = new VertexPositionColorTexture(bottomLeftFront, Color.White, uvTopLeft);
            vertices[34] = new VertexPositionColorTexture(bottomRightBack, Color.White, uvBottomRight);
            vertices[35] = new VertexPositionColorTexture(bottomLeftBack, Color.White, uvBottomLeft);

            return vertices;
        }

        public static VertexPositionColorTexture[] GetVerticesPositionTexturedPyramidSquare(int sidelength)
        {
            VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[18];
            float halfSideLength = sidelength / 2.0f;

            Vector3 topCentre = new Vector3(0, 0.71f * sidelength, 0); //multiplier gives a pyramid where the length of the rising edges == length of the base edges
            Vector3 frontLeft = new Vector3(-halfSideLength, 0, halfSideLength);
            Vector3 frontRight = new Vector3(halfSideLength, 0, halfSideLength);
            Vector3 backLeft = new Vector3(-halfSideLength, 0, -halfSideLength);
            Vector3 backRight = new Vector3(halfSideLength, 0, -halfSideLength);

            Vector2 uvTopCentre = new Vector2(0.5f, 0);
            Vector2 uvTopLeft = new Vector2(0, 0);
            Vector2 uvTopRight = new Vector2(1, 0);
            Vector2 uvBottomLeft = new Vector2(0, 1);
            Vector2 uvBottomRight = new Vector2(1, 1);

            //front 
            vertices[0] = new VertexPositionColorTexture(topCentre, Color.White, uvTopCentre);
            vertices[1] = new VertexPositionColorTexture(frontRight, Color.White, uvBottomRight);
            vertices[2] = new VertexPositionColorTexture(frontLeft, Color.White, uvBottomLeft);

            //left 
            vertices[3] = new VertexPositionColorTexture(topCentre, Color.White, uvTopCentre);
            vertices[4] = new VertexPositionColorTexture(frontLeft, Color.White, uvBottomRight);
            vertices[5] = new VertexPositionColorTexture(backLeft, Color.White, uvBottomLeft);

            //right 
            vertices[6] = new VertexPositionColorTexture(topCentre, Color.White, uvTopCentre);
            vertices[7] = new VertexPositionColorTexture(backRight, Color.White, uvBottomRight);
            vertices[8] = new VertexPositionColorTexture(frontRight, Color.White, uvBottomLeft);

            //back 
            vertices[9] = new VertexPositionColorTexture(topCentre, Color.White, uvTopCentre);
            vertices[10] = new VertexPositionColorTexture(backLeft, Color.White, uvBottomRight);
            vertices[11] = new VertexPositionColorTexture(backRight, Color.White, uvBottomLeft);

            //bottom 
            vertices[12] = new VertexPositionColorTexture(frontLeft, Color.White, uvTopLeft);
            vertices[13] = new VertexPositionColorTexture(frontRight, Color.White, uvTopRight);
            vertices[14] = new VertexPositionColorTexture(backLeft, Color.White, uvBottomLeft);

            vertices[15] = new VertexPositionColorTexture(frontRight, Color.White, uvTopRight);
            vertices[16] = new VertexPositionColorTexture(backRight, Color.White, uvBottomRight);
            vertices[17] = new VertexPositionColorTexture(backLeft, Color.White, uvBottomLeft);

            return vertices;
        }

     
    }
}
