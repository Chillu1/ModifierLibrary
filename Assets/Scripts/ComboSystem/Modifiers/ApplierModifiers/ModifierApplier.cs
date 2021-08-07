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

        protected override bool Apply()
        {
            if (!ApplyIsValid())
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