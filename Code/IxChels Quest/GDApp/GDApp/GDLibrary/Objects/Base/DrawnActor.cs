using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace GDLibrary
{
    public class DrawnActor : Actor, IUpdateable
    {
        #region Fields
        private Effect effect;
        private Color color, originalColor;
        private Vector3 colorAsVector3;
        private float alpha, originalAlpha;
        #endregion

        #region Properties   
        public Effect Effect
        {
            get
            {
                return this.effect;
            }
            set
            {
                this.effect = value;
            }
        }
        public Vector3 ColorAsVector3
        {
            get
            {
                return this.colorAsVector3;
            }
        }
        public Color Color
        {
            get
            {
                return this.color;
            }
            set
            {
                this.color = value;
                this.colorAsVector3 = this.color.ToVector3();
            }
        }
        public Color OriginalColor
        {
            get
            {
                return this.originalColor;
            }
            set
            {
                this.originalColor = value;
            }
        }
        public float OriginalAlpha
        {
            get
            {
                return this.originalAlpha;
            }
            set
            {
                this.originalAlpha = ((value >= 0) && (value <= 1))
                    ? value : 1;
            }
        }
        public float Alpha
        {
            get
            {
                return this.alpha;
            }
            set
            {
                this.alpha = ((value >= 0) && (value <= 1))
                    ? value : 1;
            }
        }
        #endregion

        public DrawnActor(string id, ObjectType objectType, Transform3D transform, Effect effect, Color color, float alpha)
            : base(id, objectType, transform)
        {
            this.Effect = effect;
            this.Color = color;
            this.Alpha = alpha;
        }

        public virtual void Draw(GameTime gameTime)
        {

        }

        public override void Remove()
        {
            //remove from object manager
            game.ObjectManager.Remove(this);

            //nullify any expensive objects
            base.Remove();
        }

        public bool Enabled
        {
            get { return this.Enabled;  }
        }

        public event System.EventHandler<System.EventArgs> EnabledChanged;

        public int UpdateOrder
        {
            get { return this.UpdateOrder; }
        }

        public event System.EventHandler<System.EventArgs> UpdateOrderChanged;
    }
}
