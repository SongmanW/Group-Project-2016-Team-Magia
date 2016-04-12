
using Microsoft.Xna.Framework.Input;

namespace GDApp
{
    public class KeyData
    {
        public static Keys[] Player_Keys = new Keys[]{Keys.W, Keys.S, Keys.A, Keys.D, Keys.Space, Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.Enter, Keys.Escape};
        public static int PlayerMoveUpIndex = 0;
        public static int PlayerMoveDownIndex = 1;
        public static int PlayerMoveLeftIndex = 2;
        public static int PlayerMoveRightIndex = 3;
        public static int PlayerInteractIndex = 4;
        public static int PlayerMoveUpIndexAlt = 5;
        public static int PlayerMoveDownIndexAlt = 6;
        public static int PlayerMoveLeftIndexAlt = 7;
        public static int PlayerMoveRightIndexAlt = 8;
        public static int PlayerInteractIndexAlt = 9;
        public static int PlayerPauseIndex = 10;

    }
}
