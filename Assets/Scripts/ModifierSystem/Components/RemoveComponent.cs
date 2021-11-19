namespace ModifierSystem
{
    public class RemoveComponent : IEffectComponent
    {
        private ModifierController _modifierController;//TODO TEMP
        private IModifier _modifier;

        public RemoveComponent(IModifier modifier)
        {
            _modifier = modifier;
        }

        public void Effect()
        {
            //_modifierController.RemoveModifier(_modifier);
        }

        public void CleanUp()
        {
        }
    }
}