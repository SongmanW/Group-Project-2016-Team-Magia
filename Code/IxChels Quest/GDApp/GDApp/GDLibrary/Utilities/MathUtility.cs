using Microsoft.Xna.Framework;
using System;

namespace GDLibrary
{
    public class MathUtility
    {
        public static Integer3 Round(Vector3 value)
        {
            return new Integer3(Math.Round(value.X), Math.Round(value.Y), Math.Round(value.Z));
        }

        public static Integer2 Round(Vector2 value)
        {
            return new Integer2(Math.Round(value.X), Math.Round(value.Y));
        }

        public static int RandomExcludeNumber(int excludedValue, int max)
        {
            Random random = new Random();
            int randomValue = 0;
            do
            {
                randomValue = random.Next(max);

            } while (randomValue == excludedValue);

            return randomValue;
        }

        public static int RandomExcludeRange(int lo, int hi, int max)
        {
            Random random = new Random();
            int randomValue = 0;
            do
            {
                randomValue = random.Next(max);

            } while ((randomValue >= lo) && (randomValue <= hi));

            return randomValue;
        }

        public static Vector2 Lerp(Vector2 a, Vector2 b, float lerpFactor)
        {
            //takes two translations x1,y1 and x2,y2 and interpolates linearly between them using a factor    
            return new Vector2(MathHelper.Lerp(a.X, b.X, lerpFactor), MathHelper.Lerp(a.Y, b.Y, lerpFactor));
        }

        public static Vector2 Round(Vector2 a, int precision)
        {
            //takes two translations x1,y1 and x2,y2 and interpolates linearly between them using a factor    
            return new Vector2((float)Math.Round(a.X, precision), (float)Math.Round(a.Y, precision));
        }
    }
}
