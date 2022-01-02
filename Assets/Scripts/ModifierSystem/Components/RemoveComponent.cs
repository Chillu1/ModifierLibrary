using BaseProject;
using JetBrains.Annotations;

namespace ModifierSystem
{
    public class RemoveComponent : IEffectComponent, IConditionEffectComponent
    {
        private readonly IModifier _modifier;
        [CanBeNull] private readonly CleanUpComponent _cleanUpComponent;

        public RemoveComponent(IModifier modifier, CleanUpComponent cleanUpComponent = null)
        {
            _modifier = modifier;
            _cleanUpComponent = cleanUpComponent;
        }

        public void SimpleEffect()
        {
            _cleanUpComponent?.CleanUp();
            _modifier.SetForRemoval();
        }

        public void ConditionEffect(BaseBeing receiver, BaseBeing acter)//Can't make it proper, because BaseBeing and not Being
        {
            SimpleEffect();
        }
    }
}