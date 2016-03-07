using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GDLibrary
{
    public class PawnModelObject : ModelObject
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

        public PawnModelObject(string id, ObjectType objectType,
            Transform3D transform, Texture2D texture, Model model)
                : base(id, objectType, transform, texture, model)
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

        //add clone...
        public override object Clone()
        {
            PawnModelObject clone
                = new PawnModelObject("clone - " + this.ID,
                    this.ObjectType,
                    (Transform3D)this.Transform3D.Clone(), //deep copy
                    this.Texture,
                    this.Model); //shallow or deep?

            for (int i = 0; i < this.controllerList.Count; i++)
            {
                IController cloneController
                        = (IController)this.controllerList[i].Clone();
                cloneController.SetParentActor(clone);
                clone.Add(cloneController);
            }

            return clone;
        }


    }
}
