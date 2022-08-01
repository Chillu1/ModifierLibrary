namespace ModifierLibrary
{
    public class RefreshComponent : IRefreshComponent
    {
        private IRefreshEffectComponent RefreshEffectComponent { get; }
        private RefreshEffectType RefreshEffect { get; }

        public RefreshComponent(IRefreshEffectComponent refreshEffectComponent, RefreshEffectType refreshEffect)
        {
            RefreshEffectComponent = refreshEffectComponent;
            RefreshEffect = refreshEffect;
        }

        public void Refresh()
        {
            RefreshEffectComponent.RefreshEffect(RefreshEffect);
        }
    }

    public enum RefreshEffectType
    {
        None = 0,
        RefreshDuration = 1,
        //Effect = 2,//Useless?
    }
}