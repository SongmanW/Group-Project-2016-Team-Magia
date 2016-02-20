using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GDLibrary
{
    public class FreeLookCamera3D : Camera3D
    {
        private float speed;

        public FreeLookCamera3D(string id, ObjectType objectType, Transform3D transform, 
            ProjectionParameters projectionParameters, Viewport viewPort, float speed)
            : base(id, objectType, transform, projectionParameters, viewPort)
        {
            this.speed = speed;
        }
        public override void Update(GameTime gameTime)
        {
            HandleKeyboardInput(gameTime);
            HandleMouseInput(gameTime);
            base.Update(gameTime);
        }

        private void HandleMouseInput(GameTime gameTime)
        {
            Vector2 mouseDelta = game.MouseManager.GetDeltaFromPosition(game.ScreenCentre);
            mouseDelta *= gameTime.ElapsedGameTime.Milliseconds;
            mouseDelta *= 0.01f;

            this.Transform3D.RotateBy(new Vector3(-mouseDelta, 0));

        }

        private void HandleKeyboardInput(GameTime gameTime)
        {
            float speedMultiplier = this.speed * gameTime.ElapsedGameTime.Milliseconds;

            if (game.KeyboardManager.IsKeyDown(Keys.Left))
            {
                this.Transform3D.TranslateBy(-this.Transform3D.Right, speedMultiplier);
            }
            else if (game.KeyboardManager.IsKeyDown(Keys.Right))
            {
                this.Transform3D.TranslateBy(this.Transform3D.Right, speedMultiplier);
            }
            if (game.KeyboardManager.IsKeyDown(Keys.Up))
            {
                this.Transform3D.TranslateBy(this.Transform3D.Look, speedMultiplier);
            }
            else if (game.KeyboardManager.IsKeyDown(Keys.Down))
            {
                this.Transform3D.TranslateBy(-this.Transform3D.Look, speedMultiplier);
            }
        }

        public override object Clone()
        {
            //if we don't override clone then the cameras cloned in Main will be cloned as simple camera3D objects with no movement
            return new FreeLookCamera3D(this.ID,
                this.ObjectType, (Transform3D)this.Transform3D.Clone(), (ProjectionParameters)this.ProjectionParameters.Clone(), this.Viewport, this.speed);
        } 

    }
}
