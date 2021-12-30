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
        private readonly IConditionEffectComponent _conditionEffectComponent;
        private readonly ITargetComponent _targetComponent;
        //private readonly IValidatorComponent<object>[] _validatorComponents;

        public ApplyComponent(IEffectComponent effectComponent, ITargetComponent targetComponent)
        {
            _effectComponent = effectComponent;
            _targetComponent = targetComponent;
            Validate();
        }
        public ApplyComponent(IConditionEffectComponent effectComponent, ITargetComponent targetComponent,
            ConditionData conditionData = default)
        {
            IsConditionEvent = true;
            _conditionEffectComponent = effectComponent;
            _targetComponent = targetComponent;
            ConditionEvent = conditionData.BeingConditionEvent;
            Validate();
        }
        /// <summary>
        ///     Special constructor for one-off/one-time use modifiers
        /// </summary>
        public ApplyComponent(RemoveComponent effectComponent, ITargetComponent targetComponent,
            ConditionData conditionData = default)
        {
            IsConditionEvent = true;
            _effectComponent = effectComponent;
            _targetComponent = targetComponent;
            ConditionEvent = conditionData.BeingConditionEvent;
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

            if(_effectComponent != null)
            {
                //TODO Temp solution to remove component not having the proper signature, we can remove the anonymous delegate, somehow
                ConditionEvent.SetupBeingEvent(_targetComponent.Target, delegate { _effectComponent.Effect(); });
            }
            else if (_conditionEffectComponent != null)
                ConditionEvent.SetupBeingEvent(_targetComponent.Target, _conditionEffectComponent.Effect);
            else
                Log.Error("Both effect components are null");
        }

        public void CleanUp()
        {
            if (ConditionEvent == BeingConditionEvent.None || !IsConditionEvent)
                return;

            if(_effectComponent != null)
            {
                //TODO Temp solution?
                ConditionEvent.RemoveBeingEvent(_targetComponent.Target, delegate { _effectComponent.Effect(); });
            }
            else if (_conditionEffectComponent != null)
                ConditionEvent.RemoveBeingEvent(_targetComponent.Target, _conditionEffectComponent.Effect);
            else
                Log.Error("Both effect components are null");
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