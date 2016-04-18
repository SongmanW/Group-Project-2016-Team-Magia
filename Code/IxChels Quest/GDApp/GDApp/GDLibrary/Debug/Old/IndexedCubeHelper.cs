using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDApp.GDLibrary
{
    //Vertex and index buffer
    public class IndexedCubeHelper : DrawableGameComponent
    {
        private BasicEffect effect;
        private Main game;
        private VertexPositionColor[] vertices;
        private Matrix world;
        private RasterizerState rasterizerState;
        private VertexBuffer vertexBuffer;
        private IndexBuffer indexBuffer;
        private Color color;

        public IndexedCubeHelper(Main game,
            BasicEffect effect, RasterizerState rasterizerState,
            Vector3 translation, Vector3 rotation, Vector3 scale, Color color)
            : base(game)
        {
            this.game = game;
            this.effect = effect;
            this.rasterizerState = rasterizerState;
            this.color = color;

            this.world = Matrix.Identity
                * Matrix.CreateScale(scale)
                    * Matrix.CreateRotationX(rotation.X)
                        * Matrix.CreateRotationY(rotation.Y)
                            * Matrix.CreateRotationZ(rotation.Z)
                                * Matrix.CreateTranslation(translation);

        }

        public override void Initialize()
        {
            IntialiseVertices();
            base.Initialize();
        }

        private void IntialiseVertices()
        {
            int vertexCount = 8;
            int indexCount = 24;

            this.vertices = new VertexPositionColor[vertexCount];
            float halfLength = 0.5f;

            Color color = Color.White;

            #region Top Points
            //point 0
            this.vertices[0]
                = new VertexPositionColor(
                    new Vector3(halfLength, halfLength, halfLength), this.color);
            //point 1
            this.vertices[1]
               = new VertexPositionColor(
                   new Vector3(halfLength, halfLength, -halfLength), this.color);

            //point 2
            this.vertices[2]
                = new VertexPositionColor(
                    new Vector3(-halfLength, halfLength, -halfLength), this.color);
            //point 3
            this.vertices[3]
               = new VertexPositionColor(
                   new Vector3(-halfLength, halfLength, halfLength), this.color);
            #endregion

            #region Bottom Points
            //point 4
            this.vertices[4]
                = new VertexPositionColor(
                    new Vector3(halfLength, -halfLength, halfLength), this.color);
            //point 5
            this.vertices[5]
               = new VertexPositionColor(
                   new Vector3(halfLength, -halfLength, -halfLength), this.color);

            //point 6
            this.vertices[6]
                = new VertexPositionColor(
                    new Vector3(-halfLength, -halfLength, -halfLength), this.color);
            //point 7
            this.vertices[7]
               = new VertexPositionColor(
                   new Vector3(-halfLength, -halfLength, halfLength), this.color);
            #endregion



            //reserve space
            this.vertexBuffer = new VertexBuffer(game.GraphicsDevice,
                typeof(VertexPositionColor), vertexCount, BufferUsage.WriteOnly);

            //send the data to the space i.e. to VRAM
            this.vertexBuffer.SetData<VertexPositionColor>(this.vertices);

            this.indexBuffer = new IndexBuffer(game.GraphicsDevice,
                IndexElementSize.SixteenBits, indexCount, BufferUsage.WriteOnly);

            ushort[] indices = new ushort[indexCount];
            //TOP SQUARE
            indices[0] = 0; //line right, front to back
            indices[1] = 1;

            indices[2] = 1; //line going right to left, back
            indices[3] = 2;

            indices[4] = 2; //line left, back to front
            indices[5] = 3;

            indices[6] = 3; //line going left to right, front
            indices[7] = 0;

            //VERTICAL LINES DOWN
            indices[8] = 0; //down
            indices[9] = 4;

            indices[10] = 1; //down
            indices[11] = 5;

            indices[12] = 2; //down
            indices[13] = 6;

            indices[14] = 3; //down
            indices[15] = 7;

            //BOTTOM SQUARE
            indices[16] = 4; //line right, front to back
            indices[17] = 5;

            indices[18] = 5; //line going right to left, back
            indices[19] = 6;

            indices[20] = 6; //line left, back to front
            indices[21] = 7;

            indices[22] = 7; //line going left to right, front
            indices[23] = 4;

            this.indexBuffer.SetData<ushort>(indices);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            DrawVertices();
            base.Draw(gameTime);
        }

        private void DrawVertices()
        {
            this.effect.GraphicsDevice.RasterizerState = this.rasterizerState;
            // this.effect.VertexColorEnabled = true;

            this.effect.World = this.world;

            //draw with reference to the camera
            this.effect.View = this.game.CameraManager.ActiveCamera.View;
            this.effect.Projection = this.game.CameraManager.ActiveCamera.ProjectionParameters.Projection;

            //load the GFX card with the W, V, and P values
            this.effect.CurrentTechnique.Passes[0].Apply();

            //use the data from the vertex buffer reserved, and set, earlier in Initialise()
            this.effect.GraphicsDevice.SetVertexBuffer(this.vertexBuffer);
            //use the data from the index buffer reserved, and set, earlier in Initialise()
            this.effect.GraphicsDevice.Indices = this.indexBuffer;

            //slight change to draw
            this.effect.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.LineList, 
                0, 0, this.vertexBuffer.VertexCount, 0, 12);
        }

    }
}
