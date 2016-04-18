using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDApp.GDLibrary
{
    //No vertex or index buffer
    public class UnbufferedCubeHelper : DrawableGameComponent
    {
        private BasicEffect effect;
        private Main game;
        private VertexPositionColor[] vertices;
        private Matrix world;
        private RasterizerState rasterizerState;
        private Color color;

        public UnbufferedCubeHelper(Main game,
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
            this.vertices = new VertexPositionColor[24];
            float halfLength = 0.5f;

            //line 1
            this.vertices[0]
                = new VertexPositionColor(
                    new Vector3(-halfLength, halfLength, halfLength), this.color);
            this.vertices[1]
               = new VertexPositionColor(
                   new Vector3(halfLength, halfLength, halfLength), this.color);

            //line 3
            this.vertices[2]
                = new VertexPositionColor(
                    new Vector3(-halfLength, halfLength, -halfLength), this.color);
            this.vertices[3]
               = new VertexPositionColor(
                   new Vector3(halfLength, halfLength, -halfLength), this.color);

            //line 9
            this.vertices[4]
                = new VertexPositionColor(
                    new Vector3(-halfLength, -halfLength, halfLength), this.color);
            this.vertices[5]
               = new VertexPositionColor(
                   new Vector3(halfLength, -halfLength, halfLength), this.color);

            //line 11
            this.vertices[6]
                = new VertexPositionColor(
                    new Vector3(-halfLength, -halfLength, -halfLength), this.color);
            this.vertices[7]
               = new VertexPositionColor(
                   new Vector3(halfLength, -halfLength, -halfLength), this.color);

            //2
            this.vertices[8]
               = new VertexPositionColor(
                   new Vector3(halfLength, halfLength, halfLength), this.color);
            this.vertices[9]
               = new VertexPositionColor(
                   new Vector3(halfLength, halfLength, -halfLength), this.color);

            //4
            this.vertices[10]
               = new VertexPositionColor(
                   new Vector3(-halfLength, halfLength, halfLength), this.color);
            this.vertices[11]
               = new VertexPositionColor(
                   new Vector3(-halfLength, halfLength, -halfLength), this.color);

            //10
            this.vertices[12]
               = new VertexPositionColor(
                   new Vector3(halfLength, -halfLength, halfLength), this.color);
            this.vertices[13]
               = new VertexPositionColor(
                   new Vector3(halfLength, -halfLength, -halfLength), this.color);

            //12
            this.vertices[14]
               = new VertexPositionColor(
                   new Vector3(-halfLength, -halfLength, halfLength), this.color);
            this.vertices[15]
               = new VertexPositionColor(
                   new Vector3(-halfLength, -halfLength, -halfLength), this.color);

            //6
            this.vertices[16]
               = new VertexPositionColor(
                   new Vector3(halfLength, halfLength, halfLength), this.color);
            this.vertices[17]
               = new VertexPositionColor(
                   new Vector3(halfLength, -halfLength, halfLength), this.color);

            //5
            this.vertices[18]
               = new VertexPositionColor(
                   new Vector3(-halfLength, halfLength, halfLength), this.color);
            this.vertices[19]
               = new VertexPositionColor(
                   new Vector3(-halfLength, -halfLength, halfLength), this.color);

            //7
            this.vertices[20]
               = new VertexPositionColor(
                   new Vector3(halfLength, halfLength, -halfLength), this.color);
            this.vertices[21]
               = new VertexPositionColor(
                   new Vector3(halfLength, -halfLength, -halfLength), this.color);

            //8
            this.vertices[22]
               = new VertexPositionColor(
                   new Vector3(-halfLength, halfLength, -halfLength), this.color);
            this.vertices[23]
               = new VertexPositionColor(
                   new Vector3(-halfLength, -halfLength, -halfLength), this.color);



          
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


            this.effect.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, this.vertices, 0, 12);
        }

    }
}
