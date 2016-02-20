using Microsoft.Xna.Framework;
using System;

namespace GDLibrary
{
    //Represents a point on a camera curve (i.e. position, look, and up) at a specified time in seconds
    public class Camera3DTrack
    {
        private Curve3D translationCurve, lookCurve, upCurve;

        public Camera3DTrack(CurveLoopType curveLoopType)
        {
            this.translationCurve = new Curve3D(curveLoopType);
            this.lookCurve = new Curve3D(curveLoopType);
            this.upCurve = new Curve3D(curveLoopType);
        }

        public void Add(Vector3 translation, 
            Vector3 look, Vector3 up, float timeInSecs)
        {
            this.translationCurve.Add(translation, timeInSecs);
            this.lookCurve.Add(look, timeInSecs);
            this.upCurve.Add(up, timeInSecs);
        }

        public void Clear()
        {
            this.translationCurve.Clear();
            this.lookCurve.Clear();
            this.upCurve.Clear();
        }

        //See https://msdn.microsoft.com/en-us/library/t3c3bfhx.aspx for information on using the out keyword
        public void Evalulate(float timeInSecs, int precision,
            out Vector3 translation, out Vector3 look, out Vector3 up)
        {
            translation = this.translationCurve.Evaluate(timeInSecs, precision);
            look = this.lookCurve.Evaluate(timeInSecs, precision);
            up = this.upCurve.Evaluate(timeInSecs, precision);
        }
    }
}
