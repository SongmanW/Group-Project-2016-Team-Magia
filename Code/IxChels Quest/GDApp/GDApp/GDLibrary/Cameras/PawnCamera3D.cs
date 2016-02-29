using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GDLibrary
{
    //Modifies a basic static camera to allow us to add multiple controllers e.g. rail, track, 1st or 3rd person
    public class PawnCamera3D : Camera3D
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

        public PawnCamera3D(string id, ObjectType objectType,
            Transform3D transform, ProjectionParameters projectionParameters, Viewport viewPort)
                : base(id, objectType, transform, projectionParameters, viewPort)
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
            for(int i = 0; i < this.controllerList.Count; i++)
            {
                if(this.controllerList[i].GetName().Equals(name))
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
            PawnCamera3D clone
                = new PawnCamera3D("clone - " + this.ID,
                    this.ObjectType,
                    (Transform3D)this.Transform3D.Clone(), //deep copy
                    (ProjectionParameters)this.ProjectionParameters.Clone(), //deep copy - contains "copy by value" types
                    this.Viewport); //shallow or deep?

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
