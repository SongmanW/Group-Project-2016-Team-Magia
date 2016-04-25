using System;
using Microsoft.Xna.Framework;

namespace GDLibrary
{
    public class Transform2D : ICloneable
    {
        #region Fields
        private Vector2 translation, scale, originalTranslation, originalScale;
        private float rotationInDegrees, rotationInRadians, originalRotationInDegrees;
        private Vector2 origin, originalOrigin;

        private bool bBoundsDirty;
        private Matrix originMatrix, translationMatrix, rotationMatrix, scaleMatrix;

        private Rectangle bounds, originalBounds;
        private Integer2 originalDimensions;
        #endregion

        #region Properties
        protected Matrix RotationMatrix
        {
            get
            {
                return this.rotationMatrix;
            }
        }
        protected Matrix PositionMatrix
        {
            get
            {
                return this.translationMatrix;
            }
        }
        public Vector2 Translation
        {
            get
            {
                return this.translation;
            }
            set
            {
                this.translation = value;
                this.translationMatrix = Matrix.CreateTranslation(new Vector3(this.translation, 0));
                this.bBoundsDirty = true;
            }
        }
        public float Rotation
        {
            get
            {
                return this.rotationInDegrees;
            }
            set
            {
                this.rotationInDegrees = value;
                this.rotationInDegrees %= 360;
                this.rotationInRadians = MathHelper.ToRadians(rotationInDegrees);
                this.rotationMatrix = Matrix.CreateRotationZ(this.rotationInRadians);
                this.bBoundsDirty = true;
            }
        }
        public float RotationInRadians
        {
            get
            {
                return this.rotationInRadians;
            }
        }
        public Vector2 Scale
        {
            get
            {
                return this.scale;
            }
            set
            {
                //do not allow scale to go to zero
                this.scale = (value != Vector2.Zero) ? value : Vector2.One;
                this.scaleMatrix = Matrix.CreateScale(new Vector3(this.scale, 1));
                this.bBoundsDirty = true;
            }
        }
        public Vector2 Origin
        {
            get
            {
                return this.origin;
            }
            set
            {
                this.origin = value;
                this.originMatrix = Matrix.CreateTranslation(new Vector3(-origin, 0));
                this.bBoundsDirty = true;
            }
        }
        public Matrix World
        {
            get
            {
                return originMatrix * scaleMatrix * rotationMatrix * translationMatrix;
            }
        }
        public Rectangle Bounds
        {
            get
            {
                if (this.bBoundsDirty)
                {
                    this.bounds = CollisionUtility.CalculateTransformedBoundingRectangle(this.originalBounds, this.World);
                    this.bBoundsDirty = false;
                }

                return this.bounds;
            }
        }
        public Rectangle OriginalBounds
        {            
            get
            {
                return this.originalBounds;
            }
            set
            {
                this.originalBounds = value;
            }
        }
        #endregion


        //used by dynamic sprites i.e. which need a look and right vector for movement
        public Transform2D(Vector2 translation, float rotationInDegrees, Vector2 scale,
            Vector2 origin, Integer2 dimensions)
        {
            //set using properties
            Set(translation, rotationInDegrees, scale, origin, dimensions);

            //store defaults used for reset
            SetDefaults(translation, rotationInDegrees, scale, origin, dimensions);
        }


        //used by static background sprites that cover the entire screen OR more than the entire screen
        public Transform2D(Vector2 scale)
        {
            //set using properties
            Set(Vector2.Zero, 0, scale, Vector2.Zero, Integer2.Zero);

            //store defaults used for reset
            SetDefaults(Vector2.Zero, 0, scale, Vector2.Zero, Integer2.Zero);
        }

        //set all new values
        public void Set(Vector2 translation, float rotationInDegrees, Vector2 scale,
            Vector2 origin, Integer2 dimensions)
        {
            this.Translation = translation;
            this.Scale = scale;
            this.Rotation = rotationInDegrees;
            this.Origin = origin;


            //original bounding box based on the texture source rectangle dimensions
            this.originalBounds = new Rectangle(0, 0, dimensions.X, dimensions.Y);
            this.originalDimensions = dimensions;

            this.bBoundsDirty = true;
        }

        //store the original default values for any subsequent reset
        private void SetDefaults(Vector2 translation, float rotationInDegrees, Vector2 scale,
            Vector2 origin, Integer2 dimensions)
        {
            this.originalTranslation = translation;
            this.originalScale = scale;
            this.originalRotationInDegrees = rotationInDegrees;
            this.originalDimensions = dimensions;
            this.originalOrigin = origin;
        }

        //used to reset sprite to it's original transform
        public void Reset()
        {
            Set(originalTranslation, originalRotationInDegrees, originalScale, originalOrigin, originalDimensions);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }


}
