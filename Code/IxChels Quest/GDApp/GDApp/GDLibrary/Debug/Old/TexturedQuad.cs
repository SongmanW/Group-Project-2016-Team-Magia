using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GDApp;
using Microsoft.Xna.Framework.Graphics;

namespace GDLibrary
{
    public class TexturedQuad : DrawableGameComponent
    {
        private Texture2D texture;
        private VertexPositionColorTexture[] vertices;
        private BasicEffect effect;
        private Main game;
        private RasterizerState rasterizerState;
        private Color color;
        private Matrix world;

        public TexturedQuad(Main game,
            Texture2D texture, BasicEffect effect, RasterizerState rasterizerState,
            Vector3 translation, Vector3 rotation, Vector3 scale, Color color)
            : base(game)
        {
            this.game = game;
            this.effect = effect;
            this.texture = texture;
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
            InitializeVertices();
            base.Initialize();
        }

        private void InitializeVertices()
        {
            float halfLength = 0.5f;

            Vector3 topLeft = new Vector3(-halfLength, halfLength, 0);
            Vector3 topRight = new Vector3(halfLength, halfLength, 0);
            Vector3 bottomLeft = new Vector3(-halfLength, -halfLength, 0);
            Vector3 bottomRight = new Vector3(halfLength, -halfLength, 0);

            //quad coplanar with the XY-plane (i.e. forward facing normal along UnitZ)
            this.vertices = new VertexPositionColorTexture[4];
            this.vertices[0] = new VertexPositionColorTexture(topLeft, Color.White, Vector2.Zero);
            this.vertices[1] = new VertexPositionColorTexture(topRight, Color.White, Vector2.UnitX);
            this.vertices[2] = new VertexPositionColorTexture(bottomLeft, Color.White, Vector2.UnitY);
            this.vertices[3] = new VertexPositionColorTexture(bottomRight, Color.White, Vector2.One);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            //allows us to support textures with transparent sections - see ther Foliage folder under Textures for trees
            this.effect.GraphicsDevice.BlendState = BlendState.AlphaBlend;

            //comment out to disable depth buffering - look at how the quads are not drawn in the correct order
            this.effect.GraphicsDevice.DepthStencilState = DepthStencilState.Default;


            this.effect.World = this.world;
            this.effect.View = this.game.ActiveCamera.View;
            this.effect.Projection = this.game.ActiveCamera.ProjectionParameters.Projection;

            this.effect.Texture = this.texture;
            this.effect.DiffuseColor = this.color.ToVector3();

            this.effect.CurrentTechnique.Passes[0].Apply();

            this.effect.GraphicsDevice.DrawUserPrimitives<
                VertexPositionColorTexture>(
                PrimitiveType.TriangleStrip, this.vertices,
                0, 2);


            base.Draw(gameTime);
        }



    }
}
