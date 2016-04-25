using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GDLibrary
{
    public class PawnCollidableObject : CollidableObject
    {
        #region Fields
        private List<IController> controllerList;
        #endregion

        #region Properties
        public List<IController> ControllerList
        {
            get
            {
                return this.controllerList;
            }
        }
        #endregion

        public PawnCollidableObject(string id, ObjectType objectType, Transform3D transform, Effect effect,
            Texture2D texture, Model model, Color color, float alpha)
            : base(id, objectType, transform, effect, texture, model, color, alpha)
        {

        }

        public void Add(IController controller)
        {
            if (this.controllerList == null)
                this.controllerList = new List<IController>();

            //contains? hash code and equals?
            this.controllerList.Add(controller);
        }

        public void Remove(string name)
        {
            for (int i = 0; i < this.controllerList.Count; i++)
            {
                if (this.controllerList[i].GetName().Equals(name))
                {
                    this.controllerList.RemoveAt(i);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (this.controllerList != null)
            {
                foreach (IController controller in this.controllerList)
                    controller.Update(gameTime);
            }
        }
    }
}
