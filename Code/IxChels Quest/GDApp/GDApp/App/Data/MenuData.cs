using System;
using Microsoft.Xna.Framework;


namespace GDLibrary
{
    public class MenuData
    {
        #region Main Menu Strings
        //all the strings shown to the user through the menu
        public static String Game_Title = "IxChels Quest";
        public static String Menu_Play = "Start";
        public static string StringMenuRestart = "Restart Game";
        public static string StringMenuResume = "Resume Game";
        public static string StringMenuOptions = "Options";
        public static String StringMenuAudio = "Audio";
        public static String StringMenuControls = "Controls";
        public static String StringMenuExit = "Exit Game";

        public static String StringMenuSFXUp = "SFX Up";
        public static String StringMenuSFXDown = "SFX Down";
        public static String StringMenuMusicUp = "Music Up";
        public static String StringMenuMusicDown = "Music Down";
        public static string StringMenuMute = "Mute";
        public static String StringMenuBack = "Return";
        #endregion

        #region MenuStates
        public static int MenuStateMain = 0;
        public static int MenuStateOptionsMain = 1;
        public static int MenuStateAudioMain = 2;
        public static int MenuStateControlsMain = 3;
        public static int MenuStatePause = 4;
        public static int MenuStateOptionsPause = 5;
        public static int MenuStateAudioPause = 6;
        public static int MenuStateControlsPause = 7;
        public static int MenuStateWin = 8;
        public static int MenuStateLose = 9;
        #endregion

        #region Colours, Padding, Texture transparency , Array Indices and Bounds
        public static Integer2 MenuTexturePadding = new Integer2(0, 0);
        public static Color MenuTextureColor = new Color(1, 1, 1, 1f);

        //the hover colours for menu items
        public static Color ColorMenuInactive = Color.Black;
        public static Color ColorMenuActive = Color.Red;

        //the position of the texture in the array of textures provided to the menu manager          
        public static int TextureButton = 0;
        public static int TextureButtonHighlighted = 1;
        public static int TextureAudio = 2;
        public static int TextureAudioMenuMusic = 3;
        public static int TextureAudioMenuMute = 4;
        public static int TextureAudioMenuReturn = 5;
        public static int TextureAudioMenuSFX = 6;
        public static int TextureControlsMenu = 7;
        public static int TextureControlsMenuReturn = 8;
        public static int TextureMainMenu = 9;
        public static int TextureMainMenuOptions = 10;
        public static int TextureMainMenuQuit = 11;
        public static int TextureMainMEnuStart = 12;
        public static int TextureOptionsMenu = 13;
        public static int TextureOptionsMenuAudio = 14;
        public static int TextureOptionsMenuControls = 15;
        public static int TextureOptionsMenuReturn = 16;
        public static int TexturePauseMenu = 17;
        public static int TexturePauseMenuOptions = 18;
        public static int TexturePauseMenuQuit = 19;
        public static int TexturePauseMenuRestart = 20;
        public static int TexturePauseMenuResume = 21;
        public static int TextureYouLose = 22;
        public static int TextureYouLoseQuit = 23;
        public static int TextureYouLoseRestart = 24;
        public static int TextureYouWin = 25;
        public static int TextureYouWinQuit = 26;
        public static int TextureYouWinRestart = 27;
        public static int TextureX = 28;

        //bounding rectangles used to detect mouse over
        public static Rectangle BoundsMainStart = new Rectangle(258, 237, 488, 123);
        public static Rectangle BoundsMainOptions = new Rectangle(258, 365, 488, 123);
        public static Rectangle BoundsMainQuit = new Rectangle(258, 493, 488, 123);

        public static Rectangle BoundsPauseResume = new Rectangle(266, 144, 488, 123);
        public static Rectangle BoundsPauseRestart = new Rectangle(266, 252, 488, 123);
        public static Rectangle BoundsPauseOptions = new Rectangle(266, 380, 488, 123);
        public static Rectangle BoundsPauseQuit = new Rectangle(266, 508, 488, 123);

        public static Rectangle BoundsOptionsAudio = new Rectangle(255, 199, 488, 123);
        public static Rectangle BoundsOptionsControls = new Rectangle(255, 327, 488, 123);
        public static Rectangle BoundsOptionsReturn = new Rectangle(255, 455, 488, 123);

        public static Rectangle BoundsAudioSFXDown = new Rectangle(746, 150, 40, 67);
        public static Rectangle BoundsAudioSFXUp = new Rectangle(975, 152, 40, 67);
        public static Rectangle BoundsAudioMusicDown = new Rectangle(746, 283, 40, 67);
        public static Rectangle BoundsAudioMusicUp = new Rectangle(975, 285, 40, 67);
        public static Rectangle BoundsAudioMute = new Rectangle(823, 392, 113, 104);
        public static Rectangle BoundsAudioReturn = new Rectangle(251, 516, 488, 123);

        public static Rectangle BoundsControlsReturn = new Rectangle(257, 625, 488, 123);

        public static Rectangle BoundsLoseRestart = new Rectangle(21, 625, 488, 123);
        public static Rectangle BoundsLoseQuit = new Rectangle(519, 625, 488, 123);

        public static Rectangle BoundsWinRestart = new Rectangle(21, 625, 488, 123);
        public static Rectangle BoundsWinQuit = new Rectangle(519, 625, 488, 123);

        #endregion

        #region UI Menu
        public static String UI_Menu_AddHouse = "Add House";
        public static string UI_Menu_AddBarracks = "Add Barracks";
        public static string UI_Menu_AddFence = "Add Fence";

        public static Rectangle UI_Menu_AddHouse_Bounds = new Rectangle(40, 380, 90, 20);
        public static Rectangle UI_Menu_AddBarracks_Bounds = new Rectangle(40, 400, 120, 20);
        public static Rectangle UI_Menu_AddFence_Bounds = new Rectangle(40, 420, 90, 20);


        #endregion

    }
}
