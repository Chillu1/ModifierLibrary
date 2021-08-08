using System;
using System.Collections.Generic;

namespace ComboSystem
{
    public class Player : Character
    {
        public Player(Func<Dictionary<string, Modifier>, List<ComboModifier>> checkForRecipes) : base(checkForRecipes)
        {
            Name = nameof(Player);
            MaxHealth = 10;
            Health = 10;
            MovementSpeed = 5;
        }
    }
}