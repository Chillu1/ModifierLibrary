using System;

namespace ModifierLibrary
{
    public interface IChanceComponent : IDisplayable
    {
        bool Roll();
        bool Roll(Random random);
    }
}