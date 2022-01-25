using System.Collections.Generic;
using BaseProject;

namespace ModifierSystem
{
    public interface IComboModifier : IModifier, IEventCopy<IComboModifier>
    {
        float Cooldown { get; }
        bool CheckRecipes(HashSet<string> modifierIds, ElementController elementController, Stats stats);
    }
}