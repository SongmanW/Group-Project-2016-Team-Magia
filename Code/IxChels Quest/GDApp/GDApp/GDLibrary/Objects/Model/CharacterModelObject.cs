using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GDLibrary
{
    public class CharacterModelObject : ModelObject
    {
        private Camera3D camera;

        public Camera3D Camera
        {
            set
            {
                this.camera = value;
            }
        }

        public CharacterModelObject(string id, ObjectType objectType, Transform3D transform,
            Texture2D texture, Model model)
            : base(id, objectType, transform, texture, model)
        {
            this.camera = null;
        }

        public CharacterModelObject(string id, ObjectType objectType, Transform3D transform,
            Texture2D texture, Model model, Camera3D camera)
            : base(id, objectType, transform, texture, model)
        {
            this.camera = camera;
        }

        public override void Update(GameTime gameTime)
        {
            if (game.KeyboardManager.IsKeyDown(Keys.W))
            {
                this.Transform3D.TranslateBy(Vector3.Normalize(new Vector3(this.camera.Transform3D.Look.X, 0, this.camera.Transform3D.Look.Z)),
                    0.01f * gameTime.ElapsedGameTime.Milliseconds);
            }
            else if (game.KeyboardManager.IsKeyDown(Keys.S))
            {
                this.Transform3D.TranslateBy(-Vector3.Normalize(new Vector3(this.camera.Transform3D.Look.X, 0, this.camera.Transform3D.Look.Z)),
                    0.01f * gameTime.ElapsedGameTime.Milliseconds);
            }

            if (game.KeyboardManager.IsKeyDown(Keys.A))
            {
                this.Transform3D.TranslateBy(-Vector3.Normalize(new Vector3(this.camera.Transform3D.Right.X, 0, this.camera.Transform3D.Right.Z)),
                    0.01f * gameTime.ElapsedGameTime.Milliseconds);
            }
            else if (game.KeyboardManager.IsKeyDown(Keys.D))
            {
                this.Transform3D.TranslateBy(Vector3.Normalize(new Vector3(this.camera.Transform3D.Right.X, 0, this.camera.Transform3D.Right.Z)),
                    0.01f * gameTime.ElapsedGameTime.Milliseconds);
            }



            base.Update(gameTime);
        }

    }
}
