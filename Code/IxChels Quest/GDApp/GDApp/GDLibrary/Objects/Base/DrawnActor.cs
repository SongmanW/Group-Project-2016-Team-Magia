using Microsoft.Xna.Framework;

namespace GDLibrary
{
    public class DrawnActor : Actor
    {
        #region Fields
        #endregion

        #region Properties     
        #endregion

        public DrawnActor(string id, ObjectType objectType, Transform3D transform)
            : base(id, objectType, transform)
        {
        }

        public virtual void Draw(GameTime gameTime)
        {

        }
    }
}
