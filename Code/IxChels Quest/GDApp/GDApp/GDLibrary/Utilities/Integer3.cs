using System;
namespace GDLibrary
{
    public class Integer3 : ICloneable
    {
        public static Integer3 Zero = new Integer3(0, 0, 0);
        public static Integer3 One = new Integer3(1, 1, 1);
        public static Integer3 UnitX = new Integer3(1, 0, 0);
        public static Integer3 UnitY = new Integer3(0, 1, 0);
        public static Integer3 UnitZ = new Integer3(0, 0, 1);
        private int x, y, z;

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
        public int Z
        {
            get
            {
                return z;
            }
            set
            {
                z = value;
            }
        }

        public Integer3(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Integer3(float x, float y, float z)
            : this((int)x, (int)y, (int)z)
        {
        }

        public Integer3(double x, double y, double z)
            : this((float)x, (float)y, (float)z)
        {
        }


        public override string ToString()
        {
            return "(x: " + x + ", " + "y: " + y + ", " + "z: " + z + ")";
        }

        public static Integer3 operator *(Integer3 value, int multiplier)
        {
            return new Integer3(value.X * multiplier, value.Y * multiplier, value.Z * multiplier);
        }

        public static Integer3 operator *(int multiplier, Integer3 value)
        {
            return new Integer3(value.X * multiplier, value.Y * multiplier, value.Z * multiplier);
        }

        //to do - add /, + - operator methods

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

}
