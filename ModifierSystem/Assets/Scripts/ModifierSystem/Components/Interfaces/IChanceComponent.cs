using System;

namespace ModifierSystem
{
    public interface IChanceComponent : IDisplay
    {
        bool Roll();
        bool Roll(Random random);
    }
}