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

        public override void Remove()
        {
            //remove from objectManager
            game.ObjectManager.Remove(this);

            //nullify any expensive objects
            base.Remove();
        }
    }
}
