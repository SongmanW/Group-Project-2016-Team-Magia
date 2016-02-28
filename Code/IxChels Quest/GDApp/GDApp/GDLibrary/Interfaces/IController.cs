using Microsoft.Xna.Framework;

namespace GDLibrary
{
    public interface IController
    {
        string GetName();
        Actor GetParentActor();
        void SetParentActor(Actor parentActor);

        void Update(GameTime gameTime);
        object Clone();
    }
}
