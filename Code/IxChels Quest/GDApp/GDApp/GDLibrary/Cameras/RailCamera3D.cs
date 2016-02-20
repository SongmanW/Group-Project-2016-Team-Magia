using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GDLibrary
{
    public class RailCamera3D : Camera3D
    {
        private RailParameters railParameters;
        private Actor targetActor;

        public RailCamera3D(string id, ObjectType objectType,
            Transform3D transform, ProjectionParameters projectionParameters, 
            Viewport viewPort, RailParameters railParameters, Actor targetActor)
            : base(id, objectType, transform, projectionParameters, viewPort)
        {
            this.railParameters = railParameters;
            this.targetActor = targetActor;

            //put the camera on the rail mid point
            this.Transform3D.Translation = railParameters.MidPoint;
        }

        public override void Update(GameTime gameTime)
        {

            //get look vector to target
            Vector3 cameraToTarget = CameraUtility.GetCameraToTarget(this.targetActor.Transform3D, this.Transform3D);

            //new position for camera if it is positioned between start and the end points of the rail
            Vector3 projectedCameraPosition = this.Transform3D.Translation
                + Vector3.Dot(cameraToTarget, railParameters.Look) * railParameters.Look * gameTime.ElapsedGameTime.Milliseconds;

            //do not allow the camera to move outside the rail
            if (railParameters.InsideRail(projectedCameraPosition))
                this.Transform3D.Translation = projectedCameraPosition;

            //set the camera to look at the object
            this.Transform3D.Look = cameraToTarget;

            //bug - set the up vector???

            base.Update(gameTime);
        }



    }
}
