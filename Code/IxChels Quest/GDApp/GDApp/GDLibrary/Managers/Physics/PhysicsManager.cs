using GDApp;
using JigLibX.Collision;
using JigLibX.Physics;
using Microsoft.Xna.Framework;
using System;

namespace GDLibrary
{
    public class PhysicsManager : DrawableGameComponent
    {
        #region Variables
        private Main game;
        private PhysicsSystem physicSystem;
        private PhysicsController physCont;
        private float timeStep = 0;
        private PhysicsDebugDrawer physicsDebugDrawer;
        //pause/unpause based on menu event
        private bool bPaused = false;
        private bool bEnableDebugDrawer = true;

        //temp vars
        private DrawnActor drawnActor;

        #endregion


        #region Properties
        public bool Paused
        {
            get
            {
                return bPaused;
            }
            set
            {
                bPaused = value;
            }
        }
        public bool EnableDebugDrawer
        {
            get
            {
                return bEnableDebugDrawer;
            }
            set
            {
                bEnableDebugDrawer = value;
            }
        }
        public PhysicsSystem PhysicsSystem
        {
            get
            {
                return physicSystem;
            }
        }
        public PhysicsController PhysicsController
        {
            get
            {
                return physCont;
            }
        }
        public PhysicsDebugDrawer DebugDrawer
        {
            get
            {
                return physicsDebugDrawer;
            }
        }
        #endregion

        public PhysicsManager(Main game, bool bEnableDebugDrawer)
            : base(game)
        {
            this.game = game;
            this.bEnableDebugDrawer = bEnableDebugDrawer;
            this.physicSystem = new PhysicsSystem();

            //add cd/cr system
            this.physicSystem.CollisionSystem = new CollisionSystemSAP();
            this.physicSystem.EnableFreezing = true;
            this.physicSystem.SolverType = PhysicsSystem.Solver.Normal;
            this.physicSystem.CollisionSystem.UseSweepTests = true;
            //affect accuracy and the overhead == time required
            this.physicSystem.NumCollisionIterations = 8; //8
            this.physicSystem.NumContactIterations = 8; //8
            this.physicSystem.NumPenetrationRelaxtionTimesteps = 12; //15

            #region SETTING_COLLISION_ACCURACY
            //affect accuracy of the collision detection
            this.physicSystem.AllowedPenetration = 0.000025f;
            this.physicSystem.CollisionTollerance = 0.00005f;
            #endregion

            this.physCont = new PhysicsController();
            this.physicSystem.AddController(physCont);

            if (bEnableDebugDrawer)
            {
                this.physicsDebugDrawer = new PhysicsDebugDrawer(game);
                game.Components.Add(this.physicsDebugDrawer);
            }

        }

        public override void Initialize()
        {
            base.Initialize();
        }
        public override void Update(GameTime gameTime)
        {
            if (!bPaused)
            {
                timeStep = (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                //if the time between updates indicates a FPS of close to 60 fps or less then update CD/CR engine
                if (timeStep < 1.0f / 60.0f)
                    physicSystem.Integrate(timeStep);
                else
                    //else fix at 60 updates per second
                    physicSystem.Integrate(1.0f / 60.0f);
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (!bPaused && bEnableDebugDrawer)
            {
                for (int i = 0; i < this.game.ObjectManager.Size; i++)
                {
                    drawnActor = this.game.ObjectManager[i];

                    if (drawnActor is CollidableObject)
                    {
                        CollidableObject collidableObject = drawnActor as CollidableObject;
                        this.DebugDrawer.DrawDebug(collidableObject.Body, collidableObject.Collision);
                    }
                    else if (drawnActor is ZoneObject)
                    {
                        ZoneObject zoneObject = drawnActor as ZoneObject;
                        this.DebugDrawer.DrawDebug(zoneObject.Body, zoneObject.Collision);
                    }
                }
            }



            base.Draw(gameTime);
        }

        //to do - dispose, clone
    }
}
