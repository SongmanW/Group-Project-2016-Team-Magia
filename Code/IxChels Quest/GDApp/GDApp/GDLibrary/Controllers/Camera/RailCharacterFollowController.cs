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

        public RailCharacterFollowController(string name, Actor parentActor, RailParameters railParameters, Actor targetActor, Vector3 look, float distanceToTarget)
            : base(name, parentActor)
        {
            this.railParameters = railParameters;
            this.targetActor = targetActor;
            this.distanceToTarget = distanceToTarget;

            //put the camera on the rail mid point
            this.ParentActor.Transform3D.Translation = railParameters.Start;
            //look along Rail all the time
            this.ParentActor.Transform3D.Look = Vector3.Normalize(look);
        }

        public override void Update(GameTime gameTime)
        {
            //define target (new target is targetActor - Camera.Look*distance )
            Vector3 target = this.targetActor.Transform3D.Translation - Vector3.Normalize(this.railParameters.End - this.railParameters.Start) * distanceToTarget;

            Vector3 cameraToTarget = CameraUtility.GetCameraToTarget(target, this.ParentActor.Transform3D);

            //new position for camera if it is positioned between start and the end points of the rail
            Vector3 projectedCameraPosition = this.ParentActor.Transform3D.Translation
                + Vector3.Dot(cameraToTarget, railParameters.Look) * railParameters.Look * gameTime.ElapsedGameTime.Milliseconds * 0.5f;

            //do not allow the camera to move outside the rail
            if (!railParameters.InsideRail(projectedCameraPosition))
            {
                //change Camera
                
            }

            this.ParentActor.Transform3D.Translation = projectedCameraPosition;

            Console.WriteLine("Camera Translation: " + this.ParentActor.Transform3D.Translation);

            base.Update(gameTime);
        }
    }
}
