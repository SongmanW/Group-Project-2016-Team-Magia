/*
Function: 		Provide keyboard input functions
Author: 		NMCG
Version:		1.0
Date Updated:	26/1/16
Bugs:			None
Fixes:			None
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GDLibrary
{
    /// <summary>
    /// Provides methods to determine the state of keyboard keys.
    /// </summary>
    public class KeyboardManager : GameComponent
    {
        #region Variables
        protected KeyboardState newState, oldState;
        #endregion

        #region Properties
        #endregion

        public KeyboardManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
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
            //store the old keyboard state for later comparison
            this.oldState = this.newState; 
            //get the current state in THIS update
            this.newState = Keyboard.GetState();
            base.Update(gameTime);
        }

        //is any key pressed on the keyboard?
        public bool IsKeyPressed()
        {
            return (this.newState.GetPressedKeys().Length != 0);
        }

        //is a key pressed?
        public bool IsKeyDown(Keys key)
        {
            return this.newState.IsKeyDown(key);
        }

        //is a key pressed now that was not pressed in the last update?
        public bool IsFirstKeyPress(Keys key)
        {
            return this.newState.IsKeyDown(key) && this.oldState.IsKeyUp(key);
        }

        //has the keyboard state changed since the last update?
        public bool IsStateChanged()
        {
            return !this.newState.Equals(oldState); //false if no change, otherwise true
        }
    }
}
