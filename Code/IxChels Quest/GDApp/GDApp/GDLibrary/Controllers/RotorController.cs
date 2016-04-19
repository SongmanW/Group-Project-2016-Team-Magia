using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GDLibrary
{
    public class RotorController : Controller
    {
        private bool running;
        private float rotated;
        private float timeToTake;
        private bool clockwise;

        public RotorController(string name, Actor parentActor, bool bEnabled)
            :base(name, parentActor, bEnabled)
        {
            this.running = false;
        }

        public override void Update(GameTime gameTime)
        {
            if(running)
            {
                if(rotated < 90)
                {
                    float magnitude = 90 / timeToTake * gameTime.ElapsedGameTime.Milliseconds;
                    rotated += magnitude;
                    if (clockwise)
                        magnitude *= -1;
                    this.ParentActor.Transform3D.RotateAroundYBy(magnitude);
                }
                else
                {
                    float magnitude = -(rotated - 90);
                    if (clockwise)
                        magnitude *= -1;
                    this.ParentActor.Transform3D.RotateAroundYBy(magnitude);
                    this.running = false;
                    EventDispatcher.Publish(new EventData("stopped rotating", this, EventType.OnRotationEnd));
                }
            }
        }

        public void Rotate(float degree, float timeToTake, bool clockwise)
        {
            this.rotated = 0;
            this.timeToTake = timeToTake;
            this.clockwise = clockwise;
            this.running = true;
        }
    }
}
