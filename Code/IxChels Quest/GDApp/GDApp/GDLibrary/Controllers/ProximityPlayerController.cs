using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GDLibrary
{
    public class ProximityPlayerController : Controller
    {
        private float distance;
        private Actor targetActor;

        public ProximityPlayerController(string name, Actor parentActor, bool bEnabled, Actor targetActor, float distance)
            :base(name, parentActor, bEnabled)
        {
            this.targetActor = targetActor;
            this.distance = distance;
        }
    }
}
