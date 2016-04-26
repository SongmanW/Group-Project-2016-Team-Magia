using GDApp;
using JigLibX.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SkinnedModel;
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
        private bool bSet;
        private Vector3 offSet;
        private Transform3D oldTransform;
        private Transform3D oldPTransform;
        private Random random;
        private Matrix[] transforms;
        private Vector3 translationOffset;

        #endregion

        #region Properties
        public Vector3 TranslationOffset
        {
            get
            {
                return translationOffset;
            }
            set
            {
                translationOffset = value;
            }
        }
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

        public PlayerObject(string id, ObjectType objectType, Transform3D transform, Effect effect, 
            Texture2D texture, Model model, Color color, float alpha, Keys[] keys,
         float radius, float height, float accelerationRate, float decelerationRate, Vector3 translationOffset)
            : base(id, objectType, transform, effect, texture, model, color, alpha,
                    radius, height, accelerationRate, decelerationRate)
        {
            this.keys = keys;
            this.translationOffset = translationOffset;

            this.Body.CollisionSkin.callbackFn += CollisionSkin_callbackFn;
            this.random = new Random();
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
            if (!bSet)
            {
                Vector3 rotate = Vector3.Zero;
                Vector3 moveVectorX = Vector3.Zero;
                Vector3 moveVectorZ = Vector3.Zero;
                if (game.KeyboardManager.IsKeyDown(Keys[KeyData.PlayerMoveUpIndex]) || game.KeyboardManager.IsKeyDown(Keys[KeyData.PlayerMoveUpIndexAlt]))
                {
                    moveVectorX = Vector3.Normalize(new Vector3(game.CameraManager.ActiveCamera.Transform3D.Look.X, 0, game.CameraManager.ActiveCamera.Transform3D.Look.Z));
                }
                else if (game.KeyboardManager.IsKeyDown(Keys[KeyData.PlayerMoveDownIndex]) || game.KeyboardManager.IsKeyDown(Keys[KeyData.PlayerMoveDownIndexAlt]))
                {
                    moveVectorX = -Vector3.Normalize(new Vector3(game.CameraManager.ActiveCamera.Transform3D.Look.X, 0, game.CameraManager.ActiveCamera.Transform3D.Look.Z));
                }

                if (game.KeyboardManager.IsKeyDown(Keys[KeyData.PlayerMoveLeftIndex]) || game.KeyboardManager.IsKeyDown(Keys[KeyData.PlayerMoveLeftIndexAlt]))
                {
                    moveVectorZ = -Vector3.Normalize(new Vector3(game.CameraManager.ActiveCamera.Transform3D.Right.X, 0, game.CameraManager.ActiveCamera.Transform3D.Right.Z));
                }
                else if (game.KeyboardManager.IsKeyDown(Keys[KeyData.PlayerMoveRightIndex]) || game.KeyboardManager.IsKeyDown(Keys[KeyData.PlayerMoveRightIndexAlt]))
                {
                    moveVectorZ = Vector3.Normalize(new Vector3(game.CameraManager.ActiveCamera.Transform3D.Right.X, 0, game.CameraManager.ActiveCamera.Transform3D.Right.Z));
                }
                if (moveVectorX == Vector3.Zero && moveVectorZ == Vector3.Zero)
                {
                    this.CharacterBody.DesiredVelocity = Vector3.Zero;
                }
                else
                {
                    this.CharacterBody.Velocity = Vector3.Normalize(moveVectorX + moveVectorZ) * 45;

                    int ran = (int)Math.Floor(random.NextDouble() * 2.9);
                    switch(ran)
                    {
                        case 0:
                            game.SoundManager.PlayCue("Footsteps_Stone1");
                            break;
                        case 1:
                            game.SoundManager.PlayCue("Footsteps_Stone2");
                            break;
                        case 2:
                            game.SoundManager.PlayCue("Footsteps_Stone3");
                            break;
                    }
                }
                if (moveVectorX != Vector3.Zero || moveVectorZ != Vector3.Zero)
                {
                    Vector3 moveVector = Vector3.Normalize(moveVectorX + moveVectorZ);
                    if (moveVector.Z >= 0)
                    {
                        this.Transform3D.RotateTo(new Vector3(-90, -MathHelper.ToDegrees((float)Math.Acos(moveVector.X)), 0));
                    }
                    else
                    {
                        this.Transform3D.RotateTo(new Vector3(-90, MathHelper.ToDegrees((float)Math.Acos(moveVector.X)), 0));
                    }
                }
                if (game.NextStep > 1)
                {
                    if (this.Transform3D.Translation.Length() < 50)
                    {
                        Console.WriteLine(this.Transform3D.Look);
                        if (game.KeyboardManager.IsFirstKeyPress(Keys[KeyData.PlayerInteractIndex]) || game.KeyboardManager.IsFirstKeyPress(Keys[KeyData.PlayerInteractIndexAlt]))
                        {
                            if(this.Transform3D.Translation.X < 0)
                            {
                                //down
                                if(this.Transform3D.Translation.Z < 0)
                                {
                                    //left
                                    if(this.Transform3D.Look.X < this.Transform3D.Look.Z)
                                        EventDispatcher.Publish(new RotationEventData("rotation start", this, EventType.OnRotationStart, false));
                                    else
                                        EventDispatcher.Publish(new RotationEventData("rotation start", this, EventType.OnRotationStart, true));
                                }
                                else
                                {
                                    //right
                                    if (this.Transform3D.Look.X + this.Transform3D.Look.Z > 0)
                                        EventDispatcher.Publish(new RotationEventData("rotation start", this, EventType.OnRotationStart, false));
                                    else
                                        EventDispatcher.Publish(new RotationEventData("rotation start", this, EventType.OnRotationStart, true));
                                }
                            }
                            else
                            {
                                //up
                                if (this.Transform3D.Translation.Z < 0)
                                {
                                    //left
                                    if (this.Transform3D.Look.X + this.Transform3D.Look.Z > 0)
                                        EventDispatcher.Publish(new RotationEventData("rotation start", this, EventType.OnRotationStart, true));
                                    else
                                        EventDispatcher.Publish(new RotationEventData("rotation start", this, EventType.OnRotationStart, false));

                                }
                                else
                                {
                                    //right
                                    if (this.Transform3D.Look.X < this.Transform3D.Look.Z)
                                        EventDispatcher.Publish(new RotationEventData("rotation start", this, EventType.OnRotationStart, true));
                                    else
                                        EventDispatcher.Publish(new RotationEventData("rotation start", this, EventType.OnRotationStart, false));

                                }

                            }
                            game.SoundManager.PlayCue("Totem_Rotate");
                        }
                    }
                }
                if (game.CameraManager.ActiveCamera.ID == "RailCamera2")
                {
                    if (this.Transform3D.Translation.Z > 20)
                    {
                        EventDispatcher.Publish(new CameraEventData("change Camera", this, EventType.OnCameraChanged, "FullScreen", "RailCamera3"));
                    }
                }
                else if (game.CameraManager.ActiveCamera.ID == "RailCamera3")
                {
                    if (this.Transform3D.Translation.Z < -20)
                    {
                        EventDispatcher.Publish(new CameraEventData("change Camera", this, EventType.OnCameraChanged, "FullScreen", "RailCamera2"));
                    }
                }
                    
            }
            else
            {
                Vector3 newRotation = oldTransform.Rotation - game.rotator.Transform3D.Rotation;

                Matrix rot = Matrix.CreateFromYawPitchRoll(-MathHelper.ToRadians(newRotation.Y),
                    MathHelper.ToRadians(newRotation.X), MathHelper.ToRadians(newRotation.Z));

                this.Transform3D.Translation = game.rotator.Transform3D.Translation + Vector3.Transform(this.offSet, rot);
                this.Transform3D.Look = Vector3.Transform(this.oldPTransform.Look, rot);
                this.Transform3D.Up = Vector3.Transform(this.oldPTransform.Up, rot);
                this.Transform3D.Rotation = this.oldPTransform.Rotation - newRotation;

                this.Body.MoveTo(this.Transform3D.Translation, this.Transform3D.Orientation);
            }
        }

        //Do we want to detect if the player object collides with something?
        //in this case we need to add the CollisionSkin_callbackFn() method
        // - See CollidableObject::CollisionSkin_callbackFn

        public bool CollisionSkin_callbackFn(CollisionSkin collider, CollisionSkin collidee)
        {
            if (collidee.Owner.ExternalData is PawnCollidableObject)
            {
                //if the object that i collide with is a pickup object then set its color to be blue
                PawnCollidableObject c = (PawnCollidableObject)collidee.Owner.ExternalData;

                if (c.ObjectType == GDLibrary.ObjectType.Plate)
                {
                    if (!((OffsetController)c.ControllerList[0]).isSet)
                    {
                        int step = 0;
                        switch (c.ID)
                        {
                            case "PressurePlate1":
                                step = 1;
                                break;
                            case "PressurePlate2":
                                step = 2;
                                break;
                            case "PressurePlate3":
                                step = 3;
                                break;
                            case "PressurePlate4":
                                step = 4;
                                break;
                            case "PressurePlate5":
                                step = 5;
                                break;
                        }

                        game.checkNext(step);
                    }
                }
                else if(c.ObjectType == ObjectType.Door)
                {
                    //win
                    game.checkNext(6);
                }
            }

            return true;
        }

        public void Set()
        {
            this.offSet = this.Transform3D.Translation - game.rotator.Transform3D.Translation;
            this.oldTransform = (Transform3D)game.rotator.Transform3D.Clone();
            this.oldPTransform = (Transform3D)this.Transform3D.Clone();

            this.Enable(true, 1);

            bSet = true;
        }

        public void Unset()
        {
            bSet = false;
            this.Body.MoveTo(this.Transform3D.Translation, this.Transform3D.Orientation);
            this.Enable(false, 1);
        }



        public override Matrix GetWorldMatrix()
        {
            return Matrix.CreateScale(this.Transform3D.Scale) *
                this.Collision.GetPrimitiveLocal(0).Transform.Orientation *
                this.Body.Orientation *
                this.Transform3D.Orientation *
                Matrix.CreateTranslation(this.Body.Position + translationOffset);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (ModelMesh mesh in this.Model.Meshes)
            {
                foreach (BasicEffect be in mesh.Effects)
                {
                    //be.EnableDefaultLighting();

                    //uncomment and try this code
                    be.EmissiveColor = Color.DarkGray.ToVector3();
                    be.DirectionalLight0.DiffuseColor = Color.White.ToVector3();
                    be.DirectionalLight0.Direction = new Vector3(0, 0, -1);
                    be.DirectionalLight0.Enabled = true;
                    be.SpecularColor = Color.White.ToVector3();
                    be.SpecularPower = 0.1f;


                    be.Projection = game.CameraManager.ActiveCamera.ProjectionParameters.Projection;
                    be.View = game.CameraManager.ActiveCamera.View;
                    be.World = transforms[mesh.ParentBone.Index] * this.Transform3D.World;

                    if (this.Texture != null)
                    {
                        be.TextureEnabled = true;
                        be.Texture = this.Texture;
                    }
                    else
                    {
                        be.TextureEnabled = true;
                    }
                }
                //Draw
                mesh.Draw();
            }
            base.Draw(gameTime);
        }
    }
}
