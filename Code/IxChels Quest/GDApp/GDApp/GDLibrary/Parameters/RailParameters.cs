using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GDLibrary
{
    public class RailParameters
    {
        #region Fields
        private string id;
        private Vector3 start, end, midPoint, look;
        private bool isDirty;
        private float length;
        #endregion

        #region Properties
        public Vector3 Look
        {
            get
            {
                Update();
                return this.look;
            }
        }

        public float Length
        {
            get
            {
                Update();
                return this.length;
            }
        }

        public Vector3 MidPoint
        {
            get
            {
                Update();
                return this.midPoint;
            }
        }
        public Vector3 Start
        {
            get
            {
                return this.start;
            }
            set
            {
                this.start = value;
                this.isDirty = true;
            }
        }
        public Vector3 End
        {
            get
            {
                return this.end;
            }
            set
            {
                this.start = value;
                this.isDirty = true;
            }
        }
        #endregion

        public RailParameters(string id, Vector3 start, Vector3 end)
        {
            this.start = start;
            this.end = end;
            this.id = id;

            this.isDirty = true;
        }


        //Returns true if the position is between start and end, otherwise false
        public bool InsideRail(Vector3 position)
        {
            float distanceToStart = Vector3.Distance(position, start);
            float distanceToEnd = Vector3.Distance(position, end);
            return ((distanceToStart <= length) && (distanceToEnd <= length));
        }


        private void Update()
        {
            if (this.isDirty)
            {
                this.length = Math.Abs(Vector3.Distance(start, end));
                this.look = Vector3.Normalize(end - start);
                this.midPoint = (start + end) / 2;
                this.isDirty = false;
            }
        }

        //todo - Add clone...


    }
}
