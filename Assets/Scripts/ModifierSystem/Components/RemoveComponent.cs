using JetBrains.Annotations;

namespace ModifierSystem
{
    public class RemoveComponent : IEffectComponent
    {
        private readonly IModifier _modifier;
        [CanBeNull] private readonly CleanUpComponent _cleanUpComponent;

        public RemoveComponent(IModifier modifier, CleanUpComponent cleanUpComponent = null)
        {
            _modifier = modifier;
            _cleanUpComponent = cleanUpComponent;
        }

        public void Effect()
        {
            _cleanUpComponent?.CleanUp();
            _modifier.SetForRemoval();
        }

        //public void Effect(BaseBeing owner, BaseBeing acter) //Can't be, because BaseBeing and not Being
        //{
        //    _targetComponent.HandleTarget(owner, acter,
        //        (receiverLocal, acterLocal) => receiverLocal.);
        //}
    }
}