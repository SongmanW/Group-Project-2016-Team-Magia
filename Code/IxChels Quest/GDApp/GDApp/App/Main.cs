using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GDApp.GDLibrary;
using GDLibrary;
using System;
using JigLibX.Geometry;
using JigLibX.Collision;

/*
 To Do:
 * Add debugdrawer to show FPS, camera type etc
 */

/*
 * Version:11.1
 *==============
 * Author: STWL
 * Revisions:
 *  - Added RotatorController
 *  - Added TransitionController (unfinished)
 *  - Added PawnModelObject and ModelObject.Clone(), .Texture and .Model
 */

/*
* Version: 11
 =============
* Author: NMCG
* Revisions:
* - Added first version of the EventDispatcher
* Bugs:
* - Jitter on Curve3D possibly related to Curve1D tangents
* - RailCamera3D shows bug when rail passes over the object - up vector?
* Fixes:
* - None
* Addition:
* - Have RailCamera3D lag the target object slightly - configurable
* - Test clone for ModelObject and MoveableModelObject
*/

/*
 * Version: 10
  =============
 * Author: NMCG
 * Revisions:
 * - Added IController, Controller, PawnCamera3D, and a rail camera controller example
 * - Added more VertexFactory methods, added security and 3rd person camera
 * Bugs:
 * - Jitter on Curve3D possibly related to Curve1D tangents
 * - RailCamera3D shows bug when rail passes over the object - up vector?
 * Fixes:
 * - None
 * Addition:
 * - Have RailCamera3D lag the target object slightly - configurable
 * - Test clone for ModelObject and MoveableModelObject
 */
/*
    Version: 9.1
    =============
    Author: STWL
    Revisions:
    - Added CharacterModelObject (movement is dependant on Camera (needs to be set to get new Movements))
    - Added InitializeLevel (to set initial CharacterModel to first Camera)
    - Added CameraUtility.GetCameraToTarget(Vector3 parent, Transform3D camera)
    - Added RailCharacterFollowCamera3D
    - Added Transform3D.RotateTo(Vector3 rotateTo)
    - Added CharacterRotation upon movement
    Bugs:
    Fixes:
 */

/*
 * Version: 9
  =============
 * Author: NMCG
 * Revisions:
 * - Added Curve3D and TrackCamera3D
 * Bugs:
 * - Jitter on Curve3D possibly related to Curve1D tangents
 * - RailCamera3D shows bug when rail passes over the object - up vector?
 * Fixes:
 * - None
 * Addition:
 * - Have RailCamera3D lag the target object slightly - configurable
 * - Test clone for ModelObject and MoveableModelObject
 */

/*
 * Version: 8
  =============
 * Author: NMCG
 * Revisions:
 * - Added MoveableModelObject
 * - Added RailParameters
 * - Added RailCamera3D (work in progress
 * - Added dictionary, utility, and debug bounding box drawer classes
 * Bugs:
 * - RailCamera3D shows bug when rail passes over the object - up vector?
 * Fixes:
 * - Fixed null on rail camera target object by moving InitialiseCamera()
 * Addition:
 * - Have RailCamera3D lag the target object slightly - configurable
 * - Test clone for ModelObject and MoveableModelObject
 */

/*
 * Version: 7
  =============
 * Author: NMCG
 * Revisions:
 * - Added IActor, Actor, DrawnActor
 * - Modified drawn objects and camera3D to work with new hierarchy
 * Bugs:
 * - Null on RailCamera3D::targetObject
 * Fixes:
 */

/*
 * Version: 6
  =============
 * Author: NMCG
 * Revisions:
 * - Added Camera3D::clone and test
 * - Added CameraManager and Camera3D code to Main::Draw()
 * - Solved reset in Transform3D and ProjectionParameters
 * - Replaced CameraManager key type from string to CameraLayout
 * - Added Main::demoCameraLayout() to switch on F1 and F2 between split and full screen layouts
 * - Added FirstPersonCamera3D::Clone() to allow us to copy this type and see movement in the cloned camera
 * - Added FirstPersonCamera3D mouse and keyboard movement
 * - Added FirstPersonCamera3D::speed and added value to GameData
 * - Added Camera3D::game static variable
 * - Added Main::InitializeGraphics to set resolution
 * Bugs:
 * Fixes:
 */

/*
 * Version: 5
  =============
 * Author: NMCG
 * Revisions:
 * - Added Transform3D and ProjectionParameters clone()
 * Bugs:
 * Fixes:
 */

/*
 * Version: 5
  =============
 * Author: NMCG
 * Revisions:
 * - Added Camera3D Equals and GetHashCode methods
 * - Added Camera3D viewport and properties
 * Bugs:
 * Fixes:
 */

/*
 * Version: 4
  =============
 * Author: NMCG
 * Revisions:
 * - Added TexturedQuad (unbuffered - change to vertex buffer?)
 * Bugs:
 * Fixes:
 */

/*
 * Version: 3.1 (3.0 was distributed during class Week 2 - 1/1/16)
  =============
 * Author: NMCG
 * Revisions:
 * - Added demo camera movement
 * - Added input managers - keybaord and mouse
 * - Added start code for Camera3D class but did not integrate in Main yet
 * - Added Transform3D
 * - Added ObjectType to indicate type of entity
 * - Added examples of buffered, and indexed and buffered primitive construction
 * Bugs:
 * Fixes:
 */

/*
 * Version: 2
 =============
 * Author: NMCG
 * Revisions:
 * - Added ProjectionParameters
 * - Added examples of unbuffered primitive construction
 * Bugs:
 * Fixes:
 */

namespace GDApp
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Main : Microsoft.Xna.Framework.Game
    {
        #region Fields
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private BasicEffect primitiveEffect;
        private BasicEffect texturedPrimitiveEffect;
        private BasicEffect texturedModelEffect;
        private Effect animatedModelEffect;
        private Effect billboardEffect;
        private SpriteFont debugFont;
        private Texture2D texture;
        private Vector2 screenCentre;
        private Microsoft.Xna.Framework.Rectangle screenRectangle;

        private CameraManager cameraManager;
        private KeyboardManager keyboardManager;
        private MouseManager mouseManager;
        private ObjectManager objectManager;
        private SoundManager soundManager;
        private PhysicsManager physicsManager;
        private UIManager uiManager;
        private MenuManager menuManager;
        private MenuManager menuManager2;


        private GenericDictionary<string, Texture2D> textureDictionary;
        private GenericDictionary<string, SpriteFont> fontDictionary;
        private GenericDictionary<string, Model> modelDictionary;
        private GenericDictionary<string, Camera3DTrack> trackDictionary;
        private EventDispatcher eventDispatcher;
        private Camera3DTrack cameraTrack;
        public PlayerObject playerActor;
        private PawnCollidableObject doorActor;
        public PawnCollidableObject rotator;
        private PawnCollidableObject step1;
        private PawnCollidableObject step2;
        private PawnCollidableObject step3;
        private PawnCollidableObject step4;
        private PawnCollidableObject step5;
        public PawnCollidableObject wall1;
        public PawnCollidableObject wall2;
        private PawnModelObject trap1;
        private PawnModelObject trap2;
        private PawnModelObject trap3;
        private PawnModelObject trap4;
        private PawnModelObject trap5;
        private PawnModelObject trap6;
        private PawnModelObject trap7;
        private PawnModelObject trap8;
        private PawnModelObject arrow1;
        private PawnModelObject arrow2;
        private PawnModelObject arrow3;
        private PawnModelObject arrow4;
        private PawnModelObject arrow5;
        private PawnModelObject arrow6;
        private PawnModelObject arrow7;
        private PawnModelObject arrow8;
        private bool mistake = false;

        private int nextStep;
        private bool bReset;
        private int resetCount;
        private bool screamed;
        private float interval;
        private float timePassed;
        private Random random;
        #endregion

        #region Properties
        public Microsoft.Xna.Framework.Rectangle ScreenRectangle
        {
            get
            {
                return screenRectangle;
            }
        }
        public int NextStep
        {
            get
            {
                return nextStep;
            }
        }
        public SpriteBatch SpriteBatch
        {
            get
            {
                return this.spriteBatch;
            }
        }
        public PhysicsManager PhysicsManager
        {
            get
            {
                return this.physicsManager;
            }
        }
        public ObjectManager ObjectManager
        {
            get
            {
                return this.objectManager;
            }
        }
        public EventDispatcher EventDispatcher
        {
            get
            {
                return this.eventDispatcher;
            }
        }
        public Vector2 ScreenCentre
        {
            get
            {
                return this.screenCentre;
            }
        }
        public SoundManager SoundManager
        {
            get
            {
                return soundManager;
            }
        }

        public CameraManager CameraManager
        {
            get
            {
                return this.cameraManager;
            }
        }
        public KeyboardManager KeyboardManager
        {
            get
            {
                return this.keyboardManager;
            }
        }
        public MouseManager MouseManager
        {
            get
            {
                return this.mouseManager;
            }
        }

        #endregion

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // testCloning();
            InitializeEventDispatcher();
            InitializeStatics();
            IntializeGraphics(1024, 768);
            InitializeDictionaries();
            LoadTextures();
            InitializeFonts();
            InitializeManagers(true);
            InitializeEffects();
            InitializeUI();

            //InitializeSkyBox(1000);
            LoadModels();
            InitializeModels();
            //InitializeTexturedPrimitives();

            InitializeCameraTracks();
            InitializeCamera();


            InitializeLevel();
            base.Initialize();

            this.soundManager.PlayCue("Background");
            this.soundManager.PlayCue("Tribal_Drums");

            this.screamed = false;
            this.random = new Random();
            this.interval = 25000 + (float)(random.NextDouble() * 10000 - 5000);
            this.timePassed = 0;
            EventDispatcher.Publish(new EventData("menu event", this, EventType.OnPause, EventCategoryType.MainMenu));
        }

        private void LoadTextures()
        {
            this.textureDictionary.Add("white", Content.Load<Texture2D>("Assets\\Textures\\UI\\white"));
            //menu
            this.textureDictionary.Add("mainmenu", Content.Load<Texture2D>("Assets/Textures/Menu/mainmenu"));
            this.textureDictionary.Add("audiomenu", Content.Load<Texture2D>("Assets/Textures/Menu/audiomenu"));
            this.textureDictionary.Add("controlsmenu", Content.Load<Texture2D>("Assets/Textures/Menu/exitmenu"));
            this.textureDictionary.Add("exitmenuwithtrans", Content.Load<Texture2D>("Assets/Textures/Menu/exitmenuwithtrans"));

            //IxChel - Menu
            this.textureDictionary.Add("IButton", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/AudioButton"));
            this.textureDictionary.Add("IButtonHighlighted", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/AudioButtonHighlighted"));
            this.textureDictionary.Add("IAudio", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/AudioMenu"));
            this.textureDictionary.Add("IAudioMenuMusic", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/AudioMenuMusic"));
            this.textureDictionary.Add("IAudioMenuMute", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/AudioMenuMute"));
            this.textureDictionary.Add("IAudioMenuReturn", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/AudioMenuReturn"));
            this.textureDictionary.Add("IAudioMenuSFX", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/AudioMenuSFX"));
            this.textureDictionary.Add("IControlsMenu", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/ControlsMenu"));
            this.textureDictionary.Add("IControlsMenuReturn", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/ControlsMenuReturn"));
            this.textureDictionary.Add("IMainMenu", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/MainMenu"));
            this.textureDictionary.Add("IMainMenuOptions", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/MainMenuOptions"));
            this.textureDictionary.Add("IMainMenuQuit", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/MainMenuQuit"));
            this.textureDictionary.Add("IMainMEnuStart", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/MainMEnuStart"));
            this.textureDictionary.Add("IOptionsMenu", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/OptionsMenu"));
            this.textureDictionary.Add("IOptionsMenuAudio", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/OptionsMenuAudio"));
            this.textureDictionary.Add("IOptionsMenuControls", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/OptionsMenuControls"));
            this.textureDictionary.Add("IOptionsMenuReturn", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/OptionsMenuReturn"));
            this.textureDictionary.Add("IPauseMenu", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/PauseMenu"));
            this.textureDictionary.Add("IPauseMenuOptions", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/PauseMenuOptions"));
            this.textureDictionary.Add("IPauseMenuQuit", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/PauseMenuQuit"));
            this.textureDictionary.Add("IPauseMenuRestart", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/PauseMenuRestart"));
            this.textureDictionary.Add("IPauseMenuResume", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/PauseMenuResume"));
            this.textureDictionary.Add("IYouLose", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/YouLose"));
            this.textureDictionary.Add("IYouLoseQuit", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/YouLoseQuit"));
            this.textureDictionary.Add("IYouLoseRestart", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/YouLoseRestart"));
            this.textureDictionary.Add("IYouWin", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/YouWin"));
            this.textureDictionary.Add("IYouWinQuit", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/YouWinQuit"));
            this.textureDictionary.Add("IYouWinRestart", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/YouWinRestart"));
        }

        private void InitializeUI()
        {
            InitializeUIMenu();
        }

        private void InitializeUIMenu()
        {
            Transform2D transform = null;
            SpriteFont font = null;
            Texture2D texture = null;
            /*
            //text
            font = this.fontDictionary["ui"];
            String text = "help me!";
            Vector2 dimensions = font.MeasureString(text);
            transform = new Transform2D(new Vector2(50, 600), 0, Vector2.One, Vector2.Zero, new Integer2(dimensions));
            UITextObject textObject = new UITextObject("test1", ObjectType.UIText, transform, new Color(15, 15, 15, 150), SpriteEffects.None, 0, "help", font, true);
            this.uiManager.Add(textObject);

            //texture
            texture = this.textureDictionary["white"];
            transform = new Transform2D(new Vector2(40, 590), 0, new Vector2(4, 4), Vector2.Zero, new Integer2(texture.Width, texture.Height));
            UITextureObject texture2DObject = new UITextureObject("texture1",
                ObjectType.UITexture2D, transform, new Color(127, 127, 127, 50),
                SpriteEffects.None, 1, texture, true);
            this.uiManager.Add(texture2DObject);
            */
        }

        private void InitializeEventDispatcher()
        {
            this.eventDispatcher = new EventDispatcher(this, 25);
            Components.Add(this.eventDispatcher);
        }

        private void LoadModels()
        {
            Model model = null;

            model = Content.Load<Model>("Assets\\Models\\Animated");
            this.modelDictionary.Add("player", model);

            model = Content.Load<Model>("Assets\\Models\\DoorV1");
            this.modelDictionary.Add("door", model);

            model = Content.Load<Model>("Assets\\Models\\RoomV6");
            this.modelDictionary.Add("room", model);

            model = Content.Load<Model>("Assets\\Models\\totemV1");
            this.modelDictionary.Add("rotation", model);

            model = Content.Load<Model>("Assets\\Models\\wallv1");
            this.modelDictionary.Add("wall", model);

            model = Content.Load<Model>("Assets\\Models\\plate");
            this.modelDictionary.Add("plate", model);

            model = Content.Load<Model>("Assets\\Models\\trap");
            this.modelDictionary.Add("trap", model);

            model = Content.Load<Model>("Assets\\Models\\arrow1");
            this.modelDictionary.Add("arrow", model);
        }

        private void InitializeLevel()
        {
            //((CharacterMoveController)this.playerActor.ControllerList[0]).Camera = this.cameraManager[0];
            //((CharacterRotatorInteractionController)this.playerActor.ControllerList[1]).TargetActor = this.rotator;
            nextStep = 1;
            mistake = false;
        }

        private void InitializeCameraTracks()
        {
            this.cameraTrack = null;

            cameraTrack = new Camera3DTrack(CurveLoopType.Oscillate);
            cameraTrack.Add(new Vector3(-20, 10, 10), -Vector3.UnitZ, Vector3.UnitY, 0);
            cameraTrack.Add(new Vector3(20, 5, 10), -Vector3.UnitZ, Vector3.UnitY, 5);
            cameraTrack.Add(new Vector3(50, 5, 10), -Vector3.UnitX, Vector3.UnitY, 10);

            this.trackDictionary.Add("simple", cameraTrack);


            cameraTrack = new Camera3DTrack(CurveLoopType.Oscillate);
            //start
            cameraTrack.Add(new Vector3(0, 2, 0), -Vector3.UnitY, Vector3.UnitZ, 0);
            //fast
            cameraTrack.Add(new Vector3(0, 100, 0), Vector3.UnitZ, Vector3.UnitY, 5);
            //slow
            cameraTrack.Add(new Vector3(0, 105, 0), Vector3.UnitZ, Vector3.UnitY, 7);
            //fall
            cameraTrack.Add(new Vector3(0, 2, 0), -Vector3.UnitY, Vector3.UnitZ, 8);

            this.trackDictionary.Add("puke", cameraTrack);
        }

        private void InitializeDictionaries()
        {
            this.textureDictionary = new GenericDictionary<string, Texture2D>("textures");
            this.fontDictionary = new GenericDictionary<string, SpriteFont>("fonts");
            this.modelDictionary = new GenericDictionary<string, Model>("model");
            this.trackDictionary = new GenericDictionary<string, Camera3DTrack>("camera tracks");
        }

        private void InitializeModels()
        {
            Texture2D texture = null;
            //MoveableModelObject playerActor = null;
            Transform3D transform = null;
            Model model = null;
            ModelObject modelObject = null;
            CollidableObject collidableObj = null;
            PawnCollidableObject collObj = null;
            ZoneObject zoneObj = null;
            CameraZoneObject camObj = null;


            #region Player Model
            model = this.modelDictionary["player"];
            transform = new Transform3D(new Vector3(-100, 10, 0),
                new Vector3(-90, 0, 0), 0.15f * Vector3.One,
                Vector3.UnitX, Vector3.UnitY);
            this.playerActor = new PlayerObject("player",
                ObjectType.Player, transform, this.animatedModelEffect, null, model, Color.White, 1f, KeyData.Player_Keys, 3.75f, 11.5f, 1f, 1f, new Vector3(0, 0, 0));
            this.playerActor.Enable(false, 100);
            this.objectManager.Add(this.playerActor);
            #endregion

            #region ExitDoor Model
            model = this.modelDictionary["door"];
            transform = new Transform3D(new Vector3(140, 0, 0), Vector3.Zero, 1f * Vector3.One, -Vector3.UnitZ, Vector3.UnitY);
            this.doorActor = new PawnCollidableObject("door", ObjectType.Door, transform, this.texturedModelEffect, null, model, Color.White, 1f);
            Vector3 scales = new Vector3(12, 250, 25);
            this.doorActor.AddPrimitive(new Box(transform.Translation, Matrix.Identity, scales), new MaterialProperties());
            this.doorActor.Enable(true, 2000);

            this.objectManager.Add(this.doorActor);
            #endregion
            #region EntranceDoor Model
            model = this.modelDictionary["door"];
            transform = new Transform3D(new Vector3(-300, 0, 0), Vector3.Zero, 1f * Vector3.One, -Vector3.UnitZ, Vector3.UnitY);
            collObj = new PawnCollidableObject("door", ObjectType.Entrance, transform, this.texturedModelEffect, null, model, Color.White, 1f);
            collObj.AddPrimitive(new Box(transform.Translation, Matrix.Identity, scales), new MaterialProperties());
            collObj.Enable(true, 2000);

            this.objectManager.Add(collObj);
            #endregion
            texture = Content.Load<Texture2D>("Assets\\Textures\\Game\\white");
            #region Room Model
            model = this.modelDictionary["room"];
            transform = new Transform3D(new Vector3(0, 0, 0), new Vector3(0,180,0), 1f * Vector3.One, -Vector3.UnitZ, Vector3.UnitY);
            MaterialProperties material = new MaterialProperties(1f, 0.1f, 0.05f);
            collidableObj = new CollidableObject("room", ObjectType.Room, transform, this.texturedModelEffect, null, model, Color.White, 1);
            //floor
            scales = new Vector3(300, 1, 300);
            collidableObj.AddPrimitive(new Box(transform.Translation, Matrix.Identity, scales), material);
            collidableObj.Enable(true, 2000);

            this.objectManager.Add(collidableObj);

            scales = new Vector3(10, 100, 300);
            transform = new Transform3D(new Vector3(152, 0, 0), Vector3.Zero, scales, -Vector3.UnitZ, Vector3.UnitY);
            zoneObj = new ZoneObject("roomwallback", ObjectType.Room, transform, this.primitiveEffect, Color.Red, 1, true);
            zoneObj.AddPrimitive(new Box(transform.Translation, Matrix.Identity, transform.Scale));
            zoneObj.Enable();

            this.objectManager.Add(zoneObj);
            
            transform = new Transform3D(new Vector3(-152, 0, 0), Vector3.Zero, scales, -Vector3.UnitZ, Vector3.UnitY);
            zoneObj = new ZoneObject("roomwallfront", ObjectType.Room, transform, this.primitiveEffect, Color.Red, 1, true);
            zoneObj.AddPrimitive(new Box(transform.Translation, Matrix.Identity, transform.Scale));
            zoneObj.Enable();

            this.objectManager.Add(zoneObj);

            scales = new Vector3(300, 100, 5);
            transform = new Transform3D(new Vector3(0, 0, 130), Vector3.Zero, scales, -Vector3.UnitZ, Vector3.UnitY);
            zoneObj = new ZoneObject("roomwallright", ObjectType.Room, transform, this.primitiveEffect, Color.Red, 1, true);
            zoneObj.AddPrimitive(new Box(transform.Translation, Matrix.Identity, transform.Scale));
            zoneObj.Enable();

            this.objectManager.Add(zoneObj);

            transform = new Transform3D(new Vector3(0, 0, -130), Vector3.Zero, scales, -Vector3.UnitZ, Vector3.UnitY);
            zoneObj = new ZoneObject("roomwallleft", ObjectType.Room, transform, this.primitiveEffect, Color.Red, 1, true);
            zoneObj.AddPrimitive(new Box(transform.Translation, Matrix.Identity, transform.Scale));
            zoneObj.Enable();

            this.objectManager.Add(zoneObj);
            #endregion

            #region Rotationthingy Model
            model = this.modelDictionary["rotation"];
            transform = new Transform3D(new Vector3(0, -57, 0), Vector3.Zero, 0.6f * Vector3.One, -Vector3.UnitZ, Vector3.UnitY);
            this.rotator = new PawnCollidableObject("RotationThingy", ObjectType.Rotation, transform, this.texturedModelEffect, null, model, Color.White, 1);
            Matrix rot;
            this.rotator.AddPrimitive(new Capsule(transform.Translation - new Vector3(0, 0, 20 * 0.6f), Matrix.Identity, 2.5f * 0.6f, 40 * 0.6f), new MaterialProperties());
            Matrix.CreateRotationY(MathHelper.ToRadians(90), out rot);
            this.rotator.AddPrimitive(new Capsule(transform.Translation - new Vector3(20 *0.6f, 0, 0), rot, 2.5f * 0.6f, 40 * 0.6f), new MaterialProperties());
            this.rotator.Enable(true, 200);

            this.objectManager.Add(this.rotator);
            #endregion

            //Walls are initialized after Rotator because of their dependancy on Rotator (see RotatorController)
            //Maybe we should rather go for a more active approach meaning Rotator gets everything which is gonna rotate around it and rotates it
            #region Wall right Model
            model = this.modelDictionary["wall"];
            transform = new Transform3D(new Vector3(-4f, -1.8f, -69.8f), new Vector3(0, 90, 0), 1f * Vector3.One, -Vector3.UnitZ, Vector3.UnitY);
            wall1 = new PawnCollidableObject("Wall1", ObjectType.Wall, transform, this.texturedModelEffect, null, model, Color.Gray, 1);
            wall1.Add(new RotatorController("wall1Rotator", wall1, true, this.rotator));
            scales = new Vector3(300, 100, 12);
            wall1.AddPrimitive(new Box(transform.Translation, Matrix.Identity, scales), material);
            wall1.Enable(true, 2000);

            this.objectManager.Add(wall1);
            #endregion
            #region Wall left Model
            model = this.modelDictionary["wall"];
            transform = new Transform3D(new Vector3(-4f, -1.8f, 69.8f), new Vector3(0,90,0), 1f * Vector3.One, -Vector3.UnitZ, Vector3.UnitY);
            wall2 = new PawnCollidableObject("Wall2", ObjectType.Wall, transform, this.texturedModelEffect, null, model, Color.White, 1);
            wall2.Add(new RotatorController("wall2Rotator", wall2, true, this.rotator));
            wall2.AddPrimitive(new Box(transform.Translation, Matrix.Identity, scales), material);
            wall2.Enable(true, 2000);

            this.objectManager.Add(wall2);
            #endregion
            texture = Content.Load<Texture2D>("Assets/skin/Brick_Tut_35");
            #region Pressure Plate Exit Model
            model = this.modelDictionary["plate"];
            scales = new Vector3(12.5f, 2.5f, 12.5f);
            transform = new Transform3D(new Vector3(125, 0, 0), new Vector3(0, 90, 0), Vector3.One * 0.5f, -Vector3.UnitZ, Vector3.UnitY);
            step1 = new PawnCollidableObject("PressurePlate1", ObjectType.Plate, transform, this.texturedModelEffect, texture, model, Color.White, 1);
            step1.AddPrimitive(new Box(transform.Translation, Matrix.Identity, scales), new MaterialProperties());
            step1.Enable(true, 2000);

            this.objectManager.Add(step1);
            #endregion
            #region Pressure Plate Right Up Model
            transform = new Transform3D(new Vector3(26, 0, 110), Vector3.Zero, Vector3.One * 0.5f, -Vector3.UnitZ, Vector3.UnitY);
            step4 = new PawnCollidableObject("PressurePlate4", ObjectType.Plate, transform, this.texturedModelEffect, texture, model, Color.White, 1);
            step4.AddPrimitive(new Box(transform.Translation, Matrix.Identity, scales), new MaterialProperties());
            step4.Enable(true, 2000);

            this.objectManager.Add(step4);
            #endregion
            #region Pressure Plate Right Down Model
            transform = new Transform3D(new Vector3(-36, 0, 110), Vector3.Zero, Vector3.One * 0.5f, -Vector3.UnitZ, Vector3.UnitY);
            step5 = new PawnCollidableObject("PressurePlate5", ObjectType.Plate, transform, this.texturedModelEffect, texture, model, Color.White, 1);
            step5.AddPrimitive(new Box(transform.Translation, Matrix.Identity, scales), new MaterialProperties());
            step5.Enable(true, 2000);
            
            this.objectManager.Add(step5);
            #endregion
            #region Pressure Plate Left Up Model
            transform = new Transform3D(new Vector3(26, 0, -110), Vector3.Zero, Vector3.One*0.5f, -Vector3.UnitZ, Vector3.UnitY);
            step2 = new PawnCollidableObject("PressurePlate2", ObjectType.Plate, transform, this.texturedModelEffect, null, model, Color.White, 1);
            step2.AddPrimitive(new Box(transform.Translation, Matrix.Identity, scales), new MaterialProperties());
            step2.Enable(true, 2000);
            
            this.objectManager.Add(step2);
            #endregion
            #region Pressure Plate Left Down Model
            transform = new Transform3D(new Vector3(-36, 0, -110), Vector3.Zero, Vector3.One * 0.5f, -Vector3.UnitZ, Vector3.UnitY);
            step3 = new PawnCollidableObject("PressurePlate3", ObjectType.Plate, transform, this.texturedModelEffect, null, model, Color.White, 1);
            step3.AddPrimitive(new Box(transform.Translation, Matrix.Identity, scales), new MaterialProperties());
            step3.Enable(true, 2000);

            this.objectManager.Add(step3);
            #endregion

            #region Traps
            model = this.modelDictionary["trap"];
            transform = new Transform3D(new Vector3(80, 20f, 105), Vector3.UnitZ * 90, 0.5f * Vector3.One, -Vector3.UnitZ, Vector3.UnitY);
            trap1 = new PawnModelObject("PressurePlate1", ObjectType.Plate, transform, this.texturedModelEffect,texture, model, Color.White, 1);
            this.objectManager.Add(trap1);

            transform = new Transform3D(new Vector3(80, 20f, 115), Vector3.UnitZ * 90, 0.5f * Vector3.One, -Vector3.UnitZ, Vector3.UnitY);
            trap2 = new PawnModelObject("PressurePlate1", ObjectType.Plate, transform, this.texturedModelEffect, texture, model, Color.White, 1);
            this.objectManager.Add(trap2);

            transform = new Transform3D(new Vector3(80, 20f, -105), Vector3.UnitZ * 90, 0.5f * Vector3.One, -Vector3.UnitZ, Vector3.UnitY);
            trap3 = new PawnModelObject("PressurePlate1", ObjectType.Plate, transform, this.texturedModelEffect, texture, model, Color.White, 1);
            this.objectManager.Add(trap3);

            transform = new Transform3D(new Vector3(80, 20f, -115), Vector3.UnitZ * 90, 0.5f * Vector3.One, -Vector3.UnitZ, Vector3.UnitY);
            trap4 = new PawnModelObject("PressurePlate1", ObjectType.Plate, transform, this.texturedModelEffect, texture, model, Color.White, 1);
            this.objectManager.Add(trap4);

            transform = new Transform3D(new Vector3(-90, 20f, 105), -Vector3.UnitZ * 90, 0.5f * Vector3.One, -Vector3.UnitZ, Vector3.UnitY);
            trap5 = new PawnModelObject("PressurePlate1", ObjectType.Plate, transform, this.texturedModelEffect, texture, model, Color.White, 1);
            this.objectManager.Add(trap5);

            transform = new Transform3D(new Vector3(-90, 20f, 115), -Vector3.UnitZ * 90, 0.5f * Vector3.One, -Vector3.UnitZ, Vector3.UnitY);
            trap6 = new PawnModelObject("PressurePlate1", ObjectType.Plate, transform, this.texturedModelEffect, texture, model, Color.White, 1);
            this.objectManager.Add(trap6);

            transform = new Transform3D(new Vector3(-90, 20f, -105), -Vector3.UnitZ * 90, 0.5f * Vector3.One, -Vector3.UnitZ, Vector3.UnitY);
            trap7 = new PawnModelObject("PressurePlate1", ObjectType.Plate, transform, this.texturedModelEffect, texture, model, Color.White, 1);
            this.objectManager.Add(trap7);

            transform = new Transform3D(new Vector3(-90, 20f, -115), -Vector3.UnitZ * 90, 0.5f * Vector3.One, -Vector3.UnitZ, Vector3.UnitY);
            trap8 = new PawnModelObject("PressurePlate1", ObjectType.Plate, transform, this.texturedModelEffect, texture, model, Color.White, 1);
            this.objectManager.Add(trap8);
            #endregion
            #region Arrow
            model = this.modelDictionary["arrow"];
            transform = new Transform3D(new Vector3(80, 20f, 105), Vector3.UnitZ * 90,  Vector3.One, -Vector3.UnitZ, Vector3.UnitY);
            arrow1 = new PawnModelObject("Arrow1", ObjectType.Plate, transform, this.texturedModelEffect, texture, model, Color.White, 1);
            this.objectManager.Add(arrow1);

            transform = new Transform3D(new Vector3(80, 20f, 115), Vector3.UnitZ * 90,  Vector3.One, -Vector3.UnitZ, Vector3.UnitY);
            arrow2 = new PawnModelObject("Arrow2", ObjectType.Plate, transform, this.texturedModelEffect, texture, model, Color.White, 1);
            this.objectManager.Add(arrow2);

            transform = new Transform3D(new Vector3(80, 20f, -105), Vector3.UnitZ * 90,  Vector3.One, -Vector3.UnitZ, Vector3.UnitY);
            arrow3 = new PawnModelObject("Arrow3", ObjectType.Plate, transform, this.texturedModelEffect, texture, model, Color.White, 1);
            this.objectManager.Add(arrow3);

            transform = new Transform3D(new Vector3(80, 20f, -115), Vector3.UnitZ * 90,  Vector3.One, -Vector3.UnitZ, Vector3.UnitY);
            arrow4 = new PawnModelObject("Arrow4", ObjectType.Plate, transform, this.texturedModelEffect, texture, model, Color.White, 1);
            this.objectManager.Add(arrow4);

            transform = new Transform3D(new Vector3(-90, 20f, 105), -Vector3.UnitZ * 90,  Vector3.One, -Vector3.UnitZ, Vector3.UnitY);
            arrow5 = new PawnModelObject("Arrow5", ObjectType.Plate, transform, this.texturedModelEffect, texture, model, Color.White, 1);
            this.objectManager.Add(arrow5);

            transform = new Transform3D(new Vector3(-90, 20f, 115), -Vector3.UnitZ * 90,  Vector3.One, -Vector3.UnitZ, Vector3.UnitY);
            arrow6 = new PawnModelObject("Arrow6", ObjectType.Plate, transform, this.texturedModelEffect, texture, model, Color.White, 1);
            this.objectManager.Add(arrow6);

            transform = new Transform3D(new Vector3(-90, 20f, -105), -Vector3.UnitZ * 90,  Vector3.One, -Vector3.UnitZ, Vector3.UnitY);
            arrow7 = new PawnModelObject("Arrow7", ObjectType.Plate, transform, this.texturedModelEffect, texture, model, Color.White, 1);
            this.objectManager.Add(arrow7);

            transform = new Transform3D(new Vector3(-90, 20f, -115), -Vector3.UnitZ * 90, Vector3.One, -Vector3.UnitZ, Vector3.UnitY);
            arrow8 = new PawnModelObject("Arrow8", ObjectType.Plate, transform, this.texturedModelEffect, texture, model, Color.White, 1);
            this.objectManager.Add(arrow8);

            scales = new Vector3(300, 100, 20);
            #endregion

            rotator.Add(new OffsetController("offset controller 2", rotator, true, new Vector3(0, 50, 0)));
            rotator.Add(new RotorController("rotor Controller", this.rotator, true));
            doorActor.Add(new OffsetController("offset vontroller 7", doorActor, true, new Vector3(0, -110, 0)));
            step1.Add(new OffsetController("offset controller 1", step1, true, new Vector3(0, -3, 0)));
            step2.Add(new OffsetController("offset controller 3", step2, true, new Vector3(0, -3, 0)));
            step3.Add(new OffsetController("offset controller 4", step3, true, new Vector3(0, -3, 0)));
            step4.Add(new OffsetController("offset controller 5", step4, true, new Vector3(0, -3, 0)));
            step5.Add(new OffsetController("offset controller 6", step5, true, new Vector3(0, -3, 0)));
            trap1.Add(new OffsetController("offset controller 7", trap1, true, new Vector3(-20, 0, 0)));
            trap2.Add(new OffsetController("offset controller 8", trap2, true, new Vector3(-20, 0, 0)));
            trap3.Add(new OffsetController("offset controller 9", trap3, true, new Vector3(-20, 0, 0)));
            trap4.Add(new OffsetController("offset controller 10", trap4, true, new Vector3(-20, 0, 0)));
            trap5.Add(new OffsetController("offset controller 11", trap5, true, new Vector3(20, 0, 0)));
            trap6.Add(new OffsetController("offset controller 12", trap6, true, new Vector3(20, 0, 0)));
            trap7.Add(new OffsetController("offset controller 13", trap7, true, new Vector3(20, 0, 0)));
            trap8.Add(new OffsetController("offset controller 14", trap8, true, new Vector3(20, 0, 0)));
            arrow1.Add(new OffsetController("offset controller 7", arrow1, true, new Vector3(-200, 0, 0), 0.2f));
            arrow2.Add(new OffsetController("offset controller 8", arrow2, true, new Vector3(-200, 0, 0), 0.2f));
            arrow3.Add(new OffsetController("offset controller 9", arrow3, true, new Vector3(-200, 0, 0), 0.2f));
            arrow4.Add(new OffsetController("offset controller 10", arrow4, true, new Vector3(-200, 0, 0), 0.2f));
            arrow5.Add(new OffsetController("offset controller 11", arrow5, true, new Vector3(200, 0, 0), 0.2f));
            arrow6.Add(new OffsetController("offset controller 12", arrow6, true, new Vector3(200, 0, 0), 0.2f));
            arrow7.Add(new OffsetController("offset controller 13", arrow7, true, new Vector3(200, 0, 0), 0.2f));
            arrow8.Add(new OffsetController("offset controller 14", arrow8, true, new Vector3(200, 0, 0), 0.2f));

        }

        

        private void IntializeGraphics(int width, int height)
        {
            this.graphics.PreferredBackBufferWidth = width;
            this.graphics.PreferredBackBufferHeight = height;
            this.screenCentre = new Vector2(width/2, height/2);
            this.screenRectangle = new Microsoft.Xna.Framework.Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            this.graphics.ApplyChanges();
        }

        private void InitializeStatics()
        {
            Actor.game = this;
            UIActor.game = this;
        }

        private void InitializeManagers(bool isMouseVisible)
        {
            this.physicsManager = new PhysicsManager(this, true);
            Components.Add(physicsManager);

            this.cameraManager = new CameraManager(this);
            Components.Add(this.cameraManager);

            this.keyboardManager = new KeyboardManager(this);
            Components.Add(this.keyboardManager);

            this.mouseManager = new MouseManager(this, isMouseVisible);
            //centre the mouse otherwise the movement for 1st person camera will be unpredictable
            this.mouseManager.SetPosition(this.screenCentre); 
            Components.Add(this.mouseManager);

            this.objectManager = new ObjectManager(this, 10, 10, false);
            this.objectManager.DrawOrder = 1;
            Components.Add(this.objectManager);

            this.soundManager = new SoundManager(this,
                 "Content\\Assets\\Audio\\IxchelsQuestAudio.xgs",
                 "Content\\Assets\\Audio\\Wave Bank.xwb",
                "Content\\Assets\\Audio\\Sound Bank.xsb");
            Components.Add(this.soundManager);

            this.uiManager = new UIManager(this, 10, 10);
            this.uiManager.DrawOrder = 2; //always draw after object manager(1)
            Components.Add(this.uiManager);


            this.textureDictionary.Add("IButton", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/AudioButton"));
            this.textureDictionary.Add("IButtonHighlighted", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/AudioButtonHighlighted"));
            this.textureDictionary.Add("IAudio", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/AudioMenu"));
            this.textureDictionary.Add("IAudioMenuMusic", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/AudioMenuMusic"));
            this.textureDictionary.Add("IAudioMenuMute", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/AudioMenuMute"));
            this.textureDictionary.Add("IAudioMenuReturn", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/AudioMenuReturn"));
            this.textureDictionary.Add("IAudioMenuSFX", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/AudioMenuSFX"));
            this.textureDictionary.Add("IControlsMenu", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/ControlsMenu"));
            this.textureDictionary.Add("IControlsMenuReturn", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/ControlsMenuReturn"));
            this.textureDictionary.Add("IMainMenu", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/MainMenu"));
            this.textureDictionary.Add("IMainMenuOptions", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/MainMenuOptions"));
            this.textureDictionary.Add("IMainMenuQuit", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/MainMenuQuit"));
            this.textureDictionary.Add("IMainMEnuStart", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/MainMEnuStart"));
            this.textureDictionary.Add("IOptionsMenu", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/OptionsMenu"));
            this.textureDictionary.Add("IOptionsMenuAudio", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/OptionsMenuAudio"));
            this.textureDictionary.Add("IOptionsMenuControls", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/OptionsMenuControls"));
            this.textureDictionary.Add("IOptionsMenuReturn", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/OptionsMenuReturn"));
            this.textureDictionary.Add("IPauseMenu", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/PauseMenu"));
            this.textureDictionary.Add("IPauseMenuOptions", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/PauseMenuOptions"));
            this.textureDictionary.Add("IPauseMenuQuit", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/PauseMenuQuit"));
            this.textureDictionary.Add("IPauseMenuRestart", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/PauseMenuRestart"));
            this.textureDictionary.Add("IPauseMenuResume", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/PauseMenuResume"));
            this.textureDictionary.Add("IYouLose", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/YouLose"));
            this.textureDictionary.Add("IYouLoseQuit", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/YouLoseQuit"));
            this.textureDictionary.Add("IYouLoseRestart", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/YouLoseRestart"));
            this.textureDictionary.Add("IYouWin", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/YouWin"));
            this.textureDictionary.Add("IYouWinQuit", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/YouWinQuit"));
            this.textureDictionary.Add("IYouWinRestart", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/YouWinRestart"));
            this.textureDictionary.Add("IX", Content.Load<Texture2D>("Assets/Textures/Menu/IxChel/X"));

            Texture2D[] menuTexturesArray = {
                                                this.textureDictionary["IButton"],
                                                this.textureDictionary["IButtonHighlighted"],
                                                this.textureDictionary["IAudio"],
                                                this.textureDictionary["IAudioMenuMusic"],
                                                this.textureDictionary["IAudioMenuMute"],
                                                this.textureDictionary["IAudioMenuReturn"],
                                                this.textureDictionary["IAudioMenuSFX"],
                                                this.textureDictionary["IControlsMenu"],
                                                this.textureDictionary["IControlsMenuReturn"],
                                                this.textureDictionary["IMainMenu"],
                                                this.textureDictionary["IMainMenuOptions"],
                                                this.textureDictionary["IMainMenuQuit"],
                                                this.textureDictionary["IMainMEnuStart"],
                                                this.textureDictionary["IOptionsMenu"],
                                                this.textureDictionary["IOptionsMenuAudio"],
                                                this.textureDictionary["IOptionsMenuControls"],
                                                this.textureDictionary["IOptionsMenuReturn"],
                                                this.textureDictionary["IPauseMenu"],
                                                this.textureDictionary["IPauseMenuOptions"],
                                                this.textureDictionary["IPauseMenuQuit"],
                                                this.textureDictionary["IPauseMenuRestart"],
                                                this.textureDictionary["IPauseMenuResume"],
                                                this.textureDictionary["IYouLose"],
                                                this.textureDictionary["IYouLoseQuit"],
                                                this.textureDictionary["IYouLoseRestart"],
                                                this.textureDictionary["IYouWin"],
                                                this.textureDictionary["IYouWinQuit"],
                                                this.textureDictionary["IYouWinRestart"],
                                                this.textureDictionary["IX"]
                                            };

            this.menuManager = new MenuManager(this, menuTexturesArray, this.fontDictionary["menu"], MenuData.MenuTexturePadding, MenuData.MenuTextureColor);
            this.menuManager.DrawOrder = 3; //always draw after ui manager(2)
            Components.Add(this.menuManager);
        }

        #region DEBUG
        private void testCloning()
        {
            Transform3D t1
                = new Transform3D(Vector3.One,
                    Vector3.Zero,
                    Vector3.One, Vector3.UnitZ,
                    Vector3.UnitY);

            Transform3D clone
                    = (Transform3D)t1.Clone();

            clone.Translation = new Vector3(1, 2, 3);

            if (clone.Translation.Equals(t1.Translation))
                Console.WriteLine("same!");
            else
                Console.WriteLine("different!");

            Camera3D c1 = new Camera3D("1",
                ObjectType.FirstPersonCamera,
                t1,
                ProjectionParameters.StandardMediumFourThree,
                new Viewport(0,0,800,600));

            Camera3D cloneC1 = (Camera3D)c1.Clone();
            cloneC1.Viewport = new Viewport(0, 0, 10, 10);
        }
        #endregion

        private void InitializeFonts()
        {
            this.debugFont = Content.Load<SpriteFont>("Assets\\Debug\\Fonts\\debug");
            this.fontDictionary.Add("debug", Content.Load<SpriteFont>("Assets\\Debug\\Fonts\\debug"));
            this.fontDictionary.Add("ui", Content.Load<SpriteFont>("Assets\\Fonts\\ui"));
            this.fontDictionary.Add("menu", Content.Load<SpriteFont>("Assets\\Fonts\\menu"));
        }
        

        private void InitializeEffects()
        {
            this.primitiveEffect = new BasicEffect(graphics.GraphicsDevice);
            this.primitiveEffect.VertexColorEnabled = true;

            this.texturedPrimitiveEffect = new BasicEffect(graphics.GraphicsDevice);
            this.texturedPrimitiveEffect.VertexColorEnabled = true;
            this.texturedPrimitiveEffect.TextureEnabled = true;

            this.texturedModelEffect = new BasicEffect(graphics.GraphicsDevice);
            this.texturedModelEffect.VertexColorEnabled = true; //bug - 22/4/16
            this.texturedModelEffect.TextureEnabled = false;

            //used for billboards
            this.billboardEffect = Content.Load<Effect>("Assets/Effects/Billboard");

            this.animatedModelEffect = Content.Load<Effect>("Assets/Effects/Animated");
        }
    

        private void InitializeCamera()
        {
            Camera3D clone = null;
            Camera3D camera = null;
            PawnCamera3D pawnCamera = null;

            camera = new FreeLookCamera3D("full", ObjectType.FirstPersonCamera,
                   new Transform3D(new Vector3(-10, 5, 30), -Vector3.UnitZ, Vector3.UnitY),
                       ProjectionParameters.StandardMediumFourThree, this.graphics.GraphicsDevice.Viewport, GameData.CameraSpeed);

            #region Fullscreen
            this.cameraManager.Add("FirstPersonFullScreen", camera);
            #endregion

            #region RailCharacterFollow
            pawnCamera = new PawnCamera3D("RailCamera1",
                ObjectType.RailCamera, (Transform3D)Transform3D.Zero.Clone(),
                ProjectionParameters.StandardMediumFourThree, this.graphics.GraphicsDevice.Viewport);

            pawnCamera.Add(new RailCharacterFollowController("rail character follow controller 1",
                pawnCamera, true, new RailParameters("r1", new Vector3(-148, 34, 0),
                new Vector3(145, 34, 0)), playerActor, 50));

            this.cameraManager.Add("FullScreen", pawnCamera);
            
            pawnCamera = new PawnCamera3D("RailCamera2",
                ObjectType.RailCamera, (Transform3D)Transform3D.Zero.Clone(),
                ProjectionParameters.StandardMediumFourThree, this.graphics.GraphicsDevice.Viewport);

            pawnCamera.Add(new RailCharacterFollowController("rail character follow controller 2",
                pawnCamera, true, new RailParameters("r2", new Vector3(0, 34, 60),
                new Vector3(0, 34, -120)), playerActor, 50));

            this.cameraManager.Add("FullScreen", pawnCamera);

            pawnCamera = new PawnCamera3D("RailCamera3",
                ObjectType.RailCamera, (Transform3D)Transform3D.Zero.Clone(),
                ProjectionParameters.StandardMediumFourThree, this.graphics.GraphicsDevice.Viewport);

            pawnCamera.Add(new RailCharacterFollowController("rail character follow controller 3",
                pawnCamera, true, new RailParameters("r3", new Vector3(0, 34, -60),
                new Vector3(0, 34, 120)), playerActor, 50));

            this.cameraManager.Add("FullScreen", pawnCamera);

            #endregion

            #region RotateCamera

            pawnCamera = new PawnCamera3D("RotateCamera",
                ObjectType.ZoomOnDoorCamera, new Transform3D(new Vector3(50,100,50), new Vector3(300, -300, 0), Vector3.UnitY),
                ProjectionParameters.StandardMediumFourThree, this.graphics.GraphicsDevice.Viewport);

            pawnCamera.Add(new RotatorController("rotate camera controller 1",
                pawnCamera, true, this.rotator));
            this.cameraManager.Add("FullScreen", pawnCamera);

            #endregion

            #region ZoomOnDoor

            pawnCamera = new PawnCamera3D("ZoomOnDoor",
                ObjectType.ZoomOnDoorCamera, new Transform3D(new Vector3(100, 126, 0), new Vector3(100,-51,0), Vector3.UnitY),
                ProjectionParameters.StandardMediumFourThree, this.graphics.GraphicsDevice.Viewport);
            pawnCamera.Add(new ShakeController("shaker1", pawnCamera, true));
            
            this.cameraManager.Add("FullScreen", pawnCamera);

            #endregion

            //set the default layout
            this.cameraManager.SetCameraLayout("FullScreen");
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            //demoCameraLayout();
            //demoCameraTrack(gameTime);
            //demoRotation();
            demoWinLose();

            if(bReset)
            {
                if(resetCount > 500)
                {
                    bReset = false;
                    resetCount = 0;
                    menuManager.OpenMenu("Lose");
                }
                else
                {
                    if(resetCount > 200 && !screamed)
                    {
                        this.soundManager.PlayCue("Death_Grunt");
                        screamed = true;
                    }
                    resetCount += gameTime.ElapsedGameTime.Milliseconds;
                }
            }
            this.timePassed += gameTime.ElapsedGameTime.Milliseconds;
            if(this.timePassed > interval)
            {
                this.timePassed = 0;
                this.interval = 25000 + (float)(random.NextDouble() * 10000 - 5000);
                this.soundManager.PlayCue("Rocks_Falling");
            }

            if(keyboardManager.IsFirstKeyPress(Keys.Escape))
            {
                EventDispatcher.Publish(new EventData("menu pause", this, EventType.OnPause, EventCategoryType.MainMenu));
                menuManager.OpenMenu("Pause");
            }

            base.Update(gameTime);
        }

        private void demoWinLose()
        {
            if (this.keyboardManager.IsFirstKeyPress(Keys.NumPad1))
                checkNext(1);
            if (this.keyboardManager.IsFirstKeyPress(Keys.NumPad2))
                checkNext(2);
            if (this.keyboardManager.IsFirstKeyPress(Keys.NumPad3))
                checkNext(3);
            if (this.keyboardManager.IsFirstKeyPress(Keys.NumPad4))
                checkNext(4);
            if (this.keyboardManager.IsFirstKeyPress(Keys.NumPad5))
                checkNext(5);
        }

        private void demoCameraTrack(GameTime gameTime)
        {
            Vector3 translation, look, up;

            this.cameraTrack.Evalulate((float)gameTime.TotalGameTime.TotalMilliseconds,
                1, out translation, out look, out up);

            Console.WriteLine("Translation: " + translation
                + " - " + gameTime.TotalGameTime.TotalMilliseconds);
        }

        private void demoCameraLayout()
        {
            if (this.keyboardManager.IsFirstKeyPress(Keys.F1))
                this.cameraManager.SetCamera("FullScreen", "RailCamera1");
            else if (this.keyboardManager.IsFirstKeyPress(Keys.F2))
                this.cameraManager.SetCameraLayout("FirstPersonFullScreen");
            else if (this.keyboardManager.IsFirstKeyPress(Keys.F3))
                this.cameraManager.SetCamera("FullScreen", "RailCamera2");
            else if (this.keyboardManager.IsFirstKeyPress(Keys.F4))
                this.cameraManager.SetCamera("FullScreen", "RailCamera3");
            else if (this.keyboardManager.IsFirstKeyPress(Keys.F5))
                this.cameraManager.SetCameraLayout("RailCharacterFollow");
            else if (this.keyboardManager.IsFirstKeyPress(Keys.F6))
            {
                this.cameraManager.SetCameraLayout("RotateCamera");
                ((RotatorController)((PawnCamera3D)this.cameraManager[0]).ControllerList[0]).Set();
            }
            else if (this.keyboardManager.IsFirstKeyPress(Keys.F7))
                this.cameraManager.SetCameraLayout("ZoomOnDoor");
        }

        private void demoRotation()
        {
            if (this.keyboardManager.IsKeyDown(Keys.H))
                this.rotator.Transform3D.RotateAroundYBy(-1);
            else if (this.keyboardManager.IsKeyDown(Keys.J))
                this.rotator.Transform3D.RotateAroundYBy(1);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gold);

            graphics.GraphicsDevice.SamplerStates[0] 
                            = SamplerState.LinearWrap;

            //RasterizerState rasterizer = new RasterizerState();
            //rasterizer.FillMode = FillMode.WireFrame;
            //graphics.GraphicsDevice.RasterizerState = rasterizer;

            //spriteBatch.Begin();
            //spriteBatch.DrawString(this.debugFont, "Keys: Camera - U, D, L, R arrow keys", new Vector2(10, 10), Color.White);
            //spriteBatch.DrawString(this.debugFont,
            //      "Camera Postion: "
            //      + this.activeCamera.Transform3D.Translation,
            //      new Vector2(10, 30), Color.White);
            //spriteBatch.End();
            
            this.graphics.GraphicsDevice.Viewport = this.CameraManager.ActiveCamera.Viewport;
            base.Draw(gameTime);
        }

        public void checkNext(int step)
        {
            if(step == this.nextStep)
            {
                switch (step)
                {
                    case 1:
                        ((OffsetController)step1.ControllerList[0]).Set();
                        ((OffsetController)rotator.ControllerList[0]).Set();
                        Transform3D oldposition = this.CameraManager.ActiveCamera.Transform3D;
                        this.CameraManager.SetCamera("FullScreen", "ZoomOnDoor");
                        this.CameraManager.ActiveCamera.Transform3D = oldposition;
                        ((ShakeController)((PawnCamera3D)this.CameraManager.ActiveCamera).ControllerList[0]).Start("FullScreen", "RailCamera1");
                        this.soundManager.PlayCue("Stone_Door");
                        break;
                    case 2:
                        ((OffsetController)step2.ControllerList[0]).Set();
                        ((OffsetController)step3.ControllerList[0]).Unset();
                        ((OffsetController)step4.ControllerList[0]).Unset();
                        ((OffsetController)step5.ControllerList[0]).Unset();
                        this.soundManager.PlayCue("Pressure_Plate");
                        break;
                    case 3:
                        ((OffsetController)step3.ControllerList[0]).Set();
                        this.soundManager.PlayCue("Pressure_Plate");
                        break;
                    case 4:
                        ((OffsetController)step4.ControllerList[0]).Set();
                        this.soundManager.PlayCue("Pressure_Plate");
                        break;
                    case 5:
                        ((OffsetController)step5.ControllerList[0]).Set();
                        ((OffsetController)doorActor.ControllerList[0]).Set();
                        oldposition = this.CameraManager.ActiveCamera.Transform3D;
                        string activeC = this.CameraManager.ActiveCamera.ID;
                        this.CameraManager.SetCamera("FullScreen", "ZoomOnDoor");
                        this.CameraManager.ActiveCamera.Transform3D = oldposition;
                        ((ShakeController)((PawnCamera3D)this.CameraManager.ActiveCamera).ControllerList[0]).Start("FullScreen", activeC);
                        this.soundManager.PlayCue("Stone_Door");
                        break;
                    case 6:
                        //win
                        this.menuManager.OpenMenu("Win");
                        break;
                }
                this.nextStep++;
            }
            else
            {
                switch (step)
                {
                    case 1:
                        break;
                    case 5:
                        ((OffsetController)step5.ControllerList[0]).Set();
                        ((OffsetController)step4.ControllerList[0]).Unset();
                        ((OffsetController)step3.ControllerList[0]).Unset();
                        ((OffsetController)step2.ControllerList[0]).Unset();
                        this.soundManager.PlayCue("Pressure_Plate");
                        goto case 2;
                    case 4:
                        ((OffsetController)step4.ControllerList[0]).Set();
                        ((OffsetController)step3.ControllerList[0]).Unset();
                        ((OffsetController)step2.ControllerList[0]).Unset();
                        this.soundManager.PlayCue("Pressure_Plate");
                        goto case 2;
                    case 3:
                        ((OffsetController)step3.ControllerList[0]).Set();
                        ((OffsetController)step2.ControllerList[0]).Unset();
                        this.soundManager.PlayCue("Pressure_Plate");
                        goto case 2;
                    case 2:
                        nextStep = 2;
                        if (mistake)
                        {
                            bReset = true;
                            ((OffsetController)arrow1.ControllerList[0]).Set();
                            ((OffsetController)arrow2.ControllerList[0]).Set();
                            ((OffsetController)arrow3.ControllerList[0]).Set();
                            ((OffsetController)arrow4.ControllerList[0]).Set();
                            ((OffsetController)arrow5.ControllerList[0]).Set();
                            ((OffsetController)arrow6.ControllerList[0]).Set();
                            ((OffsetController)arrow7.ControllerList[0]).Set();
                            ((OffsetController)arrow8.ControllerList[0]).Set();
                            this.soundManager.PlayCue("Arrow");
                        }
                        else
                        {
                            mistake = true;
                            ((OffsetController)trap1.ControllerList[0]).Set();
                            ((OffsetController)trap2.ControllerList[0]).Set();
                            ((OffsetController)trap3.ControllerList[0]).Set();
                            ((OffsetController)trap4.ControllerList[0]).Set();
                            ((OffsetController)trap5.ControllerList[0]).Set();
                            ((OffsetController)trap6.ControllerList[0]).Set();
                            ((OffsetController)trap7.ControllerList[0]).Set();
                            ((OffsetController)trap8.ControllerList[0]).Set();
                            this.soundManager.PlayCue("Arrow_Load");

                        }
                        break;
                    case 6:
                        break;
                }
            }
        }

        public void Reset()
        {
            Components.Clear();
            Initialize();
        }
    }
}
