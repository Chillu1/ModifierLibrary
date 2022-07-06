using System;

namespace ModifierSystem
{
    [Flags]
    public enum StackEffectType
    {
        None = 0,
        Effect = 1,
        Add = 2,
        AddStacksBased = 4,
        Multiply = 8,
        MultiplyStacksBased = 16,
    }
}