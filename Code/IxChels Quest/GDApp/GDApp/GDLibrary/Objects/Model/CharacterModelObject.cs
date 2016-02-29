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
            Model model)
            : base(id, objectType, transform, null, model)
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
            Vector3 rotate = Vector3.Zero;
            Vector3 moveVectorX = Vector3.Zero;
            Vector3 moveVectorZ = Vector3.Zero;
            if (game.KeyboardManager.IsKeyDown(Keys.W))
            {
                moveVectorX = Vector3.Normalize(new Vector3(this.camera.Transform3D.Look.X, 0, this.camera.Transform3D.Look.Z));
                this.Transform3D.TranslateBy(moveVectorX,
                    0.1f * gameTime.ElapsedGameTime.Milliseconds);
            }
            else if (game.KeyboardManager.IsKeyDown(Keys.S))
            {
                moveVectorX = -Vector3.Normalize(new Vector3(this.camera.Transform3D.Look.X, 0, this.camera.Transform3D.Look.Z));
                this.Transform3D.TranslateBy(moveVectorX,
                    0.1f * gameTime.ElapsedGameTime.Milliseconds);
            }

            if (game.KeyboardManager.IsKeyDown(Keys.A))
            {
                moveVectorZ = -Vector3.Normalize(new Vector3(this.camera.Transform3D.Right.X, 0, this.camera.Transform3D.Right.Z));
                this.Transform3D.TranslateBy(moveVectorZ,
                    0.1f * gameTime.ElapsedGameTime.Milliseconds);
            }
            else if (game.KeyboardManager.IsKeyDown(Keys.D))
            {
                moveVectorZ = Vector3.Normalize(new Vector3(this.camera.Transform3D.Right.X, 0, this.camera.Transform3D.Right.Z));
                this.Transform3D.TranslateBy(moveVectorZ,
                    0.1f * gameTime.ElapsedGameTime.Milliseconds);
            }
            if (moveVectorX != Vector3.Zero || moveVectorZ != Vector3.Zero)
            {
                Vector3 moveVector = Vector3.Normalize(moveVectorX + moveVectorZ);
                if (moveVector.Z >= 0)
                {
                    this.Transform3D.RotateTo(new Vector3(0, 180-MathHelper.ToDegrees((float)Math.Acos(moveVector.X)), 0));
                }
                else
                {
                    this.Transform3D.RotateTo(new Vector3(0, 180+MathHelper.ToDegrees((float)Math.Acos(moveVector.X)), 0));
                }
            }


            base.Update(gameTime);
        }

    }
}
