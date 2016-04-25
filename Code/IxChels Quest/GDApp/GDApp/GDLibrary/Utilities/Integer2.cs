using System;
using Microsoft.Xna.Framework;
namespace GDLibrary
{
    public class Integer2 : ICloneable
    {
        public static Integer2 Zero = new Integer2(0, 0);
        public static Integer2 One = new Integer2(1, 1);
        public static Integer2 UnitX = new Integer2(1, 0);
        public static Integer2 UnitY = new Integer2(0, 1);

        private int x, y;

        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }
        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        public Integer2(int x, int y)
        {
            this.x = x;
            this.y = y;

        }
        public Integer2(Vector2 value)
            : this(value.X, value.Y)
        {
        }
        public Integer2(float x, float y)
            : this((int)x, (int)y)
        {
        }

        public Integer2(double x, double y)
            : this((float)x, (float)y)
        {
        }


        public override string ToString()
        {
            return "(x: " + x + ", " + "y: " + y + ")";
        }

        public static Integer2 operator *(Integer2 value, int multiplier)
        {
            return new Integer2(value.X * multiplier, value.Y * multiplier);
        }

        public static Integer2 operator *(int multiplier, Integer2 value)
        {
            return new Integer2(value.X * multiplier, value.Y * multiplier);
        }

        //to do - add /, + - operator methods

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

}
