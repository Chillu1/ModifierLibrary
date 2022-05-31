namespace ModifierSystem
{
    public class ApplierComponent : Component, IApplyComponent
    {
        private readonly ApplierEffectComponent _effectComponent;
        public ApplierComponent(ApplierEffectComponent applierEffectComponent)
        {
            _effectComponent = applierEffectComponent;
        }

        public void Apply()
        {
            _effectComponent.SimpleEffect();
        }
    }
}