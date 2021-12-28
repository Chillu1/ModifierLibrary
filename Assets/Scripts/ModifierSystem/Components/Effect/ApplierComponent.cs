namespace ModifierSystem
{
    public class ApplierComponent : EffectComponent
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
            //Log.Info(TargetComponent);
            //Log.Info(TargetComponent.GetTarget());
            //Log.Info(Modifier);
            //Log.Info(TargetComponent.GetTarget().BaseBeing.Id);
            var clonedModifier = (Modifier)Modifier.Clone();
            clonedModifier.CopyEvents((Modifier)Modifier);
            TargetComponent.Target.AddModifier(clonedModifier, Parameters);
        }

        public void ApplyModifierToTarget(Being target)
        {
            TargetComponent.SetTarget(target);
            //Log.Verbose("Applying "+Data.Modifier);
            //Apply();
        }
    }
}