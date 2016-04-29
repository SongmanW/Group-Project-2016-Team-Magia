/*
Function: 		Provide mouse input functions
Author: 		NMCG
Version:		1.0
Date Updated:	26/1/16
Bugs:			None
Fixes:			None
*/


using GDApp;
using JigLibX.Collision;
using JigLibX.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GDLibrary
{
    class ImmovableSkinPredicate : CollisionSkinPredicate1
    {
        public override bool ConsiderSkin(CollisionSkin skin0)
        {
            if (skin0.Owner != null)
                return true;
            else
                return false;
        }
    }

    /// <summary>
    /// Provides methods to determine the state of the mouse.
    /// </summary>
    public class MouseManager : GameComponent
    {
        #region Variables
        private Main game;
        private MouseState newState, oldState;
        #endregion

        private static float MouseSensitivity = 1;
        #region Properties
        public bool IsVisible
        {
            get
            {
                return game.IsMouseVisible;
            }
            set
            {
                game.IsMouseVisible = value;
            }
        }
        public Vector2 Position
        {
            get
            {
                return new Vector2(this.newState.X, this.newState.Y);
            }
        }
        public Microsoft.Xna.Framework.Rectangle Bounds
        {
            get
            {
                return new Microsoft.Xna.Framework.Rectangle(this.newState.X, this.newState.Y, 1, 1);
            }
        }
        #endregion

        public MouseManager(Main game, bool isVisible)
            : base(game)
        {

            this.game = game;
            game.IsMouseVisible = isVisible;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }
     
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //store the old state
            this.oldState = newState;

            //get the new state
            this.newState = Mouse.GetState();

            base.Update(gameTime);
        }

        public bool HasMoved()
        {
            float deltaPositionLength = new Vector2(newState.X - oldState.X, 
                newState.Y - oldState.Y).Length();

            return (deltaPositionLength > MouseSensitivity) ? true : false;
        }
        public bool IsLeftButtonClickedOnce()
        {
            return ((newState.LeftButton.Equals(ButtonState.Pressed)) && (oldState.LeftButton.Equals(ButtonState.Released)));
        }
        public bool IsLeftButtonClicked()
        {
            return (newState.LeftButton.Equals(ButtonState.Pressed));
        }
        public bool IsRightButtonClickedOnce()
        {
            return ((newState.RightButton.Equals(ButtonState.Pressed)) && (oldState.RightButton.Equals(ButtonState.Pressed)));
        }
        public bool isRightButtonClicked()
        {
            return (newState.RightButton.Equals(ButtonState.Pressed));
        }

        //Calculates the mouse pointer distance (in X and Y) from a user-defined position
        public Vector2 GetDeltaFromPosition(Vector2 position)
        {
            return new Vector2(this.newState.X - position.X, 
            this.newState.Y - position.Y);
        }

        //has the mouse state changed since the last update?
        public bool IsStateChanged()
        {
            return (this.newState.Equals(oldState)) ? false : true;
        }

        //how much has the scroll wheel been moved since the last update?
        public int GetDeltaFromScrollWheel()
        {
            if (IsStateChanged()) //if state changed then return difference
                return newState.ScrollWheelValue - oldState.ScrollWheelValue;

            return 0;
        }

        public void SetPosition(Vector2 position)
        {
            Mouse.SetPosition((int)position.X, (int)position.Y);
        }

        //get a ray positioned at the mouse's location on the screen - used for picking 
        public Microsoft.Xna.Framework.Ray GetMouseRay(Camera3D camera)
        {
            //get the positions of the mouse in screen space
            Vector3 near = new Vector3(this.newState.X, this.Position.Y, 0);

            //convert from screen space to world space
            near = camera.Viewport.Unproject(near, camera.ProjectionParameters.Projection, camera.View, Matrix.Identity);

            return GetMouseRayFromNearPosition(camera, near);
        }

        //get a ray from a user-defined near position in world space and the mouse pointer
        public Microsoft.Xna.Framework.Ray GetMouseRayFromNearPosition(Camera3D camera, Vector3 near)
        {
            //get the positions of the mouse in screen space
            Vector3 far = new Vector3(this.newState.X, this.Position.Y, 1);

            //convert from screen space to world space
            far = camera.Viewport.Unproject(far, camera.ProjectionParameters.Projection, camera.View, Matrix.Identity);

            //generate a ray to use for intersection tests
            return new Microsoft.Xna.Framework.Ray(near, Vector3.Normalize(far - near));
        }

        public Vector3 GetMouseRayDirection(Camera3D camera)
        {
            //get the positions of the mouse in screen space
            Vector3 near = new Vector3(this.newState.X, this.Position.Y, 0);
            Vector3 far = new Vector3(this.newState.X, this.Position.Y, 1);

            //convert from screen space to world space
            near = camera.Viewport.Unproject(near, camera.ProjectionParameters.Projection, camera.View, Matrix.Identity);
            far = camera.Viewport.Unproject(far, camera.ProjectionParameters.Projection, camera.View, Matrix.Identity);

            //generate a ray to use for intersection tests
            return far - near;
        }

        float frac; CollisionSkin skin;
        public Actor GetPickedObject(Camera3D camera, float distance, out Vector3 pos, out Vector3 normal)
        {
            Vector3 ray = GetMouseRayDirection(camera);
            ImmovableSkinPredicate pred = new ImmovableSkinPredicate();

            this.game.PhysicsManager.PhysicsSystem.CollisionSystem.SegmentIntersect(out frac, out skin, out pos, out normal,
                new Segment(camera.Transform3D.Translation, ray * distance), pred);

            if (skin != null && skin.Owner != null)
            {
                return skin.Owner.ExternalData as Actor;
            }

            return null;
        }

        //used when in 1st person collidable camera mode
        //start distance allows us to start the ray outside the collidable skin of the 1st person colliable camera object
        //otherwise the only thing we would ever collide with would be ourselves!
        public Actor GetPickedObject(Camera3D camera, float startDistance, float distance,
                   out Vector3 pos, out Vector3 normal)
        {
            Vector3 ray = GetMouseRayDirection(camera);
            ImmovableSkinPredicate pred = new ImmovableSkinPredicate();

            this.game.PhysicsManager.PhysicsSystem.CollisionSystem.SegmentIntersect(
                out frac, out skin, out pos, out normal,
                new Segment(camera.Transform3D.Translation + startDistance * Vector3.Normalize(ray), ray * distance), pred);

            if (skin != null && skin.Owner != null)
            {
                return skin.Owner.ExternalData as Actor;
            }

            return null;
        }
    }
}