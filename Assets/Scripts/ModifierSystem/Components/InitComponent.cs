namespace ModifierSystem
{
    public class InitComponent : Component, IInitComponent
    {
        private readonly IApplyComponent _applyComponent;

        public InitComponent(IApplyComponent applyComponent)
        {
            _applyComponent = applyComponent;
        }

        public void Init()
        {
            _applyComponent.Apply();
        }
        public bool EffectComponentIsOfType<T>() where T : IEffectComponent
        {
            return _applyComponent.GetType() == typeof(T);
        }
    }
}