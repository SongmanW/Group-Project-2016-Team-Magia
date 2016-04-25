
namespace GDApp
{
    public class GameData
    {
        public static float CameraSpeed = 0.1f;
        #region First Person Camera Move Multipliers
        //speed at which we move forward and backwards
        public static float CameraMoveSpeed = 0.3f;
        //scale so that we can strafe 70% slower than moving always
        public static float CameraStrafeSpeed = CameraMoveSpeed * 0.8f;
        public static float CameraRotationSpeed = 0.005f;
        //jump height for 1st Person collidable camera
        public static float CameraJumpHeight = 20;

        #endregion

        #region Player Move Multipliers
        //speed at which we move forward and backwards
        public static float PlayerMoveSpeed = 0.3f;
        //scale so that we can strafe 70% slower than moving always
        public static float PlayerStrafeSpeed = PlayerMoveSpeed * 0.8f;
        public static float PlayerRotationSpeed = 0.08f;
        //jump height for player
        public static float PlayerJumpHeight = 20;

        //when in first person mode we need to start pick OUTSIDE our own capsule collision skin
        //so this variable depends DIRECTLY on your 1st person camera player radius
        public static float FirstPersonStartPickDistance = 4;

        #endregion
    }
}
