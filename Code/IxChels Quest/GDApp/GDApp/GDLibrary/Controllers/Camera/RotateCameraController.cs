using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GDLibrary
{
    public class RotateCameraController : Controller
    {
        private Vector3 offSet;
        private Transform3D oldPTransform;
        private Transform3D oldTransform;
        private Actor targetActor;

        public RotateCameraController(string name, Actor parentActor, bool bEnabled, Actor targetActor)
            :base(name, parentActor, bEnabled)
        {
            this.targetActor = targetActor;
        }

        public void Set()
        {
            this.ParentActor.Transform3D.Look = new Vector3(
                this.targetActor.Transform3D.Translation.X - this.ParentActor.Transform3D.Translation.X,
                this.ParentActor.Transform3D.Look.Y,
                this.targetActor.Transform3D.Translation.Z - this.ParentActor.Transform3D.Translation.Z);
            offSet = this.ParentActor.Transform3D.Translation - this.targetActor.Transform3D.Translation;
            oldTransform = (Transform3D)this.targetActor.Transform3D.Clone();
            oldPTransform = (Transform3D)this.ParentActor.Transform3D.Clone();


        }

        public override void Update(GameTime gameTime)
        {
            Vector3 newRotation = oldTransform.Rotation - targetActor.Transform3D.Rotation;

            Matrix rot = Matrix.CreateFromYawPitchRoll(-MathHelper.ToRadians(newRotation.Y), 
                MathHelper.ToRadians(newRotation.X), MathHelper.ToRadians(newRotation.Z));
            
            this.ParentActor.Transform3D.Translation = this.targetActor.Transform3D.Translation + Vector3.Transform(this.offSet, rot);
            this.ParentActor.Transform3D.Look = Vector3.Transform(this.oldPTransform.Look, rot);
            this.ParentActor.Transform3D.Up = Vector3.Transform(this.oldPTransform.Up, rot);
            this.ParentActor.Transform3D.Rotation = this.oldPTransform.Rotation + newRotation;
            
            base.Update(gameTime);
        }
    }
}
