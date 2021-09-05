using System;

namespace ComboSystem
{
    [Flags]
    public enum ModifierProperties
    {
        None = 0,
        Stackable = 1,
        Refreshable = 2,
    }

    public enum EffectType
    {
        None = 0,
        /// <summary>
        /// Deal damage
        /// </summary>
        Damage = 1,
        StatChange = 2,
    }

    [Flags]
    public enum AddModifierParameters
    {
        None = 0,
        OwnerIsTarget = 1,
        CheckRecipes = 2,
        NullStartTarget = 4,
        
        Default = OwnerIsTarget | CheckRecipes
    }
}