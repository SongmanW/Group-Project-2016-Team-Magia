using Microsoft.Xna.Framework;

namespace GDLibrary
{
    public class CameraUtility
    {
        //camera to target vector, also provides access to distance from camera to target
        public static Vector3 GetCameraToTarget(Transform3D parent, Transform3D camera, out float distance)
        {
            //camera to target object vector
            Vector3 cameraToTarget = parent.Translation - camera.Translation;

            //distance from camera to target
            distance = cameraToTarget.Length();

            //camera to target object vector
            cameraToTarget.Normalize();

            return cameraToTarget;
        }

        //camera to target transform
        public static Vector3 GetCameraToTarget(Transform3D parent, Transform3D camera)
        {
            //camera to target object vector
            return Vector3.Normalize(parent.Translation - camera.Translation);
        }

        //camera to target vector --added by Songman
        public static Vector3 GetCameraToTarget(Vector3 parent, Transform3D camera)
        {
            //camera to target object vector
            return Vector3.Normalize(parent - camera.Translation);
        }
    }
}
