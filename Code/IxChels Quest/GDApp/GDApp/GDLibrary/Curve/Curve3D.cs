using Microsoft.Xna.Framework;

namespace GDLibrary
{
    public class Curve3D
    {
        private Curve1D xCurve, yCurve, zCurve;
        private CurveLoopType curveLookType;

        public CurveLoopType CurveLookType
        {
            get
            {
                return curveLookType;
            }

        }
        public Curve3D(CurveLoopType curveLoopType)
        {
            this.curveLookType = curveLoopType;

            this.xCurve = new Curve1D(curveLoopType);
            this.yCurve = new Curve1D(curveLoopType);
            this.zCurve = new Curve1D(curveLoopType);
        }

        public void Add(Vector3 value, float time)
        {
            this.xCurve.Add(value.X, time);
            this.yCurve.Add(value.Y, time);
            this.zCurve.Add(value.Z, time);
        }

        public void Clear()
        {
            this.xCurve.Clear();
            this.yCurve.Clear();
            this.zCurve.Clear();
        }

        public Vector3 Evaluate(float timeInSecs, int decimalPrecision)
        {
            return new Vector3(this.xCurve.Evaluate(timeInSecs, decimalPrecision), 
                this.yCurve.Evaluate(timeInSecs, decimalPrecision),
                 this.zCurve.Evaluate(timeInSecs, decimalPrecision));
        }
    }
}
