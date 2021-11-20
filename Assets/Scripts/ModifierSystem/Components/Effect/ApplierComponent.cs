namespace ModifierSystem
{
    public class ApplierComponent : IEffectComponent
    {
        private IModifier Modifier { get; }
        private AddModifierParameters Parameters { get; }
        private ITargetComponent TargetComponent { get; }


        public ApplierComponent(Modifier modifier, ITargetComponent targetComponent,
            AddModifierParameters parameters = AddModifierParameters.Default)
        {
            Modifier = modifier;
            Parameters = parameters;
            TargetComponent = targetComponent;
        }

        public void Effect()
        {
            //Log.Info(TargetComponent);
            //Log.Info(TargetComponent.GetTarget());
            //Log.Info(Modifier);
            //Log.Info(TargetComponent.GetTarget().BaseBeing.Id);
            TargetComponent.GetTarget().AddModifier((Modifier)Modifier.Clone(), Parameters);
        }

        public void ApplyModifierToTarget(Being target)
        {
            TargetComponent.SetTarget(target);
            //Log.Verbose("Applying "+Data.Modifier);
            //Apply();
        }
    }
}