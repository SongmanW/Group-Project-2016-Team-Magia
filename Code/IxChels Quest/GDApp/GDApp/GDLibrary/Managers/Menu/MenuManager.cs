using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GDApp;


namespace GDLibrary
{
    public class MenuManager: DrawableGameComponent
    {
        #region Variables
        private List<MenuItem> menuItemList;
        private Main game;
        private SpriteFont menuFont;
        private Color menuTextureBlendColor;

        private Texture2D[] menuTextures;
        private Rectangle textureRectangle;

        private MenuItem menuPlay, menuRestart, menuExit, menuAudio, menuControls;
        private MenuItem menuVolumeUp, menuVolumeDown, menuVolumeMute, menuBack;
        private MenuItem menuExitYes, menuExitNo;

        protected int currentMenuTextureIndex = 0; //0 = main, 1 = volume
        private bool bPaused;
        #endregion

        #region Properties
        public Color MenuTextureBlendColor
        {
            get
            {
                return menuTextureBlendColor;
            }
            set
            {
                menuTextureBlendColor = value;
            }
        }
        public bool Pause
        {
            get
            {
                return bPaused;
            }
            set
            {
                bPaused = value;
            }
        }
        #endregion

        #region Core menu manager - No need to change this code
        public MenuManager(Main game, Texture2D[] menuTextures, 
            SpriteFont menuFont, Integer2 textureBorderPadding,
            Color menuTextureBlendColor) 
            : base(game)
        {
            this.game = game;

            //load the textures
            this.menuTextures = menuTextures;

            //background blend color for the menu
            this.menuTextureBlendColor = menuTextureBlendColor;

            //menu font
            this.menuFont = menuFont;

            //stores all menu item (e.g. Save, Resume, Exit) objects
            this.menuItemList = new List<MenuItem>();
               
            //set the texture background to occupy the entire screen dimension, less any padding
            this.textureRectangle = game.ScreenRectangle;

            //deflate the texture rectangle by the padding required
            this.textureRectangle.Inflate(-textureBorderPadding.X, -textureBorderPadding.Y);

            //show the menu
            ShowMenu();
        }
        public override void Initialize()
        {
            //add the basic items - "Resume", "Save", "Exit"
           InitialiseMenuOptions();

           //show the menu screen
           ShowMainMenuScreen();

           base.Initialize();
        }
                                                                                                                               
        public void Add(MenuItem theMenuItem) 
        {
            menuItemList.Add(theMenuItem);
        }

        public void Remove(MenuItem theMenuItem)
        {
            menuItemList.Remove(theMenuItem);
        }

        public void RemoveAll()
        {
            menuItemList.Clear();
        }

        public override void Update(GameTime gameTime)
        {
            TestIfPaused();

            //menu is not paused so show and process
            if (!bPaused)
                ProcessMenuItemList();

         
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (!bPaused)
            {
                //enable alpha blending on the menu objects
                this.game.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, DepthStencilState.None, null);
                //draw whatever background we expect to see based on what menu or sub-menu we are viewing
                game.SpriteBatch.Draw(menuTextures[currentMenuTextureIndex], textureRectangle, this.menuTextureBlendColor);

                //draw the text on top of the background
                for (int i = 0; i < menuItemList.Count; i++)
                {
                    menuItemList[i].Draw(game.SpriteBatch, menuFont);
                }

                game.SpriteBatch.End();
            }

            base.Draw(gameTime);
        }

        private void TestIfPaused()
        {
            //if menu is pause and we press the show menu button then show the menu
            if((bPaused) && this.game.KeyboardManager.IsFirstKeyPress(
                KeyData.KeyPauseShowMenu))
            {
                ShowMenu();
            }
        }

        private void ShowMenu()
        {
            //show the menu by setting pause to false
            bPaused = false;
            //generate an event to tell the object manager to pause
            EventDispatcher.Publish(new EventData("menu event", this, EventType.OnPause, EventCategoryType.MainMenu));

            //if the mouse is invisible then show it
            if (!this.game.IsMouseVisible)
                this.game.IsMouseVisible = true;
        }

        private void HideMenu()
        {
            //hide the menu by setting pause to true
            bPaused = true;
            //generate an event to tell the object manager to unpause
            EventDispatcher.Publish(new EventData("menu event", this, EventType.OnPlay, EventCategoryType.MainMenu));

            //if the mouse is invisible then show it
            if (this.game.IsMouseVisible)
                this.game.IsMouseVisible = false;

            this.game.MouseManager.SetPosition(this.game.ScreenCentre);
        }

        private void RestartGame()
        {
            //hide the menu by setting pause to true
            bPaused = true;

            //generate an event to tell the main method to restart
            EventDispatcher.Publish(new EventData("menu event", this, EventType.OnRestart, EventCategoryType.MainMenu));

            //if the mouse is invisible then show it
            if (this.game.IsMouseVisible)
                this.game.IsMouseVisible = false;
        }

        private void ExitGame()
        {
            //generate an event to tell the main method to exit - need to add code in main to catch this event
            EventDispatcher.Publish(new EventData("menu event", this, EventType.OnExit, EventCategoryType.MainMenu));
        }

        //iterate through each menu item and see if it is "highlighted" or "highlighted and clicked upon"
        private void ProcessMenuItemList()
        {
           for(int i = 0; i < menuItemList.Count; i++)
           {
               MenuItem item = menuItemList[i];

               //is the mouse over the item?
               if (this.game.MouseManager.Bounds.Intersects(item.Bounds)) 
               {
                   item.SetActive(true);

                   //is the left mouse button clicked
                   if (game.MouseManager.IsLeftButtonClickedOnce())
                   {
                       DoMenuAction(menuItemList[i].Name);
                       break;
                   }
               }
               else
               {
                   item.SetActive(false);
               }
           }
        }

        //to do - dispose, clone
        #endregion

        #region Code specific to your application
        private void InitialiseMenuOptions()
        {
            //add the menu items to the list
            this.menuPlay = new MenuItem(MenuData.Menu_Play, MenuData.Menu_Play,
                MenuData.BoundsMenuPlay, MenuData.ColorMenuInactive, MenuData.ColorMenuActive);
            this.menuRestart = new MenuItem(MenuData.StringMenuRestart, MenuData.StringMenuRestart,
                MenuData.BoundsMenuRestart, MenuData.ColorMenuInactive, MenuData.ColorMenuActive);
            this.menuAudio = new MenuItem(MenuData.StringMenuAudio, MenuData.StringMenuAudio,
               MenuData.BoundsMenuAudio, MenuData.ColorMenuInactive, MenuData.ColorMenuActive);
            this.menuControls = new MenuItem(MenuData.StringMenuControls, MenuData.StringMenuControls,
              MenuData.BoundsMenuControls, MenuData.ColorMenuInactive, MenuData.ColorMenuActive);
            this.menuExit = new MenuItem(MenuData.StringMenuExit, MenuData.StringMenuExit,
                MenuData.BoundsMenuExit, MenuData.ColorMenuInactive, MenuData.ColorMenuActive);

            //second menu - audio settings
            this.menuVolumeUp = new MenuItem(MenuData.StringMenuVolumeUp, MenuData.StringMenuVolumeUp,
                MenuData.BoundsMenuVolumeUp, MenuData.ColorMenuInactive, MenuData.ColorMenuActive);
            this.menuVolumeDown = new MenuItem(MenuData.StringMenuVolumeDown, MenuData.StringMenuVolumeDown,
               MenuData.BoundsMenuVolumeDown, MenuData.ColorMenuInactive, MenuData.ColorMenuActive);
            this.menuVolumeMute = new MenuItem(MenuData.StringMenuVolumeMute, MenuData.StringMenuVolumeMute,
            MenuData.BoundsMenuVolumeMute, MenuData.ColorMenuInactive, MenuData.ColorMenuActive);

            this.menuBack = new MenuItem(MenuData.StringMenuBack, MenuData.StringMenuBack,
                 MenuData.BoundsMenuBack, MenuData.ColorMenuInactive, MenuData.ColorMenuActive);

            this.menuExitYes = new MenuItem(MenuData.StringMenuExitYes, MenuData.StringMenuExitYes,
             MenuData.BoundsMenuExitYes, MenuData.ColorMenuInactive, MenuData.ColorMenuActive);
            this.menuExitNo = new MenuItem(MenuData.StringMenuExitNo, MenuData.StringMenuExitNo,
                MenuData.BoundsMenuExitNo, MenuData.ColorMenuInactive, MenuData.ColorMenuActive);

            //add your new menu options here...
        }
        //perform whatever actions are listed on the menu
        private void DoMenuAction(String name)
        {
            if (name.Equals(MenuData.Menu_Play))
            {
                HideMenu();
            }
            else if (name.Equals(MenuData.StringMenuRestart))
            {
                RestartGame();
            }
            else if (name.Equals(MenuData.StringMenuAudio))
            {
                ShowAudioMenuScreen();
            }
            else if (name.Equals(MenuData.StringMenuControls))
            {
                ShowControlsMenuScreen();
            }
            else if (name.Equals(MenuData.StringMenuExit))
            {
                ShowExitMenuScreen();       
            }
            else if (name.Equals(MenuData.StringMenuVolumeUp))
            {
                //publish an event to be received by the sound manager
            }
            else if (name.Equals(MenuData.StringMenuVolumeDown))
            {
                //publish an event to be received by the sound manager
            }
            else if (name.Equals(MenuData.StringMenuVolumeMute))
            {
                //publish an event to be received by the sound manager
            }
            else if (name.Equals(MenuData.StringMenuBack))
            {
                ShowMainMenuScreen();
            }
            else if (name.Equals(MenuData.StringMenuExitYes))
            {
                ExitGame();
            }
            else if (name.Equals(MenuData.StringMenuExitNo))
            {
                ShowMainMenuScreen();
            }

            //add your new menu actions here...
        }

        private void ShowControlsMenuScreen()
        {
            //remove any items in the menu
            RemoveAll();
            //add the appropriate items
            Add(menuBack);
            //set the background texture
            currentMenuTextureIndex = MenuData.TextureIndexControlsMenu;
        }

        //add your ShowXMenuScreen() methods here...

        private void ShowMainMenuScreen()
        {
            //remove any items in the menu
            RemoveAll();
            //add the appropriate items
            Add(menuPlay);
            Add(menuRestart);
            Add(menuAudio);
            Add(menuControls);
            Add(menuExit);
            //set the background texture
            currentMenuTextureIndex = MenuData.TextureIndexMainMenu;
        }

        private void ShowAudioMenuScreen()
        {
            //remove any items in the menu
            RemoveAll();
            //add the appropriate items
            Add(menuVolumeUp);
            Add(menuVolumeDown);
            Add(menuVolumeMute);
            Add(menuBack);
            //set the background texture
            currentMenuTextureIndex = MenuData.TextureIndexAudioMenu;
        }

        private void ShowExitMenuScreen()
        {
            //remove any items in the menu
            RemoveAll();
            //add the appropriate items
            Add(menuExitYes);
            Add(menuExitNo);
            //set the background texture
            currentMenuTextureIndex = MenuData.TextureIndexExitMenu;
        }

        #endregion
    }
}
