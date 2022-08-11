using System.Collections.Generic;

namespace ModifierLibrary
{
	public interface IComboModifierPrototypes
	{
		Dictionary<string, ComboModifier>.ValueCollection Values { get; }
		ComboModifier Get(string id);
	}
}