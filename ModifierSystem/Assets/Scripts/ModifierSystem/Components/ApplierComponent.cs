namespace ModifierSystem
{
    public class ApplierComponent : Component, IApplyComponent
    {
        private ICheckComponent CheckComponent { get; }

        public ApplierComponent(ICheckComponent checkComponent)
        {
            CheckComponent = checkComponent;
        }

        public void Apply()
        {
            CheckComponent.Effect();
        }
    }
}