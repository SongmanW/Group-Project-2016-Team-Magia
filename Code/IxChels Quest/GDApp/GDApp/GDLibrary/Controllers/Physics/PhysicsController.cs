using Microsoft.Xna.Framework;
using JigLibX.Physics;
using System.Collections.Generic;

/*
 * http://www.assembla.com/code/tomahawkengine/subversion/nodes/branches/prerelease/Engine/Tomahawk.Runtime/Physics/JigLibX/PhysicsScene.cs
 *
 * Amended: NMCG
 * Purpose: Sums all forces and torques for all moveable bodies in the world
 */
namespace GDLibrary
{
    public class PhysicsController : JigLibX.Physics.Controller
    {
        #region Properties
        public enum CoordinateSystem
        {
            WorldCoordinates = 0,
            LocalCoordinates = 1
        }

        public struct Force
        {
            public CoordinateSystem coordinateSystem;
            public Vector3 force;
            public Vector3 position;
            public Body body;
        }

        public struct Torque
        {
            public CoordinateSystem coordinateSystem;
            public Vector3 torque;
            public Body body;
        }

        public Queue<Force> forces = new Queue<Force>();
        internal Queue<Force> Forces { get { return forces; } }

        public Queue<Torque> torques = new Queue<Torque>();
        internal Queue<Torque> Torques { get { return torques; } }
        #endregion

        #region Inherited from Controller
        public override void UpdateController(float elapsedTime)
        {
            // Apply pending forces
            while (forces.Count > 0)
            {
                Force force = forces.Dequeue();
                switch (force.coordinateSystem)
                {
                    case CoordinateSystem.LocalCoordinates:
                        {
                            force.body.AddBodyForce(force.force, force.position);
                        } break;
                    case CoordinateSystem.WorldCoordinates:
                        {
                            force.body.AddWorldForce(force.force, force.position);
                        } break;
                }
            }



            // Apply pending torques
            while (torques.Count > 0)
            {
                Torque torque = torques.Dequeue();
                switch (torque.coordinateSystem)
                {
                    case CoordinateSystem.LocalCoordinates:
                        {
                            torque.body.AddBodyTorque(torque.torque);
                        } break;
                    case CoordinateSystem.WorldCoordinates:
                        {
                            torque.body.AddWorldTorque(torque.torque);
                        } break;
                }
            }

        }
        #endregion
    }
}