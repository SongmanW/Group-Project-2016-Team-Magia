using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GDLibrary
{
    public class TrackCamera3D : Camera3D
    {
        private Camera3DTrack track;

        public TrackCamera3D(string id, ObjectType objectType,
            Transform3D transform, ProjectionParameters projectionParameters,
            Viewport viewPort, Camera3DTrack track)
            : base(id, objectType, transform, projectionParameters, viewPort)
        {
            this.track = track;
        }

        public override void Update(GameTime gameTime)
        {
            Vector3 translation, look, up;
            float timeInSecs = (float)gameTime.TotalGameTime.TotalMilliseconds;

            this.track.Evalulate(timeInSecs, 2, out translation, out look, out up);

            this.Transform3D.Translation = translation;
            this.Transform3D.Look = look;
            this.Transform3D.Up = up;

            base.Update(gameTime);
        }


    }
}
