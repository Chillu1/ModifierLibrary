namespace ComboSystemComposition
{
    public class ApplierComponent : IEffectComponent
    {
        private IModifier Modifier { get; }
        private ITargetComponent TargetComponent { get; }

        public ApplierComponent(Modifier modifier, ITargetComponent targetComponent)
        {
            Modifier = modifier;
            TargetComponent = targetComponent;
        }

        public void Effect()
        {
            TargetComponent.GetTarget().AddModifier((Modifier)Modifier.Clone());
        }

        public void ApplyModifierToTarget(Being target)
        {
            TargetComponent.SetTarget(target);
            //Log.Verbose("Applying "+Data.Modifier);
            //Apply();
        }
    }
}