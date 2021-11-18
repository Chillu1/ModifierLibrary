using System;

namespace ComboSystemComposition
{
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
}