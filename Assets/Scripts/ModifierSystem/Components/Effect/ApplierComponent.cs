using BaseProject;

namespace ModifierSystem
{
    public class ApplierComponent : IEffectComponent, IConditionEffectComponent, IStackEffectComponent
    {
        private IModifier Modifier { get; }
        private AddModifierParameters Parameters { get; }
        public bool StackEffect { get; }
        private readonly ITargetComponent _targetComponent;

        public ApplierComponent(IModifier modifier, ITargetComponent targetComponent,
            AddModifierParameters parameters = AddModifierParameters.Default, bool stackEffect = false)
        {
            Modifier = modifier;
            Parameters = parameters;
            _targetComponent = targetComponent;
            StackEffect = stackEffect;
        }

        public void Effect()
        {
            //Log.Info(TargetComponent.GetTarget().BaseBeing.Id);
            var clonedModifier = (IModifier)Modifier.Clone();
            clonedModifier.CopyEvents(Modifier);
            _targetComponent.Target.AddModifier(clonedModifier, Parameters);
        }

        public void Effect(BaseBeing owner, BaseBeing acter)
        {
            _targetComponent.HandleTarget(owner, acter,
                (receiverLocal, acterLocal) =>
                {
                    var clonedModifier = (IModifier)Modifier.Clone();
                    clonedModifier.CopyEvents(Modifier);
                    ((Being)receiverLocal).AddModifier(clonedModifier, Parameters);
                });
        }

        public void Effect(int stacks, double value)
        {
            if (StackEffect)
                Effect();
        }
    }
}