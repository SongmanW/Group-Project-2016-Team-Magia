using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDLibrary
{
    public class UITextureObject : UIActor
    {
        #region Fields
        private Texture2D texture;
        private Rectangle sourceRectangle;
        private Vector2 origin;
        #endregion

        #region Properties
        public Vector2 Origin
        {
            get
            {
                return this.origin;
            }
            set
            {
                this.origin = value;
            }
        }
        public Rectangle SourceRectangle
        {
            get
            {
                return this.sourceRectangle;
            }
            set
            {
                this.sourceRectangle = value;
            }
        }
        public Texture2D Texture
        {
            get
            {
                return this.texture;
            }
            set
            {
                this.texture = value;
            }
        }
        #endregion

        public UITextureObject(string id, ObjectType objectType, Transform2D transform,
            Color color, SpriteEffects spriteEffects, float layerDepth, Texture2D texture, 
            Rectangle sourceRectangle, Vector2 origin, bool isVisible)
            : base(id, objectType, transform, color, spriteEffects, layerDepth, isVisible)
        {
            this.Texture = texture;
            this.SourceRectangle = sourceRectangle;
            this.Origin = origin;
        }

        //draws texture using full source rectangle with origin in centre
        public UITextureObject(string id, ObjectType objectType, Transform2D transform,
         Color color, SpriteEffects spriteEffects, float layerDepth, Texture2D texture, bool isVisible)
            : this(id, objectType, transform, color, spriteEffects, layerDepth, texture, 
                new Rectangle(0, 0, texture.Width, texture.Height), 
                    new Vector2(texture.Width/2.0f, texture.Height/2.0f), isVisible)
        {

        }


        public override void Draw(GameTime gameTime)
        {
            //if (game.MouseManager.Bounds.Intersects(this.Transform2D.Bounds))
            //    this.Color = Color.Red;
            //else
            //{
            //    this.Color = Color.White;
            //}

            game.SpriteBatch.Draw(this.texture,  this.Transform2D.Translation, this.sourceRectangle, this.Color, this.Transform2D.Rotation,
                this.Transform2D.Origin, this.Transform2D.Scale, this.SpriteEffects, this.LayerDepth);
        }
    }
}
