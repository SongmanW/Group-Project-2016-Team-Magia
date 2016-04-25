
using JigLibX.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDLibrary
{
    /// <summary>
    /// Represents an area for camera switching 
    /// </summary>
    public class CameraZoneObject : ZoneObject
    {
        private string cameraID, cameraLayout;
        #region Fields
        #endregion

        #region Properties
        private string CameraID
        {
            get
            {
                return this.cameraID;
            }
            set
            {
                this.cameraID = value;
            }
        }
        private string CameraLayout
        {
            get
            {
                return this.cameraLayout;
            }
            set
            {
                this.cameraLayout = value;
            }
        }

        #endregion

        public CameraZoneObject(string id, ObjectType objectType, Transform3D transform,
           Effect effect, Color color, float alpha,
           bool isImpenetrable, string cameraLayout, string cameraID)
           : base(id, objectType, transform, effect, color, alpha, isImpenetrable)
        {
            this.cameraLayout = cameraLayout;
            this.cameraID = cameraID;

            this.Collision.callbackFn += Collision_callbackFn;
        }

        public bool Collision_callbackFn(CollisionSkin collider, CollisionSkin collidee)
        {
            if (collidee.Owner.ExternalData is PlayerObject)
            {
                //change Camera
                EventDispatcher.Publish(new CameraEventData(this.ID, this, EventType.OnCameraChanged, this.cameraLayout, this.cameraID));
            }

            this.Body.DisableBody();
            this.Remove();

            return this.IsImpenetrable;
        }
    }
}
