using System;

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
}