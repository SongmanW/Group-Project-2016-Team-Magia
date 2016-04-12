using GDApp;
using JigLibX.Collision;
using JigLibX.Geometry;
using JigLibX.Math;
using JigLibX.Physics;
using Microsoft.Xna.Framework;

namespace GDLibrary
{
    /// <summary>
    /// Represents an area that can detect collisions. It does NOT have an associated model.
    /// We can use this class to create activation zones e.g. for camera switching or event generation
    /// </summary>
    public class ZoneObject : Actor
    {
        #region Variables
        private Body body;
        private CollisionSkin collision;
        private float mass;
        #endregion

        #region Properties
        public CollisionSkin Collision
        {
            get
            {
                return collision;
            }
            set
            {
                collision = value;
            }
        }
        public Body Body
        {
            get
            {
                return body;
            }
            set
            {
                body = value;
            }
        }
        #endregion

        public ZoneObject(string id, ObjectType objectType, Transform3D transform)
            : base(id, objectType, transform)
        {
            //set body and skin for this zone
            this.body = new Body();
            this.body.ExternalData = this;
            this.collision = new CollisionSkin(this.body);
            this.body.CollisionSkin = this.collision;
        }

        //Adds a primitive to this zone. Notice that material properties are irrelevant since the zone will generate any forces on the intersecting body.
        public void AddPrimitive(Primitive primitive)
        {
            this.collision.AddPrimitive(primitive, (int)MaterialTable.MaterialID.NormalNormal);
        }

        public void Enable()
        {
            //mass is irrelevant since a zone is basically an invisible area used for detecting player(s)
            this.mass = 1;

            //zones dont move
            this.body.Immovable = true;

            //calculate the centre of mass
            Vector3 com = SetMass(mass);
            //adjust skin so that it corresponds to the 3D mesh as drawn on screen
            this.body.MoveTo(this.Transform3D.Translation, this.Transform3D.Orientation);
            //set the centre of mass
            this.collision.ApplyLocalTransform(new Transform(-com, Matrix.Identity));
            //enable so that any applied forces (e.g. gravity) will affect the object
            this.body.EnableBody();
        }

        protected Vector3 SetMass(float mass)
        {
            PrimitiveProperties primitiveProperties = new PrimitiveProperties(PrimitiveProperties.MassDistributionEnum.Solid, PrimitiveProperties.MassTypeEnum.Density, mass);

            float junk;
            Vector3 com;
            Matrix it, itCoM;

            this.collision.GetMassProperties(primitiveProperties, out junk, out com, out it, out itCoM);
            body.BodyInertia = itCoM;
            body.Mass = junk;
            return com;
        }
    }
}
