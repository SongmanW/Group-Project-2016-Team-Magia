using GDApp;
using JigLibX.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace GDLibrary
{
    /// <summary>
    /// Represents your MOVEABLE player in the game. 
    /// </summary>
    public class PlayerObject : CharacterObject
    {
        #region Variables
        private Keys[] keys;
        #endregion

        #region Properties
        public Keys[] Keys
        {
            get
            {
                return keys;
            }
            set
            {
                keys = value;
            }
        }
        #endregion

        public PlayerObject(string id, ObjectType objectType, Transform3D transform, 
            Texture2D texture, Model model, Color color, float alpha, Keys[] keys,
         float radius, float height, float accelerationRate, float decelerationRate)
            : base(id, objectType, transform, texture, model, color, alpha,
                    radius, height, accelerationRate, decelerationRate)
        {
            this.keys = keys;
        }

        public override void Update(GameTime gameTime)
        {
            HandleKeyboardInput(gameTime);
            HandleMouseInput(gameTime);
            base.Update(gameTime);
        }

        protected virtual void HandleMouseInput(GameTime gameTime)
        {
          //perhaps rotate using mouse pointer distance from centre?
        }

        protected virtual void HandleKeyboardInput(GameTime gameTime)
        {
            //Sample code to illustrate how to move a player
            //this.CharacterBody.DoJump(2);
            //this.CharacterBody.Velocity = //..
            //this.CharacterBody.DesiredVelocity = Vector3.Zero;
            //this.Transform3D.RotateBy(Vector3.UnitY * 2);

            Vector3 rotate = Vector3.Zero;
            Vector3 moveVectorX = Vector3.Zero;
            Vector3 moveVectorZ = Vector3.Zero;
            if (game.KeyboardManager.IsKeyDown(Keys[KeyData.PlayerMoveUpIndex]) || game.KeyboardManager.IsKeyDown(Keys[KeyData.PlayerMoveUpIndexAlt]))
            {
                moveVectorX = Vector3.Normalize(new Vector3(game.ActiveCamera.Transform3D.Look.X, 0, game.ActiveCamera.Transform3D.Look.Z));
            }
            else if (game.KeyboardManager.IsKeyDown(Keys[KeyData.PlayerMoveDownIndex]) || game.KeyboardManager.IsKeyDown(Keys[KeyData.PlayerMoveDownIndexAlt]))
            {
                moveVectorX = -Vector3.Normalize(new Vector3(game.ActiveCamera.Transform3D.Look.X, 0, game.ActiveCamera.Transform3D.Look.Z));
            }

            if (game.KeyboardManager.IsKeyDown(Keys[KeyData.PlayerMoveLeftIndex]) || game.KeyboardManager.IsKeyDown(Keys[KeyData.PlayerMoveLeftIndexAlt]))
            {
                moveVectorZ = -Vector3.Normalize(new Vector3(game.ActiveCamera.Transform3D.Right.X, 0, game.ActiveCamera.Transform3D.Right.Z));
            }
            else if (game.KeyboardManager.IsKeyDown(Keys[KeyData.PlayerMoveRightIndex]) || game.KeyboardManager.IsKeyDown(Keys[KeyData.PlayerMoveRightIndexAlt]))
            {
                moveVectorZ = Vector3.Normalize(new Vector3(game.ActiveCamera.Transform3D.Right.X, 0, game.ActiveCamera.Transform3D.Right.Z));
            }
            this.CharacterBody.Velocity = ((moveVectorX == Vector3.Zero && moveVectorZ == Vector3.Zero ? Vector3.Zero : Vector3.Normalize(moveVectorX + moveVectorZ)) * 85);
            this.CharacterBody.DesiredVelocity = Vector3.Zero;
            if (moveVectorX != Vector3.Zero || moveVectorZ != Vector3.Zero)
            {
                Vector3 moveVector = Vector3.Normalize(moveVectorX + moveVectorZ);
                if (moveVector.Z >= 0)
                {
                    this.Transform3D.RotateTo(new Vector3(0, 180 - MathHelper.ToDegrees((float)Math.Acos(moveVector.X)), 0));
                }
                else
                {
                    this.Transform3D.RotateTo(new Vector3(0, 180 + MathHelper.ToDegrees((float)Math.Acos(moveVector.X)), 0));
                }
            }
        }

        //Do we want to detect if the player object collides with something?
        //in this case we need to add the CollisionSkin_callbackFn() method
        // - See CollidableObject::CollisionSkin_callbackFn

        
     
    }
}
