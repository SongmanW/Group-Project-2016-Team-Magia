using GDApp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDLibrary
{
    public class UIActor : IActor, IUpdateable
    {
        public static Main game;

        #region Fields
        private string id;
        private ObjectType objectType;
        private Transform2D transform;
        private Color color;
        private SpriteEffects spriteEffects;
        private float layerDepth;
        private bool isVisible;
        #endregion

        #region Properties
        public bool IsVisible
        {
            get
            {
                return this.isVisible;
            }
            set
            {
                this.isVisible = value;
            }

        }
        public float LayerDepth
        {
            get
            {
                return this.layerDepth;
            }
            set
            {
                this.layerDepth = value;
            }
        }
        public SpriteEffects SpriteEffects
        {
            get
            {
                return this.spriteEffects;
            }
            set
            {
                this.spriteEffects = value;
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
            }
        }
        public string ID
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }
        public ObjectType ObjectType
        {
            get
            {
                return this.objectType;
            }
            set
            {
                this.objectType = value;
            }
        }
        public Transform2D Transform2D
        {
            get
            {
                return this.transform;
            }
            set
            {
                this.transform = value;
            }
        }
        public Matrix World
        {
            get
            {
                return this.transform.World;
            }
        }

        #endregion

        public UIActor(string id, ObjectType objectType, Transform2D transform, 
            Color color, SpriteEffects spriteEffects, float layerDepth, bool isVisible)
        {
            this.id = id;
            this.objectType = objectType;
            this.transform = transform;

            this.color = color;
            this.spriteEffects = spriteEffects;
            this.layerDepth = layerDepth;

            this.isVisible = isVisible;
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(GameTime gameTime)
        {

        }

        public virtual Matrix GetWorldMatrix()
        {
            return this.transform.World;

        }

        public virtual void Remove()
        {
            //tag for garbage collection
            this.transform = null;
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
