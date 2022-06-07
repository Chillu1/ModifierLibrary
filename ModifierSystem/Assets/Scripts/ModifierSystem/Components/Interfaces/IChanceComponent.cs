using System;

namespace ModifierSystem
{
    public interface IChanceComponent
    {
        bool Roll();
        bool Roll(Random random);
    }
}