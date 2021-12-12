using System;
using BaseProject.Utils;
using JetBrains.Annotations;

namespace ModifierSystem
{
    public class ComboRecipes : ICloneable
    {
        /// <summary>
        ///     Possible recipes
        /// </summary>
        [NotNull] public ComboRecipe[] Recipes;

        public ComboRecipes([NotNull] ComboRecipe recipe)
        {
            Recipes = new[] { recipe };
        }

        public ComboRecipes([NotNull] ComboRecipe[] recipes)
        {
            Recipes = recipes;
        }

        public object Clone()
        {
            return this.Copy();
        }
    }
}