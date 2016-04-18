using GDApp;
using JigLibX.Collision;
using JigLibX.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GDLibrary
{
    public class PhysicsDebugDrawer : DrawableGameComponent
    {
        private Main game;
        private BasicEffect basicEffect;
        private List<VertexPositionColor> vertexData;
        private VertexPositionColor[] wf;

        public PhysicsDebugDrawer(Main game)
            : base(game)
        {
            this.game = game;
            this.vertexData = new List<VertexPositionColor>();
            this.basicEffect = new BasicEffect(game.GraphicsDevice);
        }

        public override void Draw(GameTime gameTime)
        {
            //disable this statement to see what happens to the wireframe bounding surfaces
            this.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            if (vertexData.Count == 0) return;

            this.basicEffect.AmbientLightColor = Vector3.One;
            this.basicEffect.View = this.game.CameraManager.ActiveCamera.View;
            this.basicEffect.Projection = this.game.CameraManager.ActiveCamera.ProjectionParameters.Projection;
            this.basicEffect.VertexColorEnabled = true;
            this.basicEffect.CurrentTechnique.Passes[0].Apply();
            GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, vertexData.ToArray(), 0, vertexData.Count - 1);

            vertexData.Clear();

            base.Draw(gameTime);
        }

        public void DrawShape(List<Vector3> shape, Color color)
        {
            if (vertexData.Count > 0)
            {
                Vector3 v = vertexData[vertexData.Count - 1].Position;
                vertexData.Add(new VertexPositionColor(v, new Color(0, 0, 0, 0)));
                vertexData.Add(new VertexPositionColor(shape[0], new Color(0, 0, 0, 0)));
            }

            foreach (Vector3 p in shape)
            {
                vertexData.Add(new VertexPositionColor(p, color));
            }
        }

        public void DrawShape(List<Vector3> shape, Color color, bool closed)
        {
            DrawShape(shape, color);

            Vector3 v = shape[0];
            vertexData.Add(new VertexPositionColor(v, color));
        }

        public void DrawShape(List<VertexPositionColor> shape)
        {
            if (vertexData.Count > 0)
            {
                Vector3 v = vertexData[vertexData.Count - 1].Position;
                vertexData.Add(new VertexPositionColor(v, new Color(0, 0, 0, 0)));
                vertexData.Add(new VertexPositionColor(shape[0].Position, new Color(0, 0, 0, 0)));
            }

            foreach (VertexPositionColor vps in shape)
            {
                vertexData.Add(vps);
            }
        }

        public void DrawShape(VertexPositionColor[] shape)
        {
            if (vertexData.Count > 0)
            {
                Vector3 v = vertexData[vertexData.Count - 1].Position;
                vertexData.Add(new VertexPositionColor(v, new Color(0, 0, 0, 0)));
                vertexData.Add(new VertexPositionColor(shape[0].Position, new Color(0, 0, 0, 0)));
            }

            foreach (VertexPositionColor vps in shape)
            {
                vertexData.Add(vps);
            }
        }

        public void DrawShape(List<VertexPositionColor> shape, bool closed)
        {
            DrawShape(shape);

            VertexPositionColor v = shape[0];
            vertexData.Add(v);
        }

        public void DrawDebug(Body body, CollisionSkin collision)
        {
            if (!body.CollisionSkin.GetType().Equals(typeof(JigLibX.Geometry.Plane)))
            {
                wf = collision.GetLocalSkinWireframe();

                // if the collision skin was also added to the body
                // we have to transform the skin wireframe to the body space
                if (body.CollisionSkin != null)
                {
                    body.TransformWireframe(wf);
                }

                DrawShape(wf);
            }
        }
    }
}
