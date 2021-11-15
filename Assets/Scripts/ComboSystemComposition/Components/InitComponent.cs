using System;
using JetBrains.Annotations;

namespace ComboSystemComposition
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