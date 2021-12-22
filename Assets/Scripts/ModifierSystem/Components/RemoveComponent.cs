namespace ModifierSystem
{
    public class RemoveComponent : IEffectComponent
    {
        private ModifierController _modifierController;//TODO Not sure about this implementation, can't use delegates either
        private IModifier _modifier;

        public RemoveComponent(IModifier modifier)
        {
            _modifier = modifier;
        }

        public void Init(ModifierController modifierController)
        {
            _modifierController = modifierController;
        }

        public void Effect()
        {
            _modifierController.RemoveModifier(_modifier);
        }
    }
}