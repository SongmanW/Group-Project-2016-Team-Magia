using GDApp;
using JigLibX.Collision;
using JigLibX.Geometry;
using JigLibX.Math;
using JigLibX.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GDLibrary
{
    public class AlarmZoneObject : ZoneObject
    {
        #region Variables
        #endregion

        #region Properties
   
        #endregion

        public AlarmZoneObject(string id, ObjectType objectType, Transform3D transform, Effect effect, Color color, float alpha, bool isImpenetrable)
            : base(id, objectType, transform, effect, color, alpha, isImpenetrable)
        {
            this.Collision.callbackFn += Collision_callbackFn;
        }

        public virtual bool Collision_callbackFn(CollisionSkin collider, CollisionSkin collidee)
        {
            if (collidee.Owner.ExternalData is PlayerObject)
            {
                PlayerObject playerObject = collidee.Owner.ExternalData as PlayerObject;
                //Console.WriteLine("Zone: " + playerObject.ID);

                //Event, Sound, Pickup
                //Event, ID, Remove
                //Event, UI, Bullet/Health, Increment
            }
            return this.IsImpenetrable;
        }

      
    }
}
