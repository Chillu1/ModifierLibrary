using BaseProject;
using JetBrains.Annotations;

namespace ModifierSystem
{
    /// <summary>
    ///     Component responsible for internal checks (if any), and conditions
    /// </summary>
    public class ApplyComponent : Component, IApplyComponent, ICleanUpComponent
    {
        public bool IsConditionEvent { get; }
        private ConditionEvent ConditionEvent { get; }
        private ConditionBeingStatus Status { get; }

        private readonly IEffectComponent _effectComponent;
        private readonly IConditionEffectComponent _conditionEffectComponent;
        [CanBeNull] private readonly ITargetComponent _targetComponent;
        //private readonly IValidatorComponent<object>[] _validatorComponents;

        /// <summary>
        ///     Special constructor for Appliers
        /// </summary>
        public ApplyComponent(ApplierComponent applierComponent)
        {
            _effectComponent = applierComponent;
            Validate();
        }
        public ApplyComponent(IConditionEffectComponent effectComponent, ITargetComponent targetComponent,
            ConditionEventData conditionEventData = default)
        {
            IsConditionEvent = true;
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

            if (ConditionEvent == ConditionEvent.None || !IsConditionEvent)//No conditions, just call it
            {
                _effectComponent.SimpleEffect();
                return;
            }

            if (_conditionEffectComponent == null)
            {
                Log.Error("Condition effect component is null");
                return;
            }

            ConditionEvent.SetupBeingEvent(_targetComponent!.Target, _conditionEffectComponent.ConditionEffect);
        }

        public void CleanUp()
        {
            if (ConditionEvent == ConditionEvent.None || !IsConditionEvent)
                return;

            if (_conditionEffectComponent == null)
            {
                Log.Error("Condition effect component is null");
                return;
            }

            ConditionEvent.RemoveBeingEvent(_targetComponent!.Target, _conditionEffectComponent.ConditionEffect);
        }

        private bool Validate()
        {
            bool success = true;
            bool conditionEvent = ConditionEvent != ConditionEvent.None,
                conditionTarget = _targetComponent != null && _targetComponent.ConditionEventTarget != ConditionEventTarget.None;

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