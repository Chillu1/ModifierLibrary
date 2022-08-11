using System;
using UnitLibrary.Utils;
using Force.DeepCloner;
using JetBrains.Annotations;

namespace ModifierLibrary
{
	public class ComboRecipes : ICloneable
	{
		/// <summary>
		///     Possible recipes
		/// </summary>
		[NotNull]
		public readonly ComboRecipe[] Recipes;

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
			return this.DeepClone();
		}
	}
}