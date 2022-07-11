using System;

namespace ModifierSystem
{
    public interface IChanceComponent : IDisplayable
    {
        bool Roll();
        bool Roll(Random random);
    }
}