using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDApp.GDLibrary
{
    public class OriginHelper : DrawableGameComponent  
    {
        private BasicEffect effect;
        private Main game;
        private VertexPositionColor[] vertices;
        private Matrix world;
        private RasterizerState rasterizerState;

        public OriginHelper(Main game, 
            BasicEffect effect, RasterizerState rasterizerState,
            Vector3 translation, Vector3 rotation, Vector3 scale)
            : base(game)
        {
            this.game = game;
            this.effect = effect;
            this.rasterizerState = rasterizerState;

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
            this.vertices = new VertexPositionColor[6];
            float halfLength = 0.5f;

            //x-axis
            this.vertices[0]
                = new VertexPositionColor(new Vector3(-halfLength, 0, 0),
                    Color.Red);

            this.vertices[1]
              = new VertexPositionColor(new Vector3(halfLength, 0, 0),
                  Color.Red);

            //y-axis
            this.vertices[2]
              = new VertexPositionColor(new Vector3(0, halfLength, 0),
                  Color.Green);

            this.vertices[3]
              = new VertexPositionColor(new Vector3(0, -halfLength, 0),
                  Color.Green);

            //z-axis
            this.vertices[4]
              = new VertexPositionColor(new Vector3(0, 0, halfLength),
                  Color.Blue);

            this.vertices[5]
              = new VertexPositionColor(new Vector3(0, 0, -halfLength),
                  Color.Blue);
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


            this.effect.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, this.vertices, 0, 3);
        }

    }
}
