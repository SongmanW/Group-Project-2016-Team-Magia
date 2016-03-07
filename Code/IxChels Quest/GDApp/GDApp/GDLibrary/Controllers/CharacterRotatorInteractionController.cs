using GDApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GDLibrary
{
    public class CharacterRotatorInteractionController : Controller
    {
        private Main game;
        private Actor targetActor;
        private bool isRotating;

        public Actor TargetActor
        {
            get
            {
                return targetActor;
            }
            set
            {
                this.targetActor = value;
            }
        }

        public CharacterRotatorInteractionController(Main game, string name, Actor parentActor)
            : base(name, parentActor)
        {
            this.game = game;
            this.targetActor = null;
            this.isRotating = false;
        }

        public CharacterRotatorInteractionController(Main game, string name, Actor parentActor, Actor targetActor)
            : base(name, parentActor)
        {
            this.game = game;
            this.targetActor = targetActor;
            this.isRotating = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (this.isRotating)
            {
                if (this.game.KeyboardManager.IsKeyDown(Keys.Space))
                {
                    if (this.game.KeyboardManager.IsKeyDown(Keys.H))
                        this.targetActor.Transform3D.RotateAroundYBy(-0.5f);
                    else if (this.game.KeyboardManager.IsKeyDown(Keys.J))
                        this.targetActor.Transform3D.RotateAroundYBy(0.5f);
                }
                else
                {
                    this.isRotating = false;
                }
            }
            else {
                //collision Detection needed
                if (this.game.KeyboardManager.IsFirstKeyPress(Keys.Space))
                {
                    // set Camera to right Position and CameraLayout
                    Transform3D oldposition = this.game.CameraManager[0].Transform3D;
                    this.game.CameraManager.SetCameraLayout("RotateCamera");
                    this.game.CameraManager[0].Transform3D = oldposition;

                    // Set everything for Rotation
                    ((RotatorController)((PawnCamera3D)this.game.CameraManager[0]).ControllerList[0]).Set();
                    ((CharacterMoveController)this.game.playerActor.ControllerList[0]).Camera = this.game.CameraManager[0];
                    this.isRotating = true;
                }
            }
        }
    }
}
