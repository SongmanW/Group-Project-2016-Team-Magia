using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GDApp.GDLibrary;
using GDLibrary;
using System;

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
        private BasicEffect effect;
        private SpriteFont debugFont;
        private BasicEffect textureEffect;
        private Texture2D texture;
        private Vector2 screenCentre;

        private Camera3D activeCamera;
        private CameraManager cameraManager;
        private KeyboardManager keyboardManager;
        private MouseManager mouseManager;
        private ObjectManager objectManager;
        public PawnModelObject playerActor;
        private GenericDictionary<string, Texture2D> textureDictionary;
        private GenericDictionary<string, SpriteFont> fontDictionary;
        private GenericDictionary<string, Model> modelDictionary;
        private GenericDictionary<string, Camera3DTrack> trackDictionary;
        private Camera3DTrack cameraTrack;
        private ModelObject doorActor;
        private PawnModelObject rotator;

        #endregion

        #region Properties
        public Vector2 ScreenCentre
        {
            get
            {
                return this.screenCentre;
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
        public Camera3D ActiveCamera
        {
            get
            {
                return this.activeCamera;
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
            InitializeStatics();
            IntializeGraphics(1024, 768);
            InitializeFonts();
            InitializeManagers(true);
            InitializeDictionaries();
            InitializeEffect();

            //InitializeSkyBox(1000);
            LoadModels();
            InitializeModels();

            InitializeCameraTracks();
            InitializeCamera();


            InitializeLevel();
            base.Initialize();
        }

        private void LoadModels()
        {
            Model model = null;

            model = Content.Load<Model>("Assets\\Models\\mm");
            this.modelDictionary.Add("player", model);

            model = Content.Load<Model>("Assets\\Models\\door");
            this.modelDictionary.Add("door", model);

            model = Content.Load<Model>("Assets\\Models\\room");
            this.modelDictionary.Add("room", model);

            model = Content.Load<Model>("Assets\\Models\\rotation");
            this.modelDictionary.Add("rotation", model);

            model = Content.Load<Model>("Assets\\Models\\wall");
            this.modelDictionary.Add("wall", model);

            model = Content.Load<Model>("Assets\\Models\\plate");
            this.modelDictionary.Add("plate", model);
        }

        private void InitializeLevel()
        {
            ((CharacterMoveController)this.playerActor.ControllerList[0]).Camera = this.cameraManager[0];
            ((CharacterRotatorInteractionController)this.playerActor.ControllerList[1]).TargetActor = this.rotator;
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
            PawnModelObject pawnObject = null;


            #region Player Model
            model = this.modelDictionary["player"];
            transform = new Transform3D(new Vector3(-300, 24, 0),
                new Vector3(0, 180, 0), 0.1f * Vector3.One,
                Vector3.UnitX, Vector3.UnitY);
            this.playerActor = new PawnModelObject("m",
                ObjectType.Player, transform, null, model);
            this.playerActor.Add(new CharacterMoveController(this, "character move controller", this.playerActor));
            this.playerActor.Add(new CharacterRotatorInteractionController(this, "character rotator interaction controller", this.playerActor));

            this.objectManager.Add(this.playerActor);
            #endregion

            texture = Content.Load<Texture2D>("Assets\\Textures\\Game\\whitedoor");
            #region ExitDoor Model
            model = this.modelDictionary["door"];
            transform = new Transform3D(new Vector3(300, 0, 0), Vector3.Zero, 0.1f * Vector3.One, -Vector3.UnitZ, Vector3.UnitY);
            this.doorActor = new ModelObject("door", ObjectType.Door, transform, texture, model);

            this.objectManager.Add(this.doorActor);
            #endregion
            #region EntranceDoor Model
            //model = this.modelDictionary["door"];
            //transform = new Transform3D(new Vector3(-300, 0, 0), Vector3.Zero, 0.1f * Vector3.One, -Vector3.UnitZ, Vector3.UnitY);
            //modelobject = new ModelObject("entrance", ObjectType.Door, transform, null, model);

            //this.objectManager.Add(modelobject);
            #endregion
            texture = Content.Load<Texture2D>("Assets\\Textures\\Game\\white");
            #region Room Model
            model = this.modelDictionary["room"];
            transform = new Transform3D(new Vector3(0, 0, 0), Vector3.Zero, 0.1f * Vector3.One, -Vector3.UnitZ, Vector3.UnitY);
            modelObject = new ModelObject("room", ObjectType.Wall, transform, texture, model);

            this.objectManager.Add(modelObject);
            #endregion

            #region Rotationthingy Model
            model = this.modelDictionary["rotation"];
            transform = new Transform3D(new Vector3(0, 0, 0), Vector3.Zero, 0.1f * Vector3.One, -Vector3.UnitZ, Vector3.UnitY);
            this.rotator = new PawnModelObject("RotationThingy", ObjectType.Rotation, transform, texture, model);

            this.objectManager.Add(this.rotator);
            #endregion

            //Walls are initialized after Rotator because of their dependancy on Rotator (see RotatorController)
            //Maybe we should rather go for a more active approach meaning Rotator gets everything which is gonna rotate around it and rotates it
            #region Wall right Model
            model = this.modelDictionary["wall"];
            transform = new Transform3D(new Vector3(0, 0, 153.7244f), Vector3.Zero, 0.1f * Vector3.One, -Vector3.UnitZ, Vector3.UnitY);
            pawnObject = new PawnModelObject("Wall1", ObjectType.Wall, transform, texture, model);
            pawnObject.Add(new RotatorController("wall1Rotator", pawnObject, this.rotator));
            ((RotatorController)pawnObject.ControllerList[0]).Set();

            this.objectManager.Add(pawnObject);
            #endregion
            #region Wall left Model
            model = this.modelDictionary["wall"];
            transform = new Transform3D(new Vector3(0, 0, -153.54f), new Vector3(0,180,0), 0.1f * Vector3.One, -Vector3.UnitZ, Vector3.UnitY);
            pawnObject = new PawnModelObject("Wall2", ObjectType.Wall, transform, texture, model);
            pawnObject.Add(new RotatorController("wall2Rotator", pawnObject, this.rotator));
            ((RotatorController)pawnObject.ControllerList[0]).Set();

            this.objectManager.Add(pawnObject);
            #endregion

            #region Pressure Plate Exit Model
            model = this.modelDictionary["plate"];
            transform = new Transform3D(new Vector3(250, 4.75f, 0), Vector3.Zero, Vector3.One, -Vector3.UnitZ, Vector3.UnitY);
            modelObject = new ModelObject("PressurePlate1", ObjectType.Plate, transform, texture, model);

            this.objectManager.Add(modelObject);
            #endregion
            #region Pressure Plate Right Up Model
            transform = new Transform3D(new Vector3(50, 4.75f, 230), Vector3.Zero, Vector3.One, -Vector3.UnitZ, Vector3.UnitY);
            modelObject = new ModelObject("PressurePlate2", ObjectType.Plate, transform, texture, model);

            this.objectManager.Add(modelObject);
            #endregion
            #region Pressure Plate Right Down Model
            transform = new Transform3D(new Vector3(-50, 4.75f, 230), Vector3.Zero, Vector3.One, -Vector3.UnitZ, Vector3.UnitY);
            modelObject = new ModelObject("PressurePlate2", ObjectType.Plate, transform, texture, model);

            this.objectManager.Add(modelObject);
            #endregion
            #region Pressure Plate Left Up Model
            transform = new Transform3D(new Vector3(50, 4.75f, -230), Vector3.Zero, Vector3.One, -Vector3.UnitZ, Vector3.UnitY);
            modelObject = new ModelObject("PressurePlate2", ObjectType.Plate, transform, texture, model);

            this.objectManager.Add(modelObject);
            #endregion
            #region Pressure Plate Left Down Model
            transform = new Transform3D(new Vector3(-50, 4.75f, -230), Vector3.Zero, Vector3.One, -Vector3.UnitZ, Vector3.UnitY);
            modelObject = new ModelObject("PressurePlate2", ObjectType.Plate, transform, texture, model);

            this.objectManager.Add(modelObject);
            #endregion

        }

        private void InitializeSkyBox(int scale)
        {
            VertexPositionColorTexture[] vertices = VertexFactory.GetTextureQuadVertices();

            Texture2D texture = null;
            Transform3D transform = null;
            TexturedPrimitiveObject texturedPrimitive = null, clone = null;

            int halfScale = scale / 2;

            texture = Content.Load<Texture2D>("Assets\\Textures\\Game\\Skybox\\back");
            transform = new Transform3D(new Vector3(0, 0, -halfScale), new Vector3(0, 0, 0),
                scale * Vector3.One, Vector3.UnitZ, Vector3.UnitY);

            texturedPrimitive = new TexturedPrimitiveObject("sky", ObjectType.Decorator,
                transform, vertices, this.textureEffect, PrimitiveType.TriangleStrip, 2, texture);
            this.objectManager.Add(texturedPrimitive);

            //top
            clone = (TexturedPrimitiveObject)texturedPrimitive.Clone();
            clone.Texture = Content.Load<Texture2D>("Assets\\Textures\\Game\\Skybox\\sky");
            clone.Transform3D.Translation = new Vector3(0, halfScale, 0);
            clone.Transform3D.Rotation = new Vector3(90, -90, 0);
            this.objectManager.Add(clone);

            //left
            clone = (TexturedPrimitiveObject)texturedPrimitive.Clone();
            clone.Texture = Content.Load<Texture2D>("Assets\\Textures\\Game\\Skybox\\left");
            clone.Transform3D.Translation = new Vector3(-halfScale, 0, 0);
            clone.Transform3D.Rotation = new Vector3(0, 90, 0);
            this.objectManager.Add(clone);

            //right
            clone = (TexturedPrimitiveObject)texturedPrimitive.Clone();
            clone.Texture = Content.Load<Texture2D>("Assets\\Textures\\Game\\Skybox\\right");
            clone.Transform3D.Translation = new Vector3(halfScale, 0, 0);
            clone.Transform3D.Rotation = new Vector3(0, -90, 0);
            this.objectManager.Add(clone);
          
            //front
            clone = (TexturedPrimitiveObject)texturedPrimitive.Clone();
            clone.Texture = Content.Load<Texture2D>("Assets\\Textures\\Game\\Skybox\\front");
            clone.Transform3D.Translation = new Vector3(0, 0, halfScale);
            clone.Transform3D.Rotation = new Vector3(0, 180, 0);
            this.objectManager.Add(clone);

            //grass
            clone = (TexturedPrimitiveObject)texturedPrimitive.Clone();
            clone.Texture = Content.Load<Texture2D>("Assets\\Textures\\Game\\Foliage\\Ground\\grass1");
            clone.Transform3D.Translation = new Vector3(0, -5, 0);
            clone.Transform3D.Rotation = new Vector3(-90, 0, 0);
            this.objectManager.Add(clone);
        }

        private void IntializeGraphics(int width, int height)
        {
            this.graphics.PreferredBackBufferWidth = width;
            this.graphics.PreferredBackBufferHeight = height;
            this.screenCentre = new Vector2(width/2, height/2);
            this.graphics.ApplyChanges();
        }

        private void InitializeStatics()
        {
            Actor.game = this;
        }

        private void InitializeManagers(bool isMouseVisible)
        {
            this.cameraManager = new CameraManager(this);
            Components.Add(this.cameraManager);

            this.keyboardManager = new KeyboardManager(this);
            Components.Add(this.keyboardManager);

            this.mouseManager = new MouseManager(this, isMouseVisible);
            //centre the mouse otherwise the movement for 1st person camera will be unpredictable
            this.mouseManager.SetPosition(this.screenCentre); 
            Components.Add(this.mouseManager);

            this.objectManager = new ObjectManager(this, 10, 10);
            Components.Add(this.objectManager);

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

        private void InitializeTexturedPrimitives()
        {
            RasterizerState rasterizerState = new RasterizerState();

            this.texture = Content.Load<Texture2D>("Assets\\Textures\\Game\\Foliage\\Trees\\tree1");
            Components.Add(new TexturedQuad(this, this.texture, this.textureEffect,
                    rasterizerState, new Vector3(-1, 0, 0),
                    new Vector3(0, 0, 0), new Vector3(1, 2, 1), Color.White));

            this.texture = Content.Load<Texture2D>("Assets\\Debug\\Textures\\slj");
            Components.Add(new TexturedQuad(this, this.texture, this.textureEffect,
                    rasterizerState, new Vector3(0, 0, 0),
                    new Vector3(0, 0, 0), new Vector3(1,2,1), Color.Yellow));

            this.texture = Content.Load<Texture2D>("Assets\\Debug\\Textures\\ml");
            Components.Add(new TexturedQuad(this, this.texture, this.textureEffect,
                    rasterizerState, new Vector3(0.5f, 0, 0.5f), //remember that the unscaled quad is 1x1x1
                    new Vector3(0, MathHelper.ToRadians(-90), 0), //rotate around Y by 90 degrees counter clockwise (point your right hand thumb in direction of axis and roll your fingers into a ball)
                    Vector3.One, Color.White));

 
        }

        private void InitializeFonts()
        {
            this.debugFont = Content.Load<SpriteFont>("Assets\\Debug\\Fonts\\debug");
        }

        private void InitializeWireframePrimitives()
        {
            RasterizerState rasterizerState = new RasterizerState();

            //OriginHelper originHelper
            //    = new OriginHelper(this, this.effect,
            //        rasterizerState, Vector3.Zero,
            //        Vector3.Zero, Vector3.One);

            //Components.Add(originHelper);

            //creates the vertices and passes them each DRAW call to the GPU - inefficient
            Components.Add(new UnbufferedCubeHelper(this, this.effect,
                  rasterizerState, new Vector3(-5, 2, 0),
                  Vector3.Zero, 0.5f * Vector3.One, Color.Red));  //left of the origin

            //creates the vertices BUT moves the data onto a vertex buffer on the VRAM of the GPU - efficient but stil requires a large number of duplicate vertices to draw the cube
            Components.Add(new BufferedCubeHelper(this, this.effect,
                  rasterizerState, new Vector3(0, 2, 0),
                  new Vector3(0, MathHelper.ToRadians(45), 0), new Vector3(1, 2, 1), Color.Green)); //rotated 45 degrees on Y axis, above the origin, scaled on Y axis

            //creates the vertices and indices BUT moves the data onto a vertex and index buffer on the VRAM of the GPU - MOST efficient since indices indicate which vertices from the vertex buffer are re-used
            Components.Add(new IndexedCubeHelper(this, this.effect,
               rasterizerState, new Vector3(5, 2, 0),
               new Vector3(0, 0, 0), new Vector3(2, 1, 1), Color.Blue)); //no rotation, above the origin, scaled on X axis
        }

        private void InitializeEffect()
        {
            this.effect = new BasicEffect(graphics.GraphicsDevice);
            this.effect.VertexColorEnabled = true;

            this.textureEffect = new BasicEffect(graphics.GraphicsDevice);
            this.textureEffect.VertexColorEnabled = true;
            this.textureEffect.TextureEnabled = true;  
        }

        private void InitializeCamera()
        {
            Camera3D clone = null;
            Camera3D camera = null;
            PawnCamera3D pawnCamera = null;

            #region Track
            camera = new TrackCamera3D("track", ObjectType.TrackCamera,
                Transform3D.Zero, ProjectionParameters.StandardMediumFourThree,
                this.graphics.GraphicsDevice.Viewport, 
                this.trackDictionary["puke"]);
            this.cameraManager.Add("Track", camera);
            #endregion


            camera = new FreeLookCamera3D("full", ObjectType.FirstPersonCamera,
                   new Transform3D(new Vector3(0,0,5), -Vector3.UnitZ, Vector3.UnitY),
                       ProjectionParameters.StandardMediumFourThree, this.graphics.GraphicsDevice.Viewport, GameData.CameraSpeed);

            #region Fullscreen
            this.cameraManager.Add("FullScreen", camera);
            #endregion

            #region Rail
            RailCamera3D railCamera = new RailCamera3D("rail", ObjectType.RailCamera,
                Transform3D.Zero, ProjectionParameters.StandardMediumFourThree,
                        this.graphics.GraphicsDevice.Viewport,
                    new RailParameters("r1", new Vector3(-20, 20, 10), 
                                        new Vector3(20, 1, 10)),
                        playerActor);

            this.cameraManager.Add("Rail", railCamera);
            #endregion

            #region Two Horizontal
            //front
            clone = (Camera3D)camera.Clone();
            clone.ID = "front";
            clone.Transform3D.Translation = new Vector3(0, 0, 5);
            clone.Transform3D.Look = -Vector3.UnitZ;
            clone.ViewportWidth /= 2;
            this.cameraManager.Add("TwoHorizontal", clone);

            //left
            clone = (Camera3D)camera.Clone();
            clone.ID = "left";
            clone.Transform3D.Translation = new Vector3(-5, 0, 0);
            clone.Transform3D.Look = Vector3.UnitX;
            clone.ViewportWidth /= 2;
            clone.ViewportX = clone.ViewportWidth;
            this.cameraManager.Add("TwoHorizontal", clone);
            #endregion

            #region RailCharacterFollow

            pawnCamera = new PawnCamera3D("rail character follow camera 1",
                ObjectType.ZoomOnDoorCamera, Transform3D.Zero,
                ProjectionParameters.StandardMediumFourThree, this.graphics.GraphicsDevice.Viewport);

            pawnCamera.Add(new RailCharacterFollowController("rail character follow controller 1",
                pawnCamera, new RailParameters("r1", new Vector3(-300, 126, 0),
                new Vector3(300, 126, 0)), playerActor, new Vector3(300, -300, 0), 114));
            this.cameraManager.Add("RailCharacterFollow", pawnCamera);

            #endregion

            #region RotateCamera

            pawnCamera = new PawnCamera3D("RotateCamera",
                ObjectType.ZoomOnDoorCamera, new Transform3D(new Vector3(50,100,50), Vector3.UnitZ, Vector3.UnitY),
                ProjectionParameters.StandardMediumFourThree, this.graphics.GraphicsDevice.Viewport);

            pawnCamera.Add(new RotatorController("rotate camera controller 1",
                pawnCamera, this.rotator));
            this.cameraManager.Add("RotateCamera", pawnCamera);

            #endregion

            #region ZoomOnDoor

            pawnCamera = new PawnCamera3D("ZoomOnDoor",
                ObjectType.ZoomOnDoorCamera, new Transform3D(new Vector3(100, 126, 0), new Vector3(100,-51,0), Vector3.UnitY),
                ProjectionParameters.StandardMediumFourThree, this.graphics.GraphicsDevice.Viewport);
            
            this.cameraManager.Add("ZoomOnDoor", pawnCamera);

            #endregion

            //set the default layout
            this.cameraManager.SetCameraLayout("RailCharacterFollow");
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

            demoCameraLayout();
            //demoCameraTrack(gameTime);
            //demoRotation();

            base.Update(gameTime);
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
                this.cameraManager.SetCameraLayout("FullScreen");
            else if (this.keyboardManager.IsFirstKeyPress(Keys.F2))
                this.cameraManager.SetCameraLayout("TwoHorizontal");
            else if (this.keyboardManager.IsFirstKeyPress(Keys.F3))
                this.cameraManager.SetCameraLayout("Rail");
            else if (this.keyboardManager.IsFirstKeyPress(Keys.F4))
                this.cameraManager.SetCameraLayout("Track");
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            graphics.GraphicsDevice.SamplerStates[0] 
                            = SamplerState.LinearClamp;

            //spriteBatch.Begin();
            //spriteBatch.DrawString(this.debugFont, "Keys: Camera - U, D, L, R arrow keys", new Vector2(10, 10), Color.White);
            //spriteBatch.DrawString(this.debugFont,
            //      "Camera Postion: "
            //      + this.activeCamera.Transform3D.Translation,
            //      new Vector2(10, 30), Color.White);
            //spriteBatch.End();

            for (int i = 0; i < cameraManager.Size; i++)
            {
                this.activeCamera = cameraManager[i];
                this.graphics.GraphicsDevice.Viewport = this.activeCamera.Viewport;
                base.Draw(gameTime);
            }
        }

      
    }
}
