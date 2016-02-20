using Microsoft.Xna.Framework;
using System;

namespace GDLibrary
{
    public class Curve1D
    {
        #region Variables
        private Curve curve;
        private CurveLoopType curveLookType;
        private bool bSet;
        #endregion

        #region Properties
        public CurveLoopType CurveLookType
        {
            get
            {
                return curveLookType;
            }
        }
        public Curve Curve
        {
            get
            {
                return curve;
            }
        }
        #endregion

        //See CurveLoopType - https://msdn.microsoft.com/en-us/library/microsoft.xna.framework.curvelooptype.aspx
        public Curve1D(CurveLoopType curveLookType)
        {
            this.curveLookType = curveLookType;

            this.curve = new Curve();
            this.curve.PreLoop = curveLookType;
            this.curve.PostLoop = curveLookType;
        }

        public void Add(float value, float timeInSecs)
        {
            timeInSecs *= 1000; //convert to milliseconds
            this.curve.Keys.Add(new CurveKey(timeInSecs, value));
            this.bSet = false;
            Set();
        }

        public void Set()
        {
            SetTangents(curve);
            this.bSet = true;
        }

        public void Clear()
        {
            this.curve.Keys.Clear(); 
        }

        public float Evaluate(float timeInSecs, int decimalPrecision)
        {
            if (!bSet)
                Set();

            return (float)Math.Round(this.curve.Evaluate(timeInSecs), decimalPrecision);
        }

        private void SetTangents(Curve curve)
        {
            CurveKey prev;
            CurveKey current;
            CurveKey next;
            int prevIndex;
            int nextIndex;
            for (int i = 0; i < curve.Keys.Count; i++)
            {
                prevIndex = i - 1;
                if (prevIndex < 0) prevIndex = i;

                nextIndex = i + 1;
                if (nextIndex == curve.Keys.Count) nextIndex = i;

                prev = curve.Keys[prevIndex];
                next = curve.Keys[nextIndex];
                current = curve.Keys[i];
                SetCurveKeyTangent(ref prev, ref current, ref next);
                curve.Keys[i] = current;
            }
        }

        private static void SetCurveKeyTangent(ref CurveKey prev, ref CurveKey cur, ref CurveKey next)
        {
            float dt = next.Position - prev.Position;
            float dv = next.Value - prev.Value;
            if (Math.Abs(dv) < float.Epsilon)
            {
                cur.TangentIn = 0;
                cur.TangentOut = 0;
            }
            else
            {
                // The in and out tangents should be equal to the 
                // slope between the adjacent keys.
                cur.TangentIn = dv * (cur.Position - prev.Position) / dt;
                cur.TangentOut = dv * (next.Position - cur.Position) / dt;
            }
        }

        //to do - clone, dispose
    }
}
