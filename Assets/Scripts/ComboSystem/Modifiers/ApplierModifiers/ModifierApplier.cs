using BaseProject;

namespace ComboSystem
{
    /// <summary>
    ///     Applies modifiers on others (Target). Shouldn't be used to apply to owner
    /// </summary>
    public class ModifierApplier<TModifierApplierData> : Modifier<TModifierApplierData> where TModifierApplierData : ModifierApplierData
    {
        public ModifierApplier(string id, TModifierApplierData data, ModifierProperties properties = default) : base(id, data, properties)
        {
        }

        protected override void Effect()
        {
            Target!.AddModifier(Data.Modifier);
        }

        public void ApplyModifierToTarget(Being target)
        {
            SetTarget(target);
            Log.Verbose("Applying "+Data.Modifier);
            Apply();
        }
    }
}