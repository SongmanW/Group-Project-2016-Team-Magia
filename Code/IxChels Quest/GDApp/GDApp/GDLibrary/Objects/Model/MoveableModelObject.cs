using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GDLibrary
{
    public class MoveableModelObject : ModelObject
    {
        public MoveableModelObject(string id, ObjectType objectType, Transform3D transform, Effect effect,
            Texture2D texture, Model model, Color color, float alpha)
            : base(id, objectType, transform, effect, texture, model, color, alpha)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if(game.KeyboardManager.IsKeyDown(Keys.W))
            {
                this.Transform3D.TranslateBy(this.Transform3D.Look,
                    0.01f * gameTime.ElapsedGameTime.Milliseconds);
            }
            else if (game.KeyboardManager.IsKeyDown(Keys.S))
            {
                this.Transform3D.TranslateBy(-this.Transform3D.Look,
                    0.01f * gameTime.ElapsedGameTime.Milliseconds);
            }

            if (game.KeyboardManager.IsKeyDown(Keys.A))
            {
                float rot = gameTime.ElapsedGameTime.Milliseconds * 0.1f;
                this.Transform3D.RotateAroundYBy(rot);
            }
            else if (game.KeyboardManager.IsKeyDown(Keys.D))
            {
                float rot = gameTime.ElapsedGameTime.Milliseconds * 0.1f;
                this.Transform3D.RotateAroundYBy(-rot);
            }



            base.Update(gameTime);
        }

    }
}
