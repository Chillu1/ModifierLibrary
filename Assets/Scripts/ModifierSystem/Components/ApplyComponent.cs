using BaseProject;

namespace ModifierSystem
{
    /// <summary>
    ///     Component responsible for internal checks (if any), and conditions
    /// </summary>
    public class ApplyComponent : Component, IApplyComponent, ICleanUpComponent
    {
        public bool IsConditionEvent { get; }
        private BeingConditionEvent ConditionEvent { get; }

        private readonly IEffectComponent _effectComponent;
        private readonly ITargetComponent _targetComponent;
        //private readonly IValidatorComponent<object>[] _validatorComponents;

        public ApplyComponent(IEffectComponent effectComponent, ITargetComponent targetComponent)
        {
            _effectComponent = effectComponent;
            _targetComponent = targetComponent;
            Validate();
        }
        public ApplyComponent(IConditionEffectComponent effectComponent, ITargetComponent targetComponent,
            BeingConditionEvent conditionEvent = BeingConditionEvent.None)
        {
            IsConditionEvent = true;
            _effectComponent = (IEffectComponent)effectComponent;
            _targetComponent = targetComponent;
            ConditionEvent = conditionEvent;
            Validate();
        }

        public void Apply()
        {
            //for (int i = 0; i < _validatorComponents.Length; i++)
            //{
            //    _validatorComponents[i].Validate()
            //}
            //_targetComponent.Validate()

            if (ConditionEvent == BeingConditionEvent.None || !IsConditionEvent)//No conditions, just call it
            {
                _effectComponent.Effect();
                return;
            }

            switch (_effectComponent)
            {
                case IConditionEffectComponent conditionalEffect:
                    BeingEventHelper.SetupBeingEvent(_targetComponent.Target, ConditionEvent, conditionalEffect.Effect);
                    break;
                default:
                    Log.Error("Unrecognized effect component type: "+_effectComponent.GetType());
                    break;
            }
        }

        public void CleanUp()
        {
            if (ConditionEvent == BeingConditionEvent.None)
                return;

            switch (_effectComponent)
            {
                case IConditionEffectComponent effectComponent:
                    BeingEventHelper.RemoveBeingEvent(_targetComponent.Target, ConditionEvent, effectComponent.Effect);
                    break;
                default:
                    Log.Error("Unrecognized effect component type: "+_effectComponent.GetType());
                    break;
            }
        }

        private bool Validate()
        {
            bool success = true;
            bool conditionEvent = ConditionEvent != BeingConditionEvent.None,
                conditionTarget = _targetComponent.ConditionTarget != ConditionTarget.None;

            if ((!conditionEvent || !conditionTarget) && IsConditionEvent)
            {
                Log.Error("EffectComponent is conditionComponent, but ConditionTarget and ConditionEvent are None, illegal", "modifiers");
                success = false;
            }

            if (conditionEvent)
            {
                if (!conditionTarget)
                {
                    Log.Error("ConditionEvent is set, but ConditionTarget is None, illegal", "modifiers");
                    success = false;
                }
            }
            else if (conditionTarget)
            {
                Log.Error("ConditionTarget is set, but ConditionEvent is None, illegal", "modifiers");
                success = false;
            }

            return success;
        }
    }
}