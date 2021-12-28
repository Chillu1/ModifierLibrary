using JetBrains.Annotations;

namespace ModifierSystem
{
    public class CleanUpComponent : Component, ICleanUpComponent
    {
        [CanBeNull]
        private ApplyComponent _applyComponent;

        public CleanUpComponent([CanBeNull] ApplyComponent applyComponent = null)
        {
            _applyComponent = applyComponent;
        }

        public void AddComponent(ApplyComponent component)
        {
            _applyComponent = component;
        }

        public void CleanUp()
        {
            _applyComponent?.CleanUp();
        }
    }
}