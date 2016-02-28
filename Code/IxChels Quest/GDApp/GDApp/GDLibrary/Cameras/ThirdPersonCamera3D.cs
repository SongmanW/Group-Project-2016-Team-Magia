using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GDLibrary
{
    public class ThirdPersonCamera3D : Camera3D
    {
        private Actor targetActor;
        private int elevationAngle;
        private int distance;
        private Vector3 oldTranslation;
        private Vector3 oldLook;
        private Vector3 oldUp;
        private float lerpSpeed;

        public ThirdPersonCamera3D(string id, ObjectType objectType, Transform3D transform,
            ProjectionParameters projectionParameters,
            Viewport viewPort, Actor targetActor,
            int elevationAngle, int distance)
            : base(id, objectType, transform, projectionParameters, viewPort)
        {
            this.targetActor = targetActor;
            this.elevationAngle = elevationAngle;
            this.distance = distance;

            this.lerpSpeed = 0.05f;// med = 0.1f, fast = 0.2f
            this.oldTranslation = this.Transform3D.Translation;
            this.oldLook = this.Transform3D.Look;
            this.oldUp = this.Transform3D.Up;
        }
        public override void Update(GameTime gameTime)
        {
            //rotate the target look around the target right to get a vector pointing away from the target at a specified elevation
            Vector3 cameraToTarget 
                = Vector3.Transform(-this.Transform3D.Look, 
                Matrix.CreateFromAxisAngle(this.Transform3D.Right, this.elevationAngle));


            //normalize to give unit length, otherwise distance from camera to target will vary over time
            cameraToTarget.Normalize();

            //set the position of the camera to be a set distance from target and at certain elevation angle
            this.Transform3D.Translation = Vector3.Lerp(this.oldTranslation,
                cameraToTarget * this.distance + this.Transform3D.Translation, lerpSpeed);

            //set the camera to have the same orientation as the target object
            this.Transform3D.Look = Vector3.Lerp(this.oldLook, this.Transform3D.Look, lerpSpeed);
            this.Transform3D.Up = Vector3.Lerp(this.oldUp, this.Transform3D.Up, lerpSpeed);

            this.oldTranslation = this.Transform3D.Translation;
            this.oldLook = this.Transform3D.Look;
            this.oldUp = this.Transform3D.Up;

   
            /*
            //get to get the target look and reverse the look and transform around target right
            Vector3 targetToCameraLook = Vector3.Transform(
                -this.targetActor.Transform3D.Look, 
                Matrix.CreateFromAxisAngle(
                this.targetActor.Transform3D.Right, 
                MathHelper.ToRadians(this.elevationAngle)));

            //nmultiply by camera distance
            targetToCameraLook.Normalize();
            
            //add this to target position
            this.Transform3D.Translation
                = this.targetActor.Transform3D.Translation
                    + targetToCameraLook * this.distance;

            //set the camera look
            this.Transform3D.Look = -targetToCameraLook;
            */


            base.Update(gameTime);
        }
    }
}
