using JetBrains.Annotations;

namespace ModifierSystem
{
    public class ModifierPrototypes
    {
        private readonly ModifierPrototypesBase _modifierPrototypes;

        public ModifierPrototypes()
        {
            _modifierPrototypes = new ModifierPrototypesBase();
            SetupModifierPrototypes();
        }

        private void SetupModifierPrototypes()
        {
        }

        [CanBeNull]
        public IModifier GetItem(string key)
        {
            return _modifierPrototypes.Get(key);
        }
    }
}