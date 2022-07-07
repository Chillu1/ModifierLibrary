using BaseProject;
using JetBrains.Annotations;

namespace ModifierSystem
{
    /// <summary>
    ///     Component responsible for internal checks (if any), and conditions
    /// </summary>
    public class ConditionalApplyComponent : Component, IConditionalApplyComponent, ICleanUpComponent
    {
        private readonly ConditionEvent _conditionEvent;
        private readonly ConditionUnitStatus _status;

        private IConditionEffectComponent ConditionEffectComponent { get; }
        private ITargetComponent TargetComponent { get; }
        [CanBeNull] private ICheckComponent CheckComponent { get; }

        public ConditionalApplyComponent(IConditionEffectComponent effectComponent, ITargetComponent targetComponent,
            ConditionEvent conditionEvent, ICheckComponent checkComponent = null)
        {
            ConditionEffectComponent = effectComponent;
            TargetComponent = targetComponent;
            CheckComponent = checkComponent;
            _conditionEvent = conditionEvent;
            Validate();
        }

        public void Apply()
        {
            _conditionEvent.SetupUnitEvent(TargetComponent.Target, ConditionEffectCheck);
        }

        public void CleanUp()
        {
            if (_conditionEvent == ConditionEvent.None)
                return;

            if (ConditionEffectComponent == null)
            {
                Log.Error("Condition effect component is null");
                return;
            }

            _conditionEvent.RemoveUnitEvent(TargetComponent.Target, ConditionEffectCheck);
        }

        private bool Validate()
        {
            bool success = true;
            bool conditionEvent = _conditionEvent != ConditionEvent.None,
                conditionTarget = TargetComponent.ConditionEventTarget != ConditionEventTarget.None;

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

        private void ConditionEffectCheck(BaseProject.Unit receiver, BaseProject.Unit acter)
        {
            if (CheckComponent == null || CheckComponent.Check())
            {
                ConditionEffectComponent.ConditionEffect((Unit)receiver, (Unit)acter);
                CheckComponent?.Apply();
            }
        }
    }
}