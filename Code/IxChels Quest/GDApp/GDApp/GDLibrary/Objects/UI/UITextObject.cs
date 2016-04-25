using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDLibrary
{
    public class UITextObject : UIActor
    {
        #region Fields
        private string text;
        private SpriteFont spriteFont;
        #endregion

        #region Properties
        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = (value.Length >= 0) ? value : "Default";
            }
        }
        public SpriteFont SpriteFont
        {
            get
            {
                return this.spriteFont;
            }
            set
            {
                this.spriteFont = value;
            }
        }
        #endregion

        public UITextObject(string id, ObjectType objectType, Transform2D transform,
            Color color, SpriteEffects spriteEffects, float layerDepth, string text, SpriteFont spriteFont, bool isVisible)
            : base(id, objectType, transform, color, spriteEffects, layerDepth, isVisible)
        {
            this.spriteFont = spriteFont;
            this.text = text;
        }

        public override void Draw(GameTime gameTime)
        {
            //if (game.MouseManager.Bounds.Intersects(this.Transform2D.Bounds))
            //    this.Color = Color.Green;
            //else
            //{
            //    this.Color = Color.White;
            //}

            game.SpriteBatch.DrawString(this.spriteFont, this.text, this.Transform2D.Translation, this.Color, this.Transform2D.Rotation,
                this.Transform2D.Origin, this.Transform2D.Scale, this.SpriteEffects, this.LayerDepth);
        }
    }
}
