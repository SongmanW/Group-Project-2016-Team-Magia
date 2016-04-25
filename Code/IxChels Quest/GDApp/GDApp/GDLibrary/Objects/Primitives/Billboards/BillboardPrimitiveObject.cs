using GDApp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GDLibrary
{
    public class BillboardParameters
    {
        public BillboardType BillboardType { get; set; }
        public Vector3 Up { get; set; }
        public string Technique { get; set; }

        //scrolling
        public bool IsScrolling { get; set; }
        public Vector2 ScrollRate { get; private set; }
        public Vector2 scrollValue;

        //animation
        public bool IsAnimated { get; set; }
        public int currentFrame { get; set; }
        public int startFrame { get; set; }
        public int totalFrames { get; set; }
        public Vector2 inverseFrameCount { get; set; }
        private float frameDelay { get; set; }
        private int framesElapsed;


        public void SetScrollRate(Vector2 scrollRate)
        {
            SetScrolling(true);
            this.ScrollRate = scrollRate;
            this.scrollValue = Vector2.Zero;
        }
        public void SetScrolling(bool isScrolling)
        {
            this.IsScrolling = isScrolling;
        }

        public void SetAnimated(bool isAnimated)
        {
            this.IsAnimated = isAnimated;
        }
        public void SetAnimationRate(int totalFrames, float frameDelay, int startFrame)
        {
            SetAnimated(true);
            this.totalFrames = totalFrames; //remember frames are arranged in a 1 x N strip, so totalFrames == N
            this.inverseFrameCount = new Vector2(1.0f / totalFrames, 1);
            this.frameDelay = frameDelay;
            this.startFrame = startFrame;
            this.currentFrame = startFrame;
        }

        public void UpdateScroll(GameTime gameTime)
        {
            float invDt = 1.0f / (1000 * gameTime.ElapsedGameTime.Milliseconds);

            this.scrollValue.X += this.ScrollRate.X * invDt;
            this.scrollValue.X %= 1;

            this.scrollValue.Y += this.ScrollRate.Y * invDt;
            this.scrollValue.Y %= 1;
        }

        public void UpdateAnimation(GameTime gameTime)
        {
            int frameRate = (int)(frameDelay * 1.0f / gameTime.ElapsedGameTime.TotalSeconds);

            if(framesElapsed >= frameRate)
            {
                framesElapsed = 0;
                this.currentFrame++;
                this.currentFrame %= this.totalFrames;

            }
            else
            {
                framesElapsed++;
            }
        }

        public void ResetAnimation()
        {
            this.framesElapsed = 0;
            this.currentFrame = this.startFrame;
        }
        public void ResetScroll()
        {
            this.scrollValue = Vector2.Zero;
        }
    }

    public class BillboardPrimitiveObject : TexturedPrimitiveObject, ICloneable
    {
        #region Variables
        private BillboardParameters billboardParameters;
        private BillboardType billBoardType;
        #endregion

        #region Properties
        public BillboardType BillboardType
        {
            get
            {
                return this.billBoardType;
            }
            set
            {
                this.billBoardType = value;
            }
        }
        public BillboardParameters BillboardParameters
        {
            get
            {
                return this.billboardParameters;
            }
        }
        #endregion

        //used to draw billboards that have a texture 
        public BillboardPrimitiveObject(string id, ObjectType objectType,
            Transform3D transform, IVertexData vertexData, Effect effect, Color color, float alpha, Texture2D texture, BillboardType billBoardType)
            : base(id, objectType, transform, vertexData, effect, color, alpha, texture)
        {
            this.billBoardType = billBoardType;
            this.billboardParameters = new BillboardParameters();
            if (billBoardType == BillboardType.Normal)
            {
                this.billboardParameters.Technique = "Normal";
                this.billboardParameters.BillboardType = BillboardType.Normal;
            }
            else if (billBoardType == BillboardType.Cylindrical)
            {
                this.billboardParameters.Technique = "Cylindrical";
                this.billboardParameters.BillboardType = BillboardType.Cylindrical;
                this.billboardParameters.Up = this.Transform3D.Up;
            }
            else
            {
                this.billboardParameters.Technique = "Spherical";
                this.billboardParameters.BillboardType = BillboardType.Spherical;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if(this.billboardParameters.IsScrolling)
                this.billboardParameters.UpdateScroll(gameTime);

            if (this.billboardParameters.IsAnimated)
                this.billboardParameters.UpdateAnimation(gameTime);

            base.Update(gameTime);
        }

        public new object Clone()
        {
            return new BillboardPrimitiveObject("clone - " + this.ID, this.ObjectType,
            (Transform3D)this.Transform3D.Clone(), this.VertexData, this.Effect,
            this.Color, this.Alpha, this.Texture, this.billBoardType);
        }
    }
}
