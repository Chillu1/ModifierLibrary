using System;
using System.Linq;

namespace ModifierSystem
{
    [Flags]
    public enum PositiveNegative//TODO RENAME
    {
        None = 0,
        Positive = 1,
        Negative = 2,

        All = Positive | Negative
    }

    public static class PositiveNegativeHelper
    {
        public static readonly PositiveNegative[] PositiveNegatives = Enum.GetValues(typeof(PositiveNegative)).Cast<PositiveNegative>().ToArray();
    }
}