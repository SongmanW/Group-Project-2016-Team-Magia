using Microsoft.Xna.Framework;

namespace GDLibrary
{
    public class RailController : Controller
    {
        #region Fields
        private RailParameters railParameters;
        private Actor targetActor;
        #endregion

        #region Properties
        #endregion

        public RailController(string name, Actor parentActor, 
            RailParameters railParameters, Actor targetActor)
            : base(name, parentActor)
        {
            this.railParameters = railParameters;
            this.targetActor = targetActor;

            //set the initial position of the camera
            this.ParentActor.Transform3D.Translation = railParameters.MidPoint;
        }

        public override void Update(GameTime gameTime)
        {
            //get look vector to target
            Vector3 cameraToTarget = CameraUtility.GetCameraToTarget(this.targetActor.Transform3D, this.ParentActor.Transform3D);

            //new position for camera if it is positioned between start and the end points of the rail
            Vector3 projectedCameraPosition = this.ParentActor.Transform3D.Translation
                + Vector3.Dot(cameraToTarget, railParameters.Look) * railParameters.Look * gameTime.ElapsedGameTime.Milliseconds;

            //do not allow the camera to move outside the rail
            if (railParameters.InsideRail(projectedCameraPosition))
                this.ParentActor.Transform3D.Translation = projectedCameraPosition;

            //set the camera to look at the object
            this.ParentActor.Transform3D.Look = cameraToTarget;

            //bug - set the up vector???
        }


        public override object Clone()
        {
            return new RailController("clone - " + this.Name,
                this.ParentActor, //shallow - reset normally
                this.railParameters, //shallow - change to deep - add Clone()
                this.targetActor); //shallow - cloned rail should have same target
        }
    }
}
