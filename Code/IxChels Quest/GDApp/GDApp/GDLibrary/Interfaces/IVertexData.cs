using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDLibrary
{
    public interface IVertexData
    {
        void Draw(GameTime gameTime, Effect effect);
    }
}
