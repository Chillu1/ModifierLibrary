using System;
using BaseProject;

namespace ModifierSystem
{
    [Flags]
    public enum AddModifierParameters : byte
    {
        None = 0,

        /// <summary>
        ///     All self-buffs
        /// </summary>
        OwnerIsTarget = 1,

        /// <summary>
        ///     Check combo recipes on apply
        /// </summary>
        CheckRecipes = 2,

        /// <summary>
        ///     Starts with no targets (ex. damage buff towards enemies), probably all appliers
        /// </summary>
        NullStartTarget = 4,

        DefaultOffensive = CheckRecipes | NullStartTarget,
        Default = OwnerIsTarget | CheckRecipes
    }

    [Flags]
    public enum ApplierType
    {
        None = 0,
        Attack = 1,
        Cast = 2,
        /// <summary>
        ///     Automatically castable
        /// </summary>
        Aura = 4,
    }

    public enum CostType
    {
        None = 0,
        Health = StatType.Health,
        Mana = StatType.Mana
    }
}