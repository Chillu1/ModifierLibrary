using System;

namespace ComboSystem
{
    public class ComboRecipe
    {
        public string[] Ids { get; } //Id's of specific buffs, if applicable
        public DamageType[] DamageTypes { get; }

        //public Stat[] Stats {get; private set; }
        public ComboRecipe(ComboRecipeProperties properties)
        {
            Ids = properties.Ids;
            DamageTypes = properties.DamageTypes;
            if (Ids == null)
                Ids = Array.Empty<string>();
            if (DamageTypes == null)
                DamageTypes = Array.Empty<DamageType>();
        }
    }

    public class ComboRecipeProperties
    {
        public string[] Ids { get; set; } //Id's of specific buffs, if applicable

        public DamageType[] DamageTypes { get; set; }
        //public Stat[] Stats {get; private set; }
    }
}