using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GDLibrary
{
    public class RailCharacterFollowCamera3D : Camera3D
    {
        private float distanceToTarget;
        private RailParameters railParameters;
        private Actor targetActor;

        public RailCharacterFollowCamera3D(string id, ObjectType objectType,
            Transform3D transform, ProjectionParameters projectionParameters,
            Viewport viewPort, RailParameters railParameters, Actor targetActor, Vector3 look, float distanceToTarget)
            : base(id, objectType, transform, projectionParameters, viewPort)
        {
            this.railParameters = railParameters;
            this.targetActor = targetActor;
            this.distanceToTarget = distanceToTarget;

            //put the camera on the rail mid point
            this.Transform3D.Translation = railParameters.MidPoint;
            //look along Rail all the time
            this.Transform3D.Look = Vector3.Normalize(look);
        }

        public override void Update(GameTime gameTime)
        {
            //define target (new target is targetActor - Camera.Look*distance )
            Vector3 target = this.targetActor.Transform3D.Translation - Vector3.Normalize(this.railParameters.End - this.railParameters.Start) * distanceToTarget;

            Vector3 cameraToTarget = CameraUtility.GetCameraToTarget(target, this.Transform3D);

            //new position for camera if it is positioned between start and the end points of the rail
            Vector3 projectedCameraPosition = this.Transform3D.Translation
                + Vector3.Dot(cameraToTarget, railParameters.Look) * railParameters.Look * gameTime.ElapsedGameTime.Milliseconds * 0.5f;

            //do not allow the camera to move outside the rail
            if (!railParameters.InsideRail(projectedCameraPosition))
            {
                //change Camera
            }

            this.Transform3D.Translation = projectedCameraPosition;

            //bug - set the up vector???

            base.Update(gameTime);
        }



    }
}
