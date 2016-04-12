using Microsoft.Xna.Framework;

namespace GDLibrary
{
    public interface IController
    {
        string GetName();
        Actor GetParentActor();
        void SetParentActor(Actor parentActor);

        bool isEnabled();
        void Update(GameTime gameTime);
        object Clone();
    }
}
