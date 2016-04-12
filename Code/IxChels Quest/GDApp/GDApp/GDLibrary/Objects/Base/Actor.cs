using GDApp;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GDLibrary
{
    public class Actor
    {
        public static Main game;

        #region Fields
        private string id;
        private ObjectType objectType;
        private Transform3D transform;
        #endregion

        #region Properties
        public string ID
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }
        public ObjectType ObjectType
        {
            get
            {
                return this.objectType;
            }
            set
            {
                this.objectType = value;
            }
        }
        public Transform3D Transform3D
        {
            get
            {
                return this.transform;
            }
            set
            {
                this.transform = value;
            }
        }
        public Matrix World
        {
            get
            {
                return this.transform.World;
            }
        }
        #endregion

        public Actor(string id, ObjectType objectType, Transform3D transform)
        {
            this.id = id;
            this.objectType = objectType;
            this.transform = transform;
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual Matrix GetWorldMatrix()
        {
            return this.transform.World;
        }

        public virtual void Remove()
        {
            //tag for garbage collection
            this.transform = null;
        }
    }
}
