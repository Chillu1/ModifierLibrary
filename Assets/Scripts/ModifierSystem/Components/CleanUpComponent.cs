using JetBrains.Annotations;

namespace ModifierSystem
{
    /// <summary>
    ///     Responsible for cleaning up (removing) being event
    /// </summary>
    public class CleanUpComponent : Component, ICleanUpComponent
    {
        [CanBeNull]
        private ConditionalApplyComponent _applyComponent;

        public CleanUpComponent([CanBeNull] ConditionalApplyComponent applyComponent = null)
        {
            _applyComponent = applyComponent;
        }

        public void AddComponent(ConditionalApplyComponent component)
        {
            _applyComponent = component;
        }

        public void CleanUp()
        {
            _applyComponent?.CleanUp();
        }
    }
}