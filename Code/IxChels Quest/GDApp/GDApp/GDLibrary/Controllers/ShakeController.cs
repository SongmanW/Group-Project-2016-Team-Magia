using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GDLibrary
{
    public class ShakeController : Controller
    {
        private int timer;
        private string returnCameraLayout;
        private string returnCameraID;
        private Vector3 oldTranslation;
        private Random random;

        public ShakeController(string name, Actor parentActor, bool bEnabled)
            :base(name, parentActor, bEnabled)
        {
            this.timer = 3000;
        }

        public override void Update(GameTime gameTime)
        {
            if(timer < 1000)
            {
                float range = 1;
                Vector3 randomVector = new Vector3(0,
                    ((float)random.NextDouble()) * range - range/2,
                    ((float)random.NextDouble()) * range - range/2);
                this.ParentActor.Transform3D.Translation = this.oldTranslation + randomVector;
                timer += gameTime.ElapsedGameTime.Milliseconds;
            }
            else
            {
                this.ParentActor.Transform3D.Translation = oldTranslation;
                EventDispatcher.Publish(new CameraEventData("change camera back", this, EventType.OnCameraChanged, returnCameraLayout, returnCameraID));
            }
        }

        public void Start(string cameraLayout, string cameraID)
        {
            this.timer = 0;
            this.returnCameraLayout = cameraLayout;
            this.returnCameraID = cameraID;
            this.oldTranslation = this.ParentActor.Transform3D.Translation;
            Console.WriteLine(this.ParentActor.Transform3D.Translation);
            this.random = new Random();
        }
    }
}
