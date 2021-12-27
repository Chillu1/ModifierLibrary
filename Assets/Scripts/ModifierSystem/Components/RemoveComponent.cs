namespace ModifierSystem
{
    public class RemoveComponent : IEffectComponent
    {
        private IModifier _modifier;

        public RemoveComponent(IModifier modifier)
        {
            _modifier = modifier;
        }

        public void Effect()
        {
            _modifier.SetForRemoval();
        }
    }
}