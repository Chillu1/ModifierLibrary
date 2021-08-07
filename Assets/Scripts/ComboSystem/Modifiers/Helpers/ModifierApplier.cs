namespace ComboSystem
{
    /// <summary>
    ///     Applies modifiers on others (Target). Shouldn't be used to apply to owner
    /// </summary>
    public class ModifierApplier<TModifierApplierData> : Modifier<TModifierApplierData> where TModifierApplierData : ModifierApplierData
    {
        public ModifierApplier(string id, TModifierApplierData poisonModifierBuffData, ModifierProperties properties = default) : base(id, properties)
        {
            Data = poisonModifierBuffData;
        }

        protected override bool Apply()
        {
            if (!base.Apply())
                return false;
            Target!.AddModifier(Data.Modifier);
            return true;
        }

        public void ApplyModifierToTarget(Character target)
        {
            SetTarget(target);
            Apply();
        }
    }
}