using Microsoft.Xna.Framework;

namespace GDLibrary
{
    public class Curve2D
    {
        private Curve1D xCurve, yCurve;
        private CurveLoopType curveLookType;

        public CurveLoopType CurveLookType
        {
            get
            {
                return curveLookType;
            }

        }
        public Curve2D(CurveLoopType curveLoopType)
        {
            this.curveLookType = curveLoopType;

            this.xCurve = new Curve1D(curveLoopType);
            this.yCurve = new Curve1D(curveLoopType);
        }

        public void Add(Vector2 value, float time)
        {
            this.xCurve.Add(value.X, time);
            this.yCurve.Add(value.Y, time);
        }

        public void Clear()
        {
            this.xCurve.Clear();
            this.yCurve.Clear();
        }

        public Vector2 Evaluate(float timeInSecs, int decimalPrecision)
        {
            return new Vector2(this.xCurve.Evaluate(timeInSecs, decimalPrecision), this.yCurve.Evaluate(timeInSecs, decimalPrecision));
        }
    }
}
