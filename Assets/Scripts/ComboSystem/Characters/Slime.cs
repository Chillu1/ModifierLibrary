using System;
using System.Collections.Generic;

namespace ComboSystem
{
    public class Slime : Character
    {
        public Slime(Func<Dictionary<string, Modifier>, List<ComboModifier>> checkForRecipes) : base(checkForRecipes)
        {
            Name = nameof(Slime);
        }
    }
}