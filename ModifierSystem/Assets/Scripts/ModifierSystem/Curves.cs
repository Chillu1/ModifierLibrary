using System.Collections.Generic;
using BaseProject;

namespace ModifierSystem
{
    public static class Curves
    {
        public static readonly Curve ComboElementMultiplier = new Curve(new List<CurvePoint>()
        {
            //Value - Multiplier
            new CurvePoint(1, 1),
            new CurvePoint(50, 1.3),
            new CurvePoint(250, 1.6),
            new CurvePoint(1250, 1.9),
            new CurvePoint(62500, 2.2),
            new CurvePoint(312500, 2.5),
            new CurvePoint(1562500, 2.8),
        });
    }
}