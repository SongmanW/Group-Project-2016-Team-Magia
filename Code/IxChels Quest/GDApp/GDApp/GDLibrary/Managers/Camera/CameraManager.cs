using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GDApp;
using System;

namespace GDLibrary
{
    public class CameraManager : GameComponent
    {
        #region Fields
        private Dictionary<string, List<Camera3D>> cameraDictionary;
        private List<Camera3D> activeCameraList;
        private string currentCameraLayout;
        private int activeCameraIndex;
        #endregion

        #region Properties
        public Camera3D ActiveCamera
        {
            get
            {
                return this.activeCameraList[this.activeCameraIndex];
            }
        }
        public int ActiveCameraIndex
        {
            set
            {
                this.activeCameraIndex = ((value >= 0)
                    && (value < this.activeCameraList.Count)) ? value : 0;
            }
            get
            {
                return this.activeCameraIndex;
            }
        }
        public string CurrentCameraLayout
        {
            get
            {
                return this.currentCameraLayout;
            }
        }
        public Camera3D this[int index]
        {
            get
            {
                return this.activeCameraList[index];
            }
        }
        public int Size
        {
            get
            {
                return this.activeCameraList.Count;
            }
        }
        #endregion

        public CameraManager(Main game)
            : base(game)
        {
            this.cameraDictionary = new Dictionary<string, List<Camera3D>>();

            game.EventDispatcher.RotationStarted += HandleRotationStarted;
            game.EventDispatcher.RotationEnd += HandleRotationEnd;
            game.EventDispatcher.CameraChange += HandleCameraChanged;
        }

        private void HandleCameraChanged(EventData eventData)
        {
            CameraEventData data = (CameraEventData)eventData;
            this.SetCamera(data.cameraLayout, data.cameraID);
        }

        private void HandleRotationEnd(object sender)
        {
            ((RotatorController)((PawnCamera3D)activeCameraList[activeCameraIndex]).ControllerList[0]).Unset();
            if(Math.Abs(activeCameraList[activeCameraIndex].Transform3D.Look.Z) < 0.1)
            {
                //Set to Central Camera
                Transform3D oldposition = activeCameraList[activeCameraIndex].Transform3D;
                this.SetCamera("FullScreen", "RailCamera1");
                this.activeCameraList[activeCameraIndex].Transform3D = oldposition;
            }
            else
            {
                if(activeCameraList[activeCameraIndex].Transform3D.Look.Z > 0.1)
                {
                    Transform3D oldposition = activeCameraList[activeCameraIndex].Transform3D;
                    this.SetCamera("FullScreen", "RailCamera3");
                    this.activeCameraList[activeCameraIndex].Transform3D = oldposition;
                }
                else
                {
                    Transform3D oldposition = activeCameraList[activeCameraIndex].Transform3D;
                    this.SetCamera("FullScreen", "RailCamera2");
                    this.activeCameraList[activeCameraIndex].Transform3D = oldposition;
                }
            }
        }

        private void HandleRotationStarted(object sender)
        {
            Transform3D oldposition = activeCameraList[activeCameraIndex].Transform3D;
            this.SetCamera("FullScreen", "RotateCamera");
            this.activeCameraList[activeCameraIndex].Transform3D = oldposition;
            ((RotatorController)((PawnCamera3D)activeCameraList[activeCameraIndex]).ControllerList[0]).Set();
        }

        public void Add(string cameraLayout, Camera3D camera)
        {
            if (this.cameraDictionary.ContainsKey(cameraLayout))
            {
                List<Camera3D> list = this.cameraDictionary[cameraLayout];

                if (!list.Contains(camera))
                    list.Add(camera);
            }
            else
            {
                List<Camera3D> list = new List<Camera3D>();
                list.Add(camera);
                this.cameraDictionary.Add(cameraLayout, list);
            }
        }

        public void Remove(string cameraLayout, string id)
        {
            if (this.cameraDictionary.ContainsKey(cameraLayout))
            {
                List<Camera3D> list = this.cameraDictionary[cameraLayout];

                foreach (Camera3D camera in list)
                {
                    if (camera.ID.Equals(id))
                        list.Remove(camera);
                }
            }
        }

        /// <summary>
        /// Call to cycle through the camera in the current list
        /// </summary>
        public void CycleCamera()
        {
            this.ActiveCameraIndex += 1;
        }

        public bool SetCamera(string cameraLayout, string cameraID)
        {
            bool bSuccess = SetCameraLayout(cameraLayout); //set to the appropriate layout
            int index = 0;
            Camera3D camera = null;

            if (bSuccess)
            {
                FindCameraBy(cameraLayout, cameraID, out camera, out index); //find the camera
                ActiveCameraIndex = index; //set to be active
            }

            return (camera != null); //true if we found the camera
        }

        public void FindCameraBy(string cameraLayout, string cameraID, out Camera3D camera, out int index)
        {
            camera = null;
            index = -1;

            List<Camera3D> list = this.cameraDictionary[cameraLayout];

            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].ID.Equals(cameraID))
                    {
                        camera = list[i];
                        index = i;
                        break;
                    }
                }
            }
        }

        public bool SetCameraLayout(string cameraLayout)
        {
            //if first time and NULL or not the same as current
            if ((this.activeCameraList == null) || (!this.currentCameraLayout.Equals(cameraLayout)))
            {
                //if layout exists in the dictionary
                if (this.cameraDictionary.ContainsKey(cameraLayout))
                {
                    this.activeCameraList = this.cameraDictionary[cameraLayout];
                    this.ActiveCameraIndex = 0;
                    this.currentCameraLayout = cameraLayout;
                    return true;
                }
                return false;
            }
            return true;
        }


        public override void Update(GameTime gameTime)
        {
            this.activeCameraList[activeCameraIndex].Update(gameTime);
            base.Update(gameTime);
        }
    }
}
