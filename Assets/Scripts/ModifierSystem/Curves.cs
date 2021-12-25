using System.Collections.Generic;
using BaseProject;

namespace ModifierSystem
{
    public class Curves
    {
        public static readonly Curve StatusResistance = new Curve(new List<CurvePoint>()
        {
            new CurvePoint(0, 0.00d),
            new CurvePoint(100, 0.2d),
            new CurvePoint(1_000, 0.5d),
            new CurvePoint(10_000, 0.9d),
            new CurvePoint(20_000, 0.95d),
            new CurvePoint(50_000, 0.98d),
            new CurvePoint(1_000_000, 0.99d),
            new CurvePoint(1_000_000_000, 0.995d),
            new CurvePoint(1_000_000_000_000, 0.999d),
        }, negativeAllowed: true);
    }
}