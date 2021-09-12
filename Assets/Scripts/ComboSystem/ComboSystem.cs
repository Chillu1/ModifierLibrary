using System;

namespace ComboSystem
{
    [Flags]
    public enum ModifierProperties : byte
    {
        None = 0,
        Stackable = 1,
        Refreshable = 2,
    }

    public enum EffectType//TODO Redundant?
    {
        None = 0,
        /// <summary>
        /// Deal damage
        /// </summary>
        Damage = 1,
        StatChange = 2,
    }

    [Flags]
    public enum AddModifierParameters : byte
    {
        None = 0,
        OwnerIsTarget = 1,
        /// <summary>
        ///     Check combo recipes on apply
        /// </summary>
        CheckRecipes = 2,
        /// <summary>
        ///     Starts with no targets (ex. damage buff towards enemies)
        /// </summary>
        NullStartTarget = 4,

        DefaultOffensive = CheckRecipes | NullStartTarget,
        Default = OwnerIsTarget | CheckRecipes
    }

    [Flags]
    public enum ActivationCondition
    {
        None = 0,
        Attack = 1,
        Kill = 2,
        Cast = 4,
        Death = 8
    }

    public enum Targets : byte//Not needed?
    {
        None = 0,
        Owner = 1,
        Allies = 2,
        Enemies = 4,

        Friendlies = Owner | Allies,
        Everyone = Owner | Allies | Enemies,
    }
}