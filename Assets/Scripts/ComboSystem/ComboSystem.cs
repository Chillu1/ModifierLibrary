using System;

namespace ComboSystem
{
    public enum StatType
    {
        None = 0,
        Attack = 1,
        Defense = 2,
        MovementSpeed = 3,
        Health = 4,
    }

    [Flags]
    public enum DamageType
    {
        None = 0,
        Physical = 1,
        Magical = 2,
        Poison = 4,
        Fire = 8,
        Acid = 16,
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
}