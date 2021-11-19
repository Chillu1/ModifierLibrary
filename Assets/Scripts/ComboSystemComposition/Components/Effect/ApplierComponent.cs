namespace ComboSystemComposition
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