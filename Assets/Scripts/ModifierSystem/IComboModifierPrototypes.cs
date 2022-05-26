using System.Collections.Generic;

namespace ModifierSystem
{
    public interface IComboModifierPrototypes
    {
        Dictionary<string, ComboModifier>.ValueCollection Values { get; }
        ComboModifier Get(string id);
    }
}