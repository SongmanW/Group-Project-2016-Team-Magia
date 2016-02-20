using GDApp;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace GDLibrary
{
    public class ObjectManager : DrawableGameComponent
    {
        #region Fields
        private List<DrawnActor> drawList, removeList;
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
            this.drawList = new List<DrawnActor>(initialDrawSize);
            this.removeList = new List<DrawnActor>(initialRemoveSize);
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
