using JetBrains.Annotations;

namespace ModifierSystem
{
    public class ModifierPrototypes
    {
        private readonly ModifierPrototypesBase<IModifier> _modifierPrototypes;

        public ModifierPrototypes()
        {
            _modifierPrototypes = new ModifierPrototypesBase<IModifier>();
            SetupModifierPrototypes();
        }

        private void SetupModifierPrototypes()
        {
        }

        [CanBeNull]
        public IModifier GetItem(string key)
        {
            return _modifierPrototypes.GetItem(key);
        }
    }
}