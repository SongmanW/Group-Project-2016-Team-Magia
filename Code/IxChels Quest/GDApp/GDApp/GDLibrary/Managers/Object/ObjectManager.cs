using GDApp;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace GDLibrary
{
    public class ObjectManager : DrawableGameComponent
    {
        #region Fields
        private List<DrawnActor> drawList, removeList;
        private Main game;
        #endregion

        #region Properties
        public DrawnActor this[int index]
        {
            get
            {
                return this.drawList[index];
            }
        }
        public int Size
        {
            get
            {
                return this.drawList.Count;
            }
        }
        #endregion

        public ObjectManager(Main game, int initialDrawSize, int initialRemoveSize)
            : base(game)
        {
            this.game = game;
            this.drawList = new List<DrawnActor>(initialDrawSize);
            this.removeList = new List<DrawnActor>(initialRemoveSize);

            game.EventDispatcher.RotationStarted += HandleRotationStart;
            game.EventDispatcher.RotationEnd += HandleRotationEnd;
        }

        private void HandleRotationEnd(object sender)
        {
            Console.WriteLine("Rotation End");
            ((RotatorController)(this.game.wall1).ControllerList[0]).Unset();
            ((RotatorController)(this.game.wall2).ControllerList[0]).Unset();
            this.game.playerActor.Unset();
        }

        private void HandleRotationStart(object sender)
        {
            Console.WriteLine("Rotation Start");
            ((RotatorController)(this.game.wall1).ControllerList[0]).Set();
            ((RotatorController)(this.game.wall2).ControllerList[0]).Set();
            this.game.playerActor.Set();
            ((RotorController)this.game.rotator.ControllerList[1]).Rotate(90, 3000, true);
        }

        public void Add(DrawnActor actor)
        {
            this.drawList.Add(actor);
        }

        public void Remove(DrawnActor actor)
        {
            this.removeList.Add(actor);
        }

        public override void Update(GameTime gameTime)
        {
            processRemoveList();

            for (int i = 0; i < this.drawList.Count; i++)
            {
                this.drawList[i].Update(gameTime);
            }

            base.Update(gameTime);
        }

  

        private void processRemoveList()
        {
            for(int i = 0; i < this.removeList.Count; i++)
            {
                if (this.drawList.Contains(this.removeList[i]))
                    this.drawList.Remove(this.removeList[i]);
            }

            this.removeList.Clear();
        }

        public override void Draw(GameTime gameTime)
        {
            for (int i = 0; i < this.drawList.Count; i++)
            {
                this.drawList[i].Draw(gameTime);
            }

            base.Draw(gameTime);
        }
    }
}
