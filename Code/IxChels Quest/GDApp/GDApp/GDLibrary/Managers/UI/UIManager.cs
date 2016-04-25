using GDApp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GDLibrary
{
    public class UIManager : GenericDraweableManager<UIActor>
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        public UIManager(Main game, int initialDrawSize, int initialRemoveSize)
            : base(game, initialDrawSize, initialRemoveSize)
        {
            #region Event Handling
            //add any class specific event handling methods here
            #endregion
        }


        #region Event Handling
        //add any class specific event handling methods here   
        #endregion

        public override void Draw(GameTime gameTime)
        {
            UIActor actor = null;
            if (!this.IsPaused)
            {
                (this.Game as Main).SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

                for (int i = 0; i < Size; i++)
                {
                    actor = this[i];

                    if (actor.IsVisible)
                        actor.Draw(gameTime);
                }

                (this.Game as Main).SpriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
