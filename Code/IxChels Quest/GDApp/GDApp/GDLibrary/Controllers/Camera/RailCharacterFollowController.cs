using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GDLibrary
{
    public class RailCharacterFollowController : Controller
    {
        private float distanceToTarget;
        private RailParameters railParameters;
        private Actor targetActor;

        public RailCharacterFollowController(string name, Actor parentActor, bool bEnabled, RailParameters railParameters, Actor targetActor, float distanceToTarget)
            : base(name, parentActor, bEnabled)
        {
            this.railParameters = railParameters;
            this.targetActor = targetActor;
            this.distanceToTarget = distanceToTarget;

            //put the camera on the rail mid point
            this.ParentActor.Transform3D.Translation = railParameters.Start;
        }

        public override void Update(GameTime gameTime)
        {
            //define target (new target is targetActor - Camera.Look*distance )
            Vector3 target = this.targetActor.Transform3D.Translation - railParameters.Look * distanceToTarget;

            Vector3 cameraToTarget = CameraUtility.GetCameraToTarget(target, this.ParentActor.Transform3D);

            //new position for camera if it is positioned between start and the end points of the rail
            Vector3 projectedCameraPosition = this.ParentActor.Transform3D.Translation
                + Vector3.Dot(cameraToTarget, railParameters.Look) * railParameters.Look * gameTime.ElapsedGameTime.Milliseconds * 0.5f;

            //do not allow the camera to move outside the rail
            if (railParameters.InsideRail(projectedCameraPosition))
            {
                //change Camera
                this.ParentActor.Transform3D.Translation = projectedCameraPosition;

            }

            Vector3 calcLook = Vector3.Lerp(Vector3.Normalize(cameraToTarget), railParameters.Look, 0.6f);
            this.ParentActor.Transform3D.Look = new Vector3(
                calcLook.X,
                MathHelper.Lerp(this.ParentActor.Transform3D.Look.Y, railParameters.Look.Y, 0.2f),
                calcLook.Z);

            base.Update(gameTime);
        }
    }
}
