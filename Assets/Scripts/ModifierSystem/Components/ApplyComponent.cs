using BaseProject;

namespace ModifierSystem
{
    /// <summary>
    ///     Component responsible for internal checks (if any), and conditions
    /// </summary>
    public class ApplyComponent : Component, IApplyComponent, ICleanUpComponent
    {
        private readonly EffectComponent _effectComponent;
        private readonly ITargetComponent _targetComponent;
        //private readonly IValidatorComponent<object>[] _validatorComponents;
        private readonly BeingConditionEvent _conditionEvent;

        public ApplyComponent(EffectComponent effectComponent, ITargetComponent targetComponent)
        {
            _effectComponent = effectComponent;
            _targetComponent = targetComponent;
            // TODO Validate
        }
        public ApplyComponent(IConditionalEffectComponent effectComponent, ITargetComponent targetComponent,
            BeingConditionEvent conditionEvent = BeingConditionEvent.None)
        {
            _effectComponent = (EffectComponent)effectComponent;
            _targetComponent = targetComponent;
            _conditionEvent = conditionEvent;
            // TODO Validate IConditionalEffectComponent
        }

        public void Apply()
        {
            //for (int i = 0; i < _validatorComponents.Length; i++)
            //{
            //    _validatorComponents[i].Validate()
            //}
            //_targetComponent.Validate()

            if (_conditionEvent == BeingConditionEvent.None)
            {
                _effectComponent.Effect();
                return;
            }

            switch (_effectComponent)
            {
                case IConditionalEffectComponent conditionalEffect:
                    BeingEventHelper.SetupBeingEvent(_targetComponent.Target, _conditionEvent, conditionalEffect.Effect);
                    break;
                default:
                    Log.Error("Unrecognized effect component type: "+_effectComponent.GetType());
                    break;
            }
        }

        public void CleanUp()
        {
            if (_conditionEvent == BeingConditionEvent.None)
                return;

            switch (_effectComponent)
            {
                case IConditionalEffectComponent effectComponent:
                    BeingEventHelper.RemoveBeingEvent(_targetComponent.Target, _conditionEvent, effectComponent.Effect);
                    break;
                default:
                    Log.Error("Unrecognized effect component type: "+_effectComponent.GetType());
                    break;
            }
        }

        private bool Validate()
        {
            return true;
        }
    }
}