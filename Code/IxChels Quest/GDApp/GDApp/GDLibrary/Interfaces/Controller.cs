using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GDLibrary
{
    public class Controller : IController
    {
        #region Fields
        private string name;
        private Actor parentActor;
        #endregion

        #region Properties
        public Actor ParentActor
        {
            get
            {
                return this.parentActor;
            }
            set
            {
                this.parentActor = value;
            }
        }
        public string Name
        {
            get
            {
                return this.name;
            }
        }
        #endregion

        public Controller(string name, Actor parentActor)
        {
            this.name = name;
            this.parentActor = parentActor;
        }

        public string GetName()
        {
            return this.name;
        }
        public Actor GetParentActor()
        {
            return this.parentActor;
        }

        public void SetParentActor(Actor parentActor)
        {
            this.parentActor = parentActor;
        }

        public virtual void Update(GameTime gameTime)
        {
            
        }

        public virtual object Clone()
        {
            return new Controller("clone - " + this.name,
                //we will generally reset parent actor 
                //since new controller should have new parent
                this.parentActor);
        }
    }
}
