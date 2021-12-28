namespace ModifierSystem
{
    public class ApplierComponent : EffectComponent//, IConditionalEffectComponent
    {
        private IModifier Modifier { get; }
        private AddModifierParameters Parameters { get; }
        private ITargetComponent TargetComponent { get; }

        public ApplierComponent(IModifier modifier, ITargetComponent targetComponent,
            AddModifierParameters parameters = AddModifierParameters.Default)
        {
            Modifier = modifier;
            Parameters = parameters;
            TargetComponent = targetComponent;
        }

        public override void Effect()
        {
            //Log.Info(TargetComponent.GetTarget().BaseBeing.Id);
            var clonedModifier = (Modifier)Modifier.Clone();
            clonedModifier.CopyEvents((Modifier)Modifier);
            TargetComponent.Target.AddModifier(clonedModifier, Parameters);
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

        public void ApplyModifierToTarget(Being target)
        {
            TargetComponent.SetTarget(target);
            //Log.Verbose("Applying "+Data.Modifier);
            //Apply();
        }
    }
}