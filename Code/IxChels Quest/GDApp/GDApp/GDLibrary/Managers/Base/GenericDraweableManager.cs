using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GDApp;

namespace GDLibrary
{
    //Base class for all pauseable managers that update objects e.g. object manager, ui manager, sound manager
    public class GenericDraweableManager<T> : DrawableGameComponent where T : IUpdateable
    {
        #region Fields
        protected Main game;
        private bool bPaused;
        private List<T> drawList;
        private List<T> removeList;
        #endregion

        #region Properties
        public T this[int index]
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
        public bool IsPaused
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
        #endregion

        public GenericDraweableManager(Main game, int initialDrawSize, int initialRemoveSize)
            : base(game)
        {
            this.game = game;
            this.drawList = new List<T>(initialDrawSize);
            this.removeList = new List<T>(initialRemoveSize);

            //register for the menu events
            this.game.EventDispatcher.MainMenuChanged += EventDispatcher_MainMenu;
        }

        public virtual void SetGraphicsStateObjects()
        { 

        }

        #region Event Handling
        //handle the relevant menu events
        public virtual void EventDispatcher_MainMenu(EventData eventData)
        {
            if ((eventData.eventType == EventType.OnPlay) || (eventData.eventType == EventType.OnRestart))
                this.bPaused = false;
            else if (eventData.eventType == EventType.OnPause)
                this.bPaused = true;
        }
        #endregion

        public virtual void Add(T obj)
        {
            this.drawList.Add(obj);
        }

        public virtual void Remove(T obj)
        {
            this.removeList.Add(obj);
        }

        public virtual void processRemoveList()
        {
            for (int i = 0; i < this.removeList.Count; i++)
            {
                if (this.drawList.Contains(this.removeList[i]))
                    this.drawList.Remove(this.removeList[i]);
            }

            this.removeList.Clear();
        }

        //called typically at the start of each level in a game
        public virtual void Clear()
        {
            if((this.removeList != null) && (this.removeList.Count != 0))
                this.removeList.Clear();

            if ((this.drawList != null) && (this.drawList.Count != 0))
                this.drawList.Clear();
        }

        public override void Update(GameTime gameTime)
        {
            if (!this.IsPaused)
            {
                processRemoveList();

                for (int i = 0; i < this.drawList.Count; i++)
                {
                    this.drawList[i].Update(gameTime);
                }
            }
            base.Update(gameTime);
        }

    }
}
