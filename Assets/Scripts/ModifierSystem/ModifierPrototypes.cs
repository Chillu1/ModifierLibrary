using JetBrains.Annotations;

namespace ModifierSystem
{
    public class ModifierPrototypes
    {
        private readonly ModifierPrototypesBase<Modifier> _modifierPrototypes;

        public ModifierPrototypes()
        {
            _modifierPrototypes = new ModifierPrototypesBase<Modifier>(true);
            SetupModifierPrototypes();
        }

        private void SetupModifierPrototypes()
        {
        }

        [CanBeNull]
        public Modifier GetItem(string key)
        {
            return _modifierPrototypes.Get(key);
        }
    }
}