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

        protected int currentMenuTextureIndex = 0; //0 = main, 1 = volume
        private bool bPaused;
        private int menuState;
        private AdvancedMenuItem mainStart;
        private AdvancedMenuItem mainOptions;
        private AdvancedMenuItem mainQuit;
        private AdvancedMenuItem pauseResume;
        private AdvancedMenuItem pauseRestart;
        private AdvancedMenuItem pauseOptions;
        private AdvancedMenuItem pauseQuit;
        private AdvancedMenuItem optionsAudio;
        private AdvancedMenuItem optionsControls;
        private AdvancedMenuItem optionsReturn;
        private AdvancedMenuItem audioSFXDown;
        private AdvancedMenuItem audioSFXUp;
        private AdvancedMenuItem audioMusicDown;
        private AdvancedMenuItem audioMusicUp;
        private AdvancedMenuItem audioMute;
        private AdvancedMenuItem audioReturn;
        private AdvancedMenuItem controlsReturn;
        private AdvancedMenuItem loseRestart;
        private AdvancedMenuItem loseQuit;
        private AdvancedMenuItem winRestart;
        private AdvancedMenuItem winQuit;

        private int numberBarsSFX;
        private int numberBarsSFXMax;
        private int numberBarsMusic;
        private int numberBarsMusicMax;

        private float volumeSFX;
        private float volumeMusic;

        private bool muted;
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

            this.menuState = MenuData.MenuStateMain;

            this.numberBarsMusicMax = 6;
            this.numberBarsSFXMax = 6;
            this.numberBarsMusic = 3;
            this.numberBarsSFX = 3;

            this.volumeMusic = numberBarsMusic/(float)numberBarsMusicMax;
            this.volumeSFX = numberBarsSFX/(float)numberBarsSFXMax;

            this.muted = false;

            game.SoundManager.SetVolume(volumeMusic, "Music");
            game.SoundManager.SetVolume(volumeSFX, "SFX");

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

        public void OpenMenu(string name)
        {
            if(name.Equals("Pause"))
            {
                menuState = MenuData.MenuStatePause;
                ShowPauseMenuScreen();
            }
            else if(name.Equals("Win"))
            {
                menuState = MenuData.MenuStateWin;
                ShowWinScreen();
            }
            else if(name.Equals("Lose"))
            {
                menuState = MenuData.MenuStateLose;
                ShowLoseScreen();
            }
            ShowMenu();
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

                if(menuState == MenuData.MenuStateAudioMain || menuState == MenuData.MenuStateAudioPause)
                {
                    DrawAudio();
                }

                game.SpriteBatch.End();
            }
            game.Window.Title = "currentScreen" + currentMenuTextureIndex;

            base.Draw(gameTime);
        }

        private void DrawAudio()
        {
            Texture2D texture = null;
            if (muted)
            {
                texture = menuTextures[MenuData.TextureX];
                game.SpriteBatch.Draw(texture, new Vector2(833, 403), Color.White);
            }
            else
            {
                texture = menuTextures[MenuData.TextureButton];
                if (currentMenuTextureIndex == MenuData.TextureAudioMenuSFX)
                {
                    texture = menuTextures[MenuData.TextureButtonHighlighted];
                }
                for (int i = 0; i < numberBarsSFX; i++)
                {
                    game.SpriteBatch.Draw(texture, new Vector2(790 + 30 * i, 150), Color.White);
                }

                texture = menuTextures[MenuData.TextureButton];
                if (currentMenuTextureIndex == MenuData.TextureAudioMenuMusic)
                {
                    texture = menuTextures[MenuData.TextureButtonHighlighted];
                }
                for (int i = 0; i < numberBarsMusic; i++)
                {
                    game.SpriteBatch.Draw(texture, new Vector2(790 + 30 * i, 283), Color.White);
                }
            }
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
            bool hover = false;
           for(int i = 0; i < menuItemList.Count; i++)
           {
               MenuItem item = menuItemList[i];

               //is the mouse over the item?
               if (this.game.MouseManager.Bounds.Intersects(item.Bounds)) 
               {
                    hover = true;
                    item.SetActive(true);
                    if(item is AdvancedMenuItem)
                    {
                        if (game.MouseManager.IsLeftButtonClickedOnce())
                        {
                            DoMenuAction(menuItemList[i].Name);
                            break;
                        }
                        else
                        { 
                            DoMenuHighlight(item.Name);
                            break;
                        }

                    }

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
           if(!hover)
           {
                DoMenuHighlight("Unhighlight");
           }
        }

        private void DoMenuHighlight(string name)
        {
            if (name.Equals("Unhighlight"))
            {
                if (menuState == MenuData.MenuStateMain)
                    currentMenuTextureIndex = MenuData.TextureMainMenu;
                else if (menuState == MenuData.MenuStatePause)
                    currentMenuTextureIndex = MenuData.TexturePauseMenu;
                else if (menuState == MenuData.MenuStateAudioMain || menuState == MenuData.MenuStateAudioPause)
                    currentMenuTextureIndex = MenuData.TextureAudio;
                else if (menuState == MenuData.MenuStateControlsMain || menuState == MenuData.MenuStateControlsPause)
                    currentMenuTextureIndex = MenuData.TextureControlsMenu;
                else if (menuState == MenuData.MenuStateOptionsMain || menuState == MenuData.MenuStateOptionsPause)
                    currentMenuTextureIndex = MenuData.TextureOptionsMenu;
                else if (menuState == MenuData.MenuStateLose)
                    currentMenuTextureIndex = MenuData.TextureYouLose;
                else if (menuState == MenuData.MenuStateWin)
                    currentMenuTextureIndex = MenuData.TextureYouWin;
            }
            else if (name.Equals(MenuData.Menu_Play))
            {
                if (menuState == MenuData.MenuStateMain)
                    currentMenuTextureIndex = MenuData.TextureMainMEnuStart;
                else if (menuState == MenuData.MenuStatePause)
                    currentMenuTextureIndex = MenuData.TexturePauseMenuResume;
            }
            else if (name.Equals(MenuData.StringMenuRestart))
            {
                if (menuState == MenuData.MenuStatePause)
                    currentMenuTextureIndex = MenuData.TexturePauseMenuRestart;
                else if (menuState == MenuData.MenuStateWin)
                    currentMenuTextureIndex = MenuData.TextureYouWinRestart;
                else if (menuState == MenuData.MenuStateLose)
                    currentMenuTextureIndex = MenuData.TextureYouLoseRestart;
            }
            else if (name.Equals(MenuData.StringMenuResume))
            {
                currentMenuTextureIndex = MenuData.TexturePauseMenuResume;
            }
            else if (name.Equals(MenuData.StringMenuOptions))
            {
                if (menuState == MenuData.MenuStateMain)
                    currentMenuTextureIndex = MenuData.TextureMainMenuOptions;
                else if (menuState == MenuData.MenuStatePause)
                    currentMenuTextureIndex = MenuData.TexturePauseMenuOptions;
            }
            else if (name.Equals(MenuData.StringMenuAudio))
            {
                currentMenuTextureIndex = MenuData.TextureOptionsMenuAudio;
            }
            else if (name.Equals(MenuData.StringMenuControls))
            {
                currentMenuTextureIndex = MenuData.TextureOptionsMenuControls;
            }
            else if (name.Equals(MenuData.StringMenuExit))
            {
                if (menuState == MenuData.MenuStateMain)
                    currentMenuTextureIndex = MenuData.TextureMainMenuQuit;
                else if (menuState == MenuData.MenuStatePause)
                    currentMenuTextureIndex = MenuData.TexturePauseMenuQuit;
                else if (menuState == MenuData.MenuStateWin)
                    currentMenuTextureIndex = MenuData.TextureYouWinQuit;
                else if (menuState == MenuData.MenuStateLose)
                    currentMenuTextureIndex = MenuData.TextureYouLoseQuit;
            }
            else if (name.Equals(MenuData.StringMenuSFXUp) || name.Equals(MenuData.StringMenuSFXDown))
            {
                currentMenuTextureIndex = MenuData.TextureAudioMenuSFX;
            }
            else if (name.Equals(MenuData.StringMenuMusicUp) || name.Equals(MenuData.StringMenuMusicDown))
            {
                currentMenuTextureIndex = MenuData.TextureAudioMenuMusic;
            }
            else if (name.Equals(MenuData.StringMenuMute))
            {
                currentMenuTextureIndex = MenuData.TextureAudioMenuMute;
            }
            else if (name.Equals(MenuData.StringMenuBack))
            {
                if (menuState == MenuData.MenuStateAudioMain || menuState == MenuData.MenuStateAudioPause)
                    currentMenuTextureIndex = MenuData.TextureAudioMenuReturn;
                else if (menuState == MenuData.MenuStateControlsMain || menuState == MenuData.MenuStateControlsPause)
                    currentMenuTextureIndex = MenuData.TextureControlsMenuReturn;
                else if (menuState == MenuData.MenuStateOptionsMain || menuState == MenuData.MenuStateOptionsPause)
                    currentMenuTextureIndex = MenuData.TextureOptionsMenuReturn;
            }
            else
            {
                Console.WriteLine("There is a case not covered!");
            }
        }



        //to do - dispose, clone
        #endregion

        #region Code specific to your application
        private void InitialiseMenuOptions()
        {
            //add the menu items to the list
            //Main Menu
            this.mainStart = new AdvancedMenuItem(MenuData.Menu_Play, "", MenuData.BoundsMainStart, MenuData.ColorMenuInactive, MenuData.ColorMenuActive);
            this.mainOptions = new AdvancedMenuItem(MenuData.StringMenuOptions, "", MenuData.BoundsMainOptions, MenuData.ColorMenuInactive, MenuData.ColorMenuActive);
            this.mainQuit = new AdvancedMenuItem(MenuData.StringMenuExit, "", MenuData.BoundsMainQuit, MenuData.ColorMenuInactive, MenuData.ColorMenuActive);

            //Pause Menu
            this.pauseResume = new AdvancedMenuItem(MenuData.StringMenuResume, "", MenuData.BoundsPauseResume, MenuData.ColorMenuInactive, MenuData.ColorMenuActive);
            this.pauseRestart = new AdvancedMenuItem(MenuData.StringMenuRestart, "", MenuData.BoundsPauseRestart, MenuData.ColorMenuInactive, MenuData.ColorMenuActive);
            this.pauseOptions= new AdvancedMenuItem(MenuData.StringMenuOptions, "", MenuData.BoundsPauseOptions, MenuData.ColorMenuInactive, MenuData.ColorMenuActive);
            this.pauseQuit = new AdvancedMenuItem(MenuData.StringMenuExit, "", MenuData.BoundsPauseQuit, MenuData.ColorMenuInactive, MenuData.ColorMenuActive);

            //Options Menu
            this.optionsAudio = new AdvancedMenuItem(MenuData.StringMenuAudio, "", MenuData.BoundsOptionsAudio, MenuData.ColorMenuInactive, MenuData.ColorMenuActive);
            this.optionsControls = new AdvancedMenuItem(MenuData.StringMenuControls, "", MenuData.BoundsOptionsControls, MenuData.ColorMenuInactive, MenuData.ColorMenuActive);
            this.optionsReturn = new AdvancedMenuItem(MenuData.StringMenuBack, "", MenuData.BoundsOptionsReturn, MenuData.ColorMenuInactive, MenuData.ColorMenuActive);

            //Audio Menu
            this.audioSFXDown = new AdvancedMenuItem(MenuData.StringMenuSFXDown, "", MenuData.BoundsAudioSFXDown, MenuData.ColorMenuInactive, MenuData.ColorMenuActive);
            this.audioSFXUp = new AdvancedMenuItem(MenuData.StringMenuSFXUp, "", MenuData.BoundsAudioSFXUp, MenuData.ColorMenuInactive, MenuData.ColorMenuActive);
            this.audioMusicDown = new AdvancedMenuItem(MenuData.StringMenuMusicDown, "", MenuData.BoundsAudioMusicDown, MenuData.ColorMenuInactive, MenuData.ColorMenuActive);
            this.audioMusicUp = new AdvancedMenuItem(MenuData.StringMenuMusicUp, "", MenuData.BoundsAudioMusicUp, MenuData.ColorMenuInactive, MenuData.ColorMenuActive);
            this.audioMute = new AdvancedMenuItem(MenuData.StringMenuMute, "", MenuData.BoundsAudioMute, MenuData.ColorMenuInactive, MenuData.ColorMenuActive);
            this.audioReturn = new AdvancedMenuItem(MenuData.StringMenuBack, "", MenuData.BoundsAudioReturn, MenuData.ColorMenuInactive, MenuData.ColorMenuActive);

            //Controls Menu
            this.controlsReturn = new AdvancedMenuItem(MenuData.StringMenuBack, "", MenuData.BoundsControlsReturn, MenuData.ColorMenuInactive, MenuData.ColorMenuActive);

            //Lose
            this.loseRestart = new AdvancedMenuItem(MenuData.StringMenuRestart, "", MenuData.BoundsLoseRestart, MenuData.ColorMenuInactive, MenuData.ColorMenuActive);
            this.loseQuit = new AdvancedMenuItem(MenuData.StringMenuExit, "", MenuData.BoundsLoseQuit, MenuData.ColorMenuInactive, MenuData.ColorMenuActive);

            //Win
            this.winRestart = new AdvancedMenuItem(MenuData.StringMenuRestart, "", MenuData.BoundsWinRestart, MenuData.ColorMenuInactive, MenuData.ColorMenuActive);
            this.winQuit = new AdvancedMenuItem(MenuData.StringMenuExit, "", MenuData.BoundsWinQuit, MenuData.ColorMenuInactive, MenuData.ColorMenuActive);

            //add your new menu options here...
        }
        //perform whatever actions are listed on the menu
        private void DoMenuAction(String name)
        {
            if (name.Equals(MenuData.Menu_Play))
            {
                HideMenu();
                game.SoundManager.PlayCue("Menu_Select");
            }
            else if (name.Equals(MenuData.StringMenuRestart))
            {
                game.Reset();
                game.SoundManager.PlayCue("Menu_Select");
            }
            else if (name.Equals(MenuData.StringMenuResume))
            {
                HideMenu();
                game.SoundManager.PlayCue("Menu_Select");
            }
            else if(name.Equals(MenuData.StringMenuOptions))
            {
                ShowOptionsMenuScreen();
                menuState = menuState == MenuData.MenuStateMain ? MenuData.MenuStateOptionsMain : MenuData.MenuStateOptionsPause;
                game.SoundManager.PlayCue("Menu_Select");
            }
            else if (name.Equals(MenuData.StringMenuAudio))
            {
                ShowAudioMenuScreen();
                if (menuState == MenuData.MenuStateOptionsMain)
                    menuState = MenuData.MenuStateAudioMain;
                else if (menuState == MenuData.MenuStateOptionsPause)
                    menuState = MenuData.MenuStateAudioPause;
                game.SoundManager.PlayCue("Menu_Select");
            }
            else if (name.Equals(MenuData.StringMenuControls))
            {
                ShowControlsMenuScreen();
                if (menuState == MenuData.MenuStateOptionsMain)
                    menuState = MenuData.MenuStateControlsMain;
                else if (menuState == MenuData.MenuStateOptionsPause)
                    menuState = MenuData.MenuStateControlsPause;
                game.SoundManager.PlayCue("Menu_Select");
            }
            else if (name.Equals(MenuData.StringMenuExit))
            {
                game.Exit();
                game.SoundManager.PlayCue("Menu_Select");
            }
            else if (name.Equals(MenuData.StringMenuSFXUp))
            {
                IncreaseSFX();
                game.SoundManager.PlayCue("Menu_Select");
            }
            else if (name.Equals(MenuData.StringMenuSFXDown))
            {
                DecreaseSFX();
                game.SoundManager.PlayCue("Menu_Select");
            }
            else if (name.Equals(MenuData.StringMenuMusicUp))
            {
                IncreaseMusic();
                game.SoundManager.PlayCue("Menu_Select");
            }
            else if (name.Equals(MenuData.StringMenuMusicDown))
            {
                DecreaseMusic();
                game.SoundManager.PlayCue("Menu_Select");
            }
            else if (name.Equals(MenuData.StringMenuMute))
            {
                if (muted)
                {
                    Unmute();
                }
                else
                {
                    Mute();
                }
                game.SoundManager.PlayCue("Menu_Select");
            }
            else if (name.Equals(MenuData.StringMenuBack))
            {
                if (menuState == MenuData.MenuStateAudioMain || menuState == MenuData.MenuStateAudioPause)
                {
                    ShowOptionsMenuScreen();
                    menuState = menuState == MenuData.MenuStateAudioMain ? MenuData.MenuStateOptionsMain : MenuData.MenuStateOptionsPause;
                }
                else if (menuState == MenuData.MenuStateControlsMain || menuState == MenuData.MenuStateControlsPause)
                {
                    ShowOptionsMenuScreen();
                    menuState = menuState == MenuData.MenuStateControlsMain ? MenuData.MenuStateOptionsMain : MenuData.MenuStateOptionsPause;
                }
                else if (menuState == MenuData.MenuStateOptionsMain)
                {
                    ShowMainMenuScreen();
                    menuState = MenuData.MenuStateMain;
                }
                else if(menuState == MenuData.MenuStateOptionsPause)
                {
                    ShowPauseMenuScreen();
                    menuState = MenuData.MenuStatePause;
                }
                game.SoundManager.PlayCue("Menu_Select");
            }
            else
            {
                Console.WriteLine("There is a case not covered!");
            }

            //add your new menu actions here...
        }

        private void Mute()
        {
            game.SoundManager.SetVolume(0, "SFX");
            game.SoundManager.SetVolume(0, "Music");
            muted = true;
        }

        private void Unmute()
        {
            game.SoundManager.SetVolume(volumeSFX, "SFX");
            game.SoundManager.SetVolume(volumeMusic, "Music");
            muted = false;
        }

        private void DecreaseMusic()
        {
            muted = false;
            numberBarsMusic--;
            if (numberBarsMusic < 0)
            {
                numberBarsMusic = 0;
            }
            volumeMusic = numberBarsMusic / (float)numberBarsMusicMax;
            game.SoundManager.SetVolume(volumeMusic, "Music");
        }

        private void IncreaseMusic()
        {
            muted = false;
            numberBarsMusic++;
            if (numberBarsMusic > numberBarsMusicMax)
            {
                numberBarsMusic = numberBarsMusicMax;
            }
            volumeMusic = numberBarsMusic / (float)numberBarsMusicMax;
            game.SoundManager.SetVolume(volumeMusic, "Music");
        }

        private void DecreaseSFX()
        {
            muted = false;
            numberBarsSFX--;
            if (numberBarsSFX < 0)
            {
                numberBarsSFX = 0;
            }
            volumeSFX = numberBarsSFX / (float)numberBarsSFXMax;
            game.SoundManager.SetVolume(volumeSFX, "SFX");
        }

        private void IncreaseSFX()
        {
            muted = false;
            numberBarsSFX++;
            if(numberBarsSFX > numberBarsSFXMax)
            {
                numberBarsSFX = numberBarsSFXMax;
            }
            volumeSFX = numberBarsSFX / (float)numberBarsSFXMax;
            game.SoundManager.SetVolume(volumeSFX, "SFX");
        }

        private void ShowControlsMenuScreen()
        {
            RemoveAll();

            Add(controlsReturn);

            currentMenuTextureIndex = MenuData.TextureControlsMenu;
        }

        private void ShowOptionsMenuScreen()
        {
            RemoveAll();

            Add(optionsAudio);
            Add(optionsControls);
            Add(optionsReturn);

            currentMenuTextureIndex = MenuData.TextureOptionsMenu;
        }

        private void ShowMainMenuScreen()
        {
            //remove any items in the menu
            RemoveAll();
            //add the appropriate items
            Add(mainStart);
            Add(mainOptions);
            Add(mainQuit);
            //set the background texture
            currentMenuTextureIndex = MenuData.TextureMainMenu;
        }

        private void ShowPauseMenuScreen()
        {
            RemoveAll();

            Add(pauseResume);
            Add(pauseRestart);
            Add(pauseOptions);
            Add(pauseQuit);

            currentMenuTextureIndex = MenuData.TexturePauseMenu;
        }

        private void ShowAudioMenuScreen()
        {
            //remove any items in the menu
            RemoveAll();
            //add the appropriate items
            Add(audioSFXDown);
            Add(audioSFXUp);
            Add(audioMusicDown);
            Add(audioMusicUp);
            Add(audioMute);
            Add(audioReturn);
            //set the background texture
            currentMenuTextureIndex = MenuData.TextureAudio;
        }

        private void ShowLoseScreen()
        {
            RemoveAll();

            Add(loseRestart);
            Add(loseQuit);

            currentMenuTextureIndex = MenuData.TextureYouLose;
        }

        private void ShowWinScreen()
        {
            RemoveAll();

            Add(winRestart);
            Add(winQuit);

            currentMenuTextureIndex = MenuData.TextureYouWin;
        }

        #endregion
    }
}
