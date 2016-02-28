using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GDLibrary
{
    public class SecurityController : Controller
    {
        #region Fields
        private int maxSweepAngle;
        private int sweepSpeed;
        #endregion

        #region Properties
        #endregion

        public SecurityController(string name, Actor parentActor,
            int maxSweepAngle, int sweepSpeed)
            : base(name, parentActor)
        {
            this.maxSweepAngle = maxSweepAngle;
            this.sweepSpeed = sweepSpeed;
        }
        public override void Update(GameTime gameTime)
        {
            float sinOfTime = (float)Math.Sin(MathHelper.ToRadians(this.sweepSpeed * (float)gameTime.TotalGameTime.TotalSeconds));
            float rotationAngle = sinOfTime * this.maxSweepAngle;
            this.ParentActor.Transform3D.RotateBy(Vector3.UnitX * rotationAngle);
            base.Update(gameTime);
        }

        public override object Clone()
        {
            return new SecurityController("clone - " + this.Name,
                this.ParentActor,
                this.maxSweepAngle, //deep - primitive - copy by value
                this.sweepSpeed); //deep - primitive - copy by value

            
        }
    }
}
