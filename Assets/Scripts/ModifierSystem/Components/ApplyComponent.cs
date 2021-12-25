namespace ModifierSystem
{
    /// <summary>
    ///     Component responsible for internal checks (if any)
    /// </summary>
    public class ApplyComponent : Component, IApplyComponent
    {
        public IEffectComponent EffectComponent { get; }
        private readonly TargetComponent _targetComponent;
        //[CanBeNull] private readonly Func<object, bool> _conditionCheck;//TODO object
        private readonly IValidatorComponent<object>[] _validatorComponents;

        public ApplyComponent(IEffectComponent effectComponent, TargetComponent targetComponent) //params IValidatorComponent<object>[] validatorComponents)
        {
            EffectComponent = effectComponent;
            _targetComponent = targetComponent;
            //_conditionCheck = applyCheck;
            //_validatorComponents = validatorComponents;
        }

        public void Apply()
        {
            //for (int i = 0; i < _validatorComponents.Length; i++)
            //{
            //    _validatorComponents[i].Validate()
            //}
            //_targetComponent.Validate()

            //if (_conditionCheck?.Invoke(this) == false)
            //    return;
            
            EffectComponent.Effect();
        }
    }
}