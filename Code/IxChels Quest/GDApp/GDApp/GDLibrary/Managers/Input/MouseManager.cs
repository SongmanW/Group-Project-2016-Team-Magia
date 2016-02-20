/*
Function: 		Provide mouse input functions
Author: 		NMCG
Version:		1.0
Date Updated:	26/1/16
Bugs:			None
Fixes:			None
*/


using GDApp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GDLibrary
{
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
        public bool IsLeftButtonClickedOnce(MouseState mouseState)
        {
            return ((newState.LeftButton.Equals(ButtonState.Pressed)) && (oldState.LeftButton.Equals(ButtonState.Pressed)));
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
    }
}