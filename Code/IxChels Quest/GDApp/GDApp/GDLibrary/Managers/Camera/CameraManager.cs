using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GDApp;

namespace GDLibrary
{
    public class CameraManager : GameComponent
    {
        #region Fields
        private Dictionary<CameraLayout, List<Camera3D>>cameraDictionary;
        private List<Camera3D> activeCameraList;
        private CameraLayout currentCameraLayout;
        #endregion

        #region Properties
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
            this.cameraDictionary =
                new Dictionary<CameraLayout, List<Camera3D>>();
        }

        public void Add(CameraLayout cameraLayout, Camera3D camera)
        {
            if (this.cameraDictionary.ContainsKey(cameraLayout))
            {
                List<Camera3D> list = this.cameraDictionary[cameraLayout];

                if(!list.Contains(camera))
                    list.Add(camera);
            }
            else
            {
                List<Camera3D> list = new List<Camera3D>();
                list.Add(camera);
                this.cameraDictionary.Add(cameraLayout, list);
            }
        }

        public void Remove(CameraLayout cameraLayout, string id)
        {
            if (this.cameraDictionary.ContainsKey(cameraLayout))
            {
                List<Camera3D> list = this.cameraDictionary[cameraLayout];

                foreach (Camera3D camera in list)
                {
                    if(camera.ID.Equals(id))
                        list.Remove(camera);
                }
            }
        }

        public bool SetCameraLayout(CameraLayout cameraLayout)
        {
            //if first time and NULL or not the same as current
            if((this.activeCameraList == null) || (!this.currentCameraLayout.Equals(cameraLayout)))
            {
                //if layout exists in the dictionary
                if(this.cameraDictionary.ContainsKey(cameraLayout))
                {
                    this.activeCameraList = this.cameraDictionary[cameraLayout];
                    this.currentCameraLayout = cameraLayout;
                    return true;
                }
            }
            return false;
        }
        

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < this.activeCameraList.Count; i++)
            {
                this.activeCameraList[i].Update(gameTime);
            }
            base.Update(gameTime);
        }
    }
}
