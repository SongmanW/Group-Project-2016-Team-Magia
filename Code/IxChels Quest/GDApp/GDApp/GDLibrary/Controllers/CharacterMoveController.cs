using GDApp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GDLibrary
{
    public class CharacterMoveController : Controller
    {
        private Camera3D camera;
        private Main game;

        public Camera3D Camera
        {
            set
            {
                this.camera = value;
            }
        }

        public CharacterMoveController(Main game, string name, Actor parentActor)
            : base(name, parentActor)
        {
            this.game = game;
            this.camera = null;
        }

        public CharacterMoveController(Main game, string name, Actor parentActor, Camera3D camera)
            : base(name, parentActor)
        {
            this.game = game;
            this.camera = camera;
        }

        public override void Update(GameTime gameTime)
        {
            Vector3 rotate = Vector3.Zero;
            Vector3 moveVectorX = Vector3.Zero;
            Vector3 moveVectorZ = Vector3.Zero;
            if (this.game.KeyboardManager.IsKeyDown(Keys.W))
            {
                moveVectorX = Vector3.Normalize(new Vector3(this.camera.Transform3D.Look.X, 0, this.camera.Transform3D.Look.Z));
                this.ParentActor.Transform3D.TranslateBy(moveVectorX,
                    0.1f * gameTime.ElapsedGameTime.Milliseconds);
            }
            else if (this.game.KeyboardManager.IsKeyDown(Keys.S))
            {
                moveVectorX = -Vector3.Normalize(new Vector3(this.camera.Transform3D.Look.X, 0, this.camera.Transform3D.Look.Z));
                this.ParentActor.Transform3D.TranslateBy(moveVectorX,
                    0.1f * gameTime.ElapsedGameTime.Milliseconds);
            }

            if (this.game.KeyboardManager.IsKeyDown(Keys.A))
            {
                moveVectorZ = -Vector3.Normalize(new Vector3(this.camera.Transform3D.Right.X, 0, this.camera.Transform3D.Right.Z));
                this.ParentActor.Transform3D.TranslateBy(moveVectorZ,
                    0.1f * gameTime.ElapsedGameTime.Milliseconds);
            }
            else if (game.KeyboardManager.IsKeyDown(Keys.D))
            {
                moveVectorZ = Vector3.Normalize(new Vector3(this.camera.Transform3D.Right.X, 0, this.camera.Transform3D.Right.Z));
                this.ParentActor.Transform3D.TranslateBy(moveVectorZ,
                    0.1f * gameTime.ElapsedGameTime.Milliseconds);
            }
            if (moveVectorX != Vector3.Zero || moveVectorZ != Vector3.Zero)
            {
                Vector3 moveVector = Vector3.Normalize(moveVectorX + moveVectorZ);
                if (moveVector.Z >= 0)
                {
                    this.ParentActor.Transform3D.RotateTo(new Vector3(0, 180 - MathHelper.ToDegrees((float)Math.Acos(moveVector.X)), 0));
                }
                else
                {
                    this.ParentActor.Transform3D.RotateTo(new Vector3(0, 180 + MathHelper.ToDegrees((float)Math.Acos(moveVector.X)), 0));
                }
            }
        }

    }
}
