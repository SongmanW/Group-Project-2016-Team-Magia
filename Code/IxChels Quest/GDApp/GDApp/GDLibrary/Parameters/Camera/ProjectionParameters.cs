using System;
using Microsoft.Xna.Framework;

/*
Function: 		Encapsulates the projection matrix specific parameters for the camera class
Author: 		NMCG
Version:		1.0
Date Updated:	1/1/16
Bugs:			None
Fixes:			None
*/

namespace GDLibrary
{
    public class ProjectionParameters : ICloneable
    {
        #region Fields
        public static ProjectionParameters StandardMediumFourThree = new ProjectionParameters(MathHelper.PiOver2, 4.0f / 3, 1, 1000);

        private float fieldOfView, aspectRatio, nearClipPlane, farClipPlane;
        private Matrix projection;
        private bool isDirty;
        private float originalFOV, originalAspectRatio, originalNearClipPlane, originalFarClipPlane;
        #endregion

        #region Properties
        public float FOV
        {
            get
            {
                return this.fieldOfView;
            }
            set
            {
                this.fieldOfView = value;
                this.isDirty = true;
            }
        }
        public float AspectRatio
        {
            get
            {
                return this.aspectRatio;
            }
            set
            {
                this.aspectRatio = value;
                this.isDirty = true;
            }
        }
        public float NearClipPlane
        {
            get
            {
                return this.nearClipPlane;
            }
            set
            {
                this.nearClipPlane = value;
                this.isDirty = true;
            }
        }
        public float FarClipPlane
        {
            get
            {
                return this.farClipPlane;
            }
            set
            {
                this.farClipPlane = value;
                this.isDirty = true;
            }
        }

        public Matrix Projection
        {
            get
            {
                if (this.isDirty)
                {
                    this.projection = Matrix.CreatePerspectiveFieldOfView(
                        this.fieldOfView, this.aspectRatio,
                        this.nearClipPlane, this.farClipPlane);
                    this.isDirty = false; 
                }
                return this.projection;
            }
        }
        #endregion

        public ProjectionParameters(float fieldOfView, float aspectRatio,
            float nearClipPlane, float farClipPlane)
        {
            this.FOV = this.originalFOV = fieldOfView;
            this.AspectRatio = this.originalAspectRatio = aspectRatio;
            this.NearClipPlane = this.originalNearClipPlane = nearClipPlane;
            this.FarClipPlane = this.originalFarClipPlane = farClipPlane;
        }

        public void Reset()
        {
            this.FOV = this.originalFOV;
            this.AspectRatio = this.originalAspectRatio;
            this.NearClipPlane = this.originalNearClipPlane;
            this.FarClipPlane = this.originalFarClipPlane;
        }

        public object Clone() //deep copy
        {
            //remember we can use a simple this.MemberwiseClone() because all fields are primitive C# types
            return this.MemberwiseClone();
        }

        public override bool Equals(object obj)
        {
            ProjectionParameters other = obj as ProjectionParameters;

            return float.Equals(this.FOV, other.FOV)
                && float.Equals(this.AspectRatio, other.AspectRatio)
                    && float.Equals(this.NearClipPlane, other.NearClipPlane)
                        && float.Equals(this.FarClipPlane, other.FarClipPlane);
        }

        public override int GetHashCode() //a simple hash code method 
        {
            int hash = 1;
            hash = hash * 31 + this.FOV.GetHashCode();
            hash = hash * 17 + this.AspectRatio.GetHashCode();
            hash = hash * 13 + this.NearClipPlane.GetHashCode();
            return hash;
        }


    }
}
