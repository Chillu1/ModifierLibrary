using System;

namespace ComboSystem
{
    //TODO Make a class for statType that takes statType & value (probably in an array to have multiple stat changes)
    public enum StatType
    {
        None = 0,
        Attack = 1,
        Defense = 2,
        MovementSpeed = 3,
        Health = 4,
        AttackSpeed = 5,
    }

    [Flags]
    public enum DamageType//Might be smart to separate: Physical, Magical, Pure, etc. From elementalTypes?
    {
        None = 0,
        Physical = 1,
        Magical = 2,
        Pure = 4,

        Explosion = 8,

        Poison = 128,//TEMP Bigger numbers to have space for more of these^
        Fire = 256,
        Cold = 512,
        Acid = 1024,
        Toxic = 2048,
        Bleed = 4096,
    }

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

    public enum LogLevel
    {
        None = 0,
        Error = 1,
        Warning = 2,
        Info = 3,
        Verbose = 4,
    }
}