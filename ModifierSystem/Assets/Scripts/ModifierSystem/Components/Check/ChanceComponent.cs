using System;
using BaseProject;

namespace ModifierSystem
{
    public class ChanceComponent : IChanceComponent
    {
        private double Chance { get; }

        private readonly Random _random;

        private const string Info = "Chance: ";

        public ChanceComponent(double chance)
        {
            Validate();
            Chance = chance;
            _random = new Random();
        }

        public bool Roll()
        {
            bool result = Roll(_random);
            //Log.Info(this+"_"+result);
            return result;
        }

        public bool Roll(Random random)
        {
            return random.NextDouble() < Chance;
        }

        private bool Validate()
        {
            bool valid = true;
            if (Chance < 0 || Chance > 1)
            {
                Log.Error(this + ": Chance is not between 0 and 1");
                valid = false;
            }

            return valid;
        }

        public string GetBasicInfo()
        {
            return $"{Info}{Chance * 100d}%\n";
        }

        public override string ToString()
        {
            return GetBasicInfo();
        }
    }
}