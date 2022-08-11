using UnitLibrary;
using JetBrains.Annotations;

namespace ModifierLibrary
{
	/// <summary>
	///     Recipe (condition) for a ComboModifier to be added, possible conditions: specific modifiers (ID), ElementalData or Stats
	/// </summary>
	public class ComboRecipe
	{
		[CanBeNull]
		public readonly string[] Id;

		[CanBeNull]
		public readonly ElementalRecipe[] ElementalRecipe;

		[CanBeNull]
		public readonly Stat[] Stat;

		public ComboRecipe()
		{
		}

		public ComboRecipe(string[] id)
		{
			Id = id;
		}

		public ComboRecipe(ElementalRecipe[] elementalRecipe)
		{
			ElementalRecipe = elementalRecipe;
		}

		public ComboRecipe(Stat[] stat)
		{
			Stat = stat;
		}

		public ComboRecipe(string[] id, ElementalRecipe[] elementalRecipe)
		{
			Id = id;
			ElementalRecipe = elementalRecipe;
		}
	}
}