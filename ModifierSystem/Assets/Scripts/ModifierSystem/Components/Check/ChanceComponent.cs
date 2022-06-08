using System;
using BaseProject;

namespace ModifierSystem
{
    public class ChanceComponent : IChanceComponent
    {
        public double Chance { get; }

        private readonly Random _random;

        public ChanceComponent(double chance)
        {
            Chance = chance;
            _random = new Random();
        }

        public bool Roll()
        {
            bool result = Roll(_random);
            return result;
        }

        public bool Roll(Random random)
        {
            return random.NextDouble() < Chance;
        }
    }
}