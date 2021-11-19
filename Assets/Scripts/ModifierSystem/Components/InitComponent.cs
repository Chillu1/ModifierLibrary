using JetBrains.Annotations;

namespace ModifierSystem
{
    public class InitComponent : Component, IInitComponent
    {
        [CanBeNull]
        private readonly IApplyComponent _applyComponent;

        public InitComponent(IApplyComponent applyComponent = null)
        {
            _applyComponent = applyComponent;
        }

        public void Init()
        {
            _applyComponent?.Apply();
        }
    }
}