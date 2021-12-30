namespace ModifierSystem
{
    public class ApplierComponent : IEffectComponent, IStackEffectComponent//, IConditionalEffectComponent
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
            var clonedModifier = (Modifier)Modifier.Clone();
            clonedModifier.CopyEvents((Modifier)Modifier);
            _targetComponent.Target.AddModifier(clonedModifier, Parameters);
        }

        //TODO Should appliers be allowed for conditional? Most probably yes, like applying poison modifier on getting hit
        //public void Effect(BaseBeing owner, BaseBeing acter)
        //{
        //    TargetComponent.HandleTarget(owner, acter,
        //        delegate(BaseBeing receiverLocal, BaseBeing acterLocal)
        //        {
        //            var clonedModifier = (Modifier)Modifier.Clone();
        //            clonedModifier.CopyEvents((Modifier)Modifier);
        //            receiverLocal.AddModifier(clonedModifier, Parameters);//Problem is, BaseBeing has all the events
        //        });
        //}

        public void Effect(int stacks, double value)
        {
            if (StackEffect)
                Effect();
        }

        public void ApplyModifierToTarget(Being target)
        {
            _targetComponent.SetTarget(target);
            //Log.Verbose("Applying "+Data.Modifier);
            //Apply();
        }
    }
}