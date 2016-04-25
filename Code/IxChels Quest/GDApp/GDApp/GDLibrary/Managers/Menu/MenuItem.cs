using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDLibrary
{
    public class MenuItem
    {
        #region Variables
        private String name, text;
        protected Color inactiveColor, activeColor;
        private Rectangle bounds;
        private Vector2 position;
        private Color drawColor;
        #endregion

        #region Properties
        public Rectangle Bounds
        {
            get
            {
                return bounds;
            }
            set
            {
                bounds = value;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
        }
        public string Text
        {
            get
            {
                return text;
            }
        }
        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }
        #endregion

        public MenuItem(String name, String text, 
            Rectangle bounds, Color inactiveColor, Color activeColor)
        {
            this.name = name;
            this.text = text;
            this.inactiveColor = inactiveColor;
            this.activeColor = activeColor;
            this.drawColor = inactiveColor;
            this.bounds = bounds;
            this.position = new Vector2(bounds.X, bounds.Y);
        }

        public void SetActive(bool bActive)
        {
            if (bActive)
                drawColor = activeColor;
            else
                drawColor = inactiveColor;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont menuFont)
        {
            spriteBatch.DrawString(menuFont, text, position, drawColor, 
                0, Vector2.Zero, 1, SpriteEffects.None, 0);
        }

        //Add Equals and GetHashCode to support comparison and detect duplicates
        public override bool Equals(object obj)
        {
            MenuItem other = obj as MenuItem;
            return this.Name.Equals(other.Name)
                && this.Text.Equals(other.Text)
                    && (this.Position == other.Position);
        }

        public override int GetHashCode()
        {
            int seed = 53;
            return this.Name.GetHashCode() 
                + seed * this.Text.GetHashCode() 
                    + this.position.GetHashCode();
        }
    }
}
