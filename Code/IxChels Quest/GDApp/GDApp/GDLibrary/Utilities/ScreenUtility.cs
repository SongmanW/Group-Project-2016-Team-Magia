using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDLibrary
{
    public class ScreenUtility
    {
        //returns a viewport with padding on horizontal and vertical edges - used by cameras typically - see Main::InitialiseCamera()
        public static Viewport GetPaddedViewport(Viewport viewport, int horizontalPadding, int verticalPadding)
        {
            //get the rectangle dimensions for the viewport provided (e.g. normally the fullscreen viewport)
            Rectangle screenRectangle = viewport.Bounds;

            //reduce it by whatever padding has been specified
            screenRectangle.Inflate(-horizontalPadding, -verticalPadding);

            //create a new smaller (because of padding) viewport and return
            return new Viewport(screenRectangle);
        }
    }
}
