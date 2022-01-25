using BaseProject;

namespace ModifierSystem
{
    /// <summary>
    ///     Component responsible for internal checks (if any), and conditions
    /// </summary>
    public class ConditionalApplyComponent : Component, IConditionalApplyComponent, ICleanUpComponent
    {
        private ConditionEvent ConditionEvent { get; }
        private ConditionBeingStatus Status { get; }

        private readonly IConditionEffectComponent _conditionEffectComponent;
        private readonly ITargetComponent _targetComponent;
        //private readonly IValidatorComponent<object>[] _validatorComponents;

        public ConditionalApplyComponent(IConditionEffectComponent effectComponent, ITargetComponent targetComponent,
            ConditionEventData conditionEventData)
        {
            _conditionEffectComponent = effectComponent;
            _targetComponent = targetComponent;
            ConditionEvent = conditionEventData.conditionEvent;
            Validate();
        }

        public void Apply()
        {
            //for (int i = 0; i < _validatorComponents.Length; i++)
            //{
            //    _validatorComponents[i].Validate()
            //}
            //_targetComponent.Validate()

            ConditionEvent.SetupBeingEvent(_targetComponent.Target, _conditionEffectComponent.ConditionEffect);
        }

        public void CleanUp()
        {
            if (ConditionEvent == ConditionEvent.None)
                return;

            if (_conditionEffectComponent == null)
            {
                Log.Error("Condition effect component is null");
                return;
            }

            ConditionEvent.RemoveBeingEvent(_targetComponent.Target, _conditionEffectComponent.ConditionEffect);
        }

        private bool Validate()
        {
            bool success = true;
            bool conditionEvent = ConditionEvent != ConditionEvent.None,
                conditionTarget = _targetComponent.ConditionEventTarget != ConditionEventTarget.None;

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