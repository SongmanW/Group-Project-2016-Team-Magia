using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GDLibrary
{
    public class TransitionController : Controller
    {
        private Camera3DTrack track;

        public TransitionController(string name, Actor parentActor,
            Actor actor1, Actor actor2, float timeInSecs)
            : base(name, parentActor)
        {
            this.track = new Camera3DTrack(CurveLoopType.Constant);
            //set the initial position of the camera
            this.track.Add(actor1.Transform3D.Translation, actor1.Transform3D.Look, actor1.Transform3D.Up, 0);
            this.track.Add(actor2.Transform3D.Translation, actor2.Transform3D.Look, actor2.Transform3D.Up, timeInSecs);
        }

        public override void Update(GameTime gameTime)
        {
            Vector3 translation, look, up;
            float timeInSecs = (float)gameTime.TotalGameTime.TotalMilliseconds;

            this.track.Evalulate(timeInSecs, 2, out translation, out look, out up);

            this.ParentActor.Transform3D.Translation = translation;
            this.ParentActor.Transform3D.Look = look;
            this.ParentActor.Transform3D.Up = up;

            base.Update(gameTime);
        }
    }
}
