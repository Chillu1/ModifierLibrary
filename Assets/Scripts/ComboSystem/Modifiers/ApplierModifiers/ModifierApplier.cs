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
            //TODO Condition
            Target!.AddModifier((Modifier)Data.Modifier.Clone());
        }

        public void ApplyModifierToTarget(Being target)
        {
            if(SetTarget(target))
            {
                //Reset target if applier if the target has been killed
                target.DeathEvent += obj => Target = null;
            }
            Log.Verbose("Applying "+Data.Modifier);
            Apply();
        }
    }
}