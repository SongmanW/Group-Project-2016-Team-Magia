using JigLibX.Physics;
using Microsoft.Xna.Framework;
using JigLibX.Collision;
using Microsoft.Xna.Framework.Graphics;
using JigLibX.Geometry;
using JigLibX.Math;

namespace GDLibrary
{
    /// <summary>
    /// Provides a parent class representing a player (or non-player) character
    /// with appropriate collision skin (i.e. capsule) and move rates (e.g. acceleration)
    /// 
    /// Inherit from this class to create a specific player (e.g. PlayerObject extends CharacterObject)
    /// then set Body::velocity and Transform3D::rotation to move and turn the character
    /// - See PlayerObject
    /// </summary>
    public class CharacterObject : CollidableObject
    {
        public Character CharacterBody { get; set; }

        public CharacterObject(string id, ObjectType objectType, Transform3D transform, 
            Texture2D texture, Model model, Color color, float alpha,
            float radius, float height, float accelerationRate, float decelerationRate)
            : base(id, objectType, transform, texture, model, color, alpha)
        {
            this.Body = new Character(accelerationRate, decelerationRate);
            this.Collision = new CollisionSkin(Body);
            this.Body.ExternalData = this;
            this.Body.CollisionSkin = this.Collision;
            Capsule capsule = new Capsule(Vector3.Zero, Matrix.CreateRotationX(MathHelper.PiOver2), radius, height);
            this.Collision.AddPrimitive(capsule, (int)MaterialTable.MaterialID.NormalSmooth);
        }


        public override void Enable(bool bImmovable, float mass)
        {
            base.Enable(bImmovable, mass);
            Body.SetBodyInvInertia(0.0f, 0.0f, 0.0f);
            CharacterBody = Body as Character;
            Body.AllowFreezing = false;
            Body.EnableBody();
        }

        public override void Update(GameTime gameTime)
        {
            //update actual position of the model e.g. used by rail camera controllers
            this.Transform3D.Translation = this.Body.Transform.Position;
            base.Update(gameTime);
        }

        public override Matrix GetWorldMatrix()
        {
            return Matrix.CreateScale(this.Transform3D.Scale) *
                this.Collision.GetPrimitiveLocal(0).Transform.Orientation *
                this.Body.Orientation *
                this.Transform3D.Orientation * 
                Matrix.CreateTranslation(this.Body.Position);
        }
    }

    class ASkinPredicate : CollisionSkinPredicate1
    {
        public override bool ConsiderSkin(CollisionSkin skin0)
        {
            if (!(skin0.Owner is Character))
                return true;
            else
                return false;
        }
    }

    public class Character : Body
    {
        private bool doJump;
        private float jumpHeight=5;

        public float accelerationRate { get; set; }
        public float decelerationRate { get; set; }
        public Vector3 DesiredVelocity { get; set; }

        public Character(float accelerationRate, float decelerationRate)
            : base()
        {
            this.accelerationRate = accelerationRate;
            this.decelerationRate = decelerationRate;
        }

        public void DoJump(float jumpHeight)
        {
            this.jumpHeight = jumpHeight;
            doJump = true;
        }

        public override void AddExternalForces(float dt)
        {
            ClearForces();

            if (doJump)
            {
                foreach (CollisionInfo info in CollisionSkin.Collisions)
                {
                    Vector3 N = info.DirToBody0;
                    if (this == info.SkinInfo.Skin1.Owner)
                        Vector3.Negate(ref N, out N);

                    if (Vector3.Dot(N, Orientation.Up) > 0.7f)
                    {
                        Vector3 vel = Velocity;
                        vel.Y = jumpHeight;
                        Velocity = vel;
                        break;
                    }
                }
            }

            Vector3 deltaVel = DesiredVelocity - Velocity;

            bool running = true;

            if (DesiredVelocity.LengthSquared() < JiggleMath.Epsilon)
                running = false;
            else
                deltaVel.Normalize();

            deltaVel.Y = -2.0f;

            // start fast, slow down slower
            if (running)
                deltaVel *= accelerationRate; //acceleration multiplier
            else
                deltaVel *= decelerationRate;  //deceleration multiplier

            float forceFactor = 500.0f;
            AddBodyForce(deltaVel * Mass * dt * forceFactor);
            doJump = false;
            AddGravityToExternalForce();
        }
    }

}
