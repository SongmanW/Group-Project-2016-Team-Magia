using System;
using Microsoft.Xna.Framework;

/*
Function: 		Encapsulates the transformation and World matrix specific parameters for any entity that can have a position e.g. a player, a prop, a camera
Author: 		NMCG
Version:		1.0
Date Updated:	1/1/16
Bugs:			None
Fixes:			None
*/

namespace GDLibrary
{
    public class Transform3D : ICloneable
    {
        public static Transform3D Zero = new Transform3D(Vector3.Zero,
                                Vector3.Zero, Vector3.One, -Vector3.UnitZ, Vector3.UnitY);

        #region Fields
        private Vector3 translation, rotation, scale;
        private Vector3 look, up;
        private Matrix world;
        private bool isDirty;
        private Vector3 originalRotation, originalLook, originalUp, originalTranslation, originalScale;
        #endregion

        #region Properties
        public Matrix World
        {
            set
            {
                this.world = value;
            }
            get
            {
                if (this.isDirty)
                {
                    this.world = Matrix.Identity * Matrix.CreateScale(scale)
                                    * Matrix.CreateRotationX(MathHelper.ToRadians(rotation.X))
                                        * Matrix.CreateRotationY(MathHelper.ToRadians(rotation.Y))
                                            * Matrix.CreateRotationZ(MathHelper.ToRadians(rotation.Z))
                                                * Matrix.CreateTranslation(translation);
                    this.isDirty = false;
                }
                return this.world;
            }
        }
        public Vector3 Translation
        {
            get
            {
                return this.translation;
            }
            set
            {
                this.translation = value;
                this.isDirty = true;
            }
        }
        public Vector3 Rotation
        {
            get
            {
                return this.rotation;
            }
            set
            {
                this.rotation = value;
                this.isDirty = true;
            }
        }
        public Vector3 Scale
        {
            get
            {
                return this.scale;
            }
            set
            {
                this.scale = value;
            }
        }

        public Vector3 Target
        {
            get
            {
                return this.translation + this.look;
            }
        }
        public Vector3 Up
        {
            get
            {
                return this.up;
            }
            set
            {
                this.up = value;
            }
        }
        public Vector3 Look
        {
            get
            {
                return this.look;
            }
            set
            {
                this.look = value;
            }
        }
        public Vector3 Right
        {
            get
            {
                return Vector3.Normalize(Vector3.Cross(this.look, this.up));
            }
        }
        #endregion

        //used by the camera
        public Transform3D(Vector3 translation, Vector3 look, Vector3 up)
            : this(translation, Vector3.Zero, Vector3.One, look, up)
        {

        }

        //used by drawn objects
        public Transform3D(Vector3 translation, Vector3 rotation, Vector3 scale, Vector3 look, Vector3 up)
        {
            this.Translation = this.originalTranslation = translation;
            this.Rotation = this.originalRotation = rotation;
            this.Scale = this.originalScale = scale;

            //set look, up
            this.Look = this.originalLook = Vector3.Normalize(look);
            this.Up = this.originalUp = Vector3.Normalize(up);
        }

        public void Reset()
        {
            this.translation = this.originalTranslation;
            this.rotation = this.originalRotation;
            this.scale = this.originalScale;
            this.look = this.originalLook;
            this.up = this.originalUp;
        }

        public object Clone() //deep copy - Vector3 are structures (i.e. value types) and so MemberwiseClone() will copy by value and effectively make a deep copy
        {
            return this.MemberwiseClone();
        }

        public override bool Equals(object obj)
        {
            Transform3D other = obj as Transform3D;

            return Vector3.Equals(this.translation, other.Translation)
                && Vector3.Equals(this.rotation, other.Rotation)
                    && Vector3.Equals(this.scale, other.Scale)
                        && Vector3.Equals(this.look, other.Look)
                         && Vector3.Equals(this.up, other.Up);
        }

        public override int GetHashCode() //a simple hash code method 
        {
            int hash = 1;
            hash = hash * 31 + this.translation.GetHashCode();
            hash = hash * 17 + this.look.GetHashCode();
            hash = hash * 13 + this.up.GetHashCode();
            return hash;
        }


        public void RotateBy(Vector3 rotateBy)
        {
            this.rotation = this.originalRotation + rotateBy;

            //update the look and up - RADIANS!!!!
            Matrix rot = Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(this.rotation.X),
                MathHelper.ToRadians(this.rotation.Y), MathHelper.ToRadians(this.rotation.Z));

            this.look = Vector3.Transform(this.originalLook, rot);
            this.up = Vector3.Transform(this.originalUp, rot);

            this.isDirty = true;
        }

        public void RotateAroundYBy(float magnitude)
        {
            this.rotation.Y += magnitude;
            this.look = Vector3.Normalize(Vector3.Transform(originalLook,
                Matrix.CreateRotationY(MathHelper.ToRadians(rotation.Y))));
   
            this.isDirty = true;
        }

        public void Rotate(Vector3 rotateBy)
        {
            this.rotation += rotateBy;

            //update the look and up - RADIANS!!!!
            Matrix rot = Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(this.rotation.X),
                MathHelper.ToRadians(this.rotation.Y), MathHelper.ToRadians(this.rotation.Z));

            this.look = Vector3.Transform(this.originalLook, rot);
            this.up = Vector3.Transform(this.originalUp, rot);

            this.isDirty = true;
        }

        public void RotateTo(Vector3 rotateTo)
        {
            this.rotation = rotateTo;

            //update the look and up - RADIANS!!!!
            Matrix rot = Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(this.rotation.X),
                MathHelper.ToRadians(this.rotation.Y), MathHelper.ToRadians(this.rotation.Z));

            this.look = Vector3.Transform(this.originalLook, rot);
            this.up = Vector3.Transform(this.originalUp, rot);

            this.isDirty = true;
        }

        public void TranslateBy(Vector3 translateBy, float speedMultiplier)
        {
            this.translation += translateBy * speedMultiplier;
            this.isDirty = true;
        }

        public void ScaleTo(Vector3 scale)
        {
            this.scale = scale;
            this.isDirty = true;
        }




    }
}
