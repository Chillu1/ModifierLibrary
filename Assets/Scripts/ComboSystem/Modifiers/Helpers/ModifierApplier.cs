namespace ComboSystem
{
    public class ModifierApplier<TModifierApplierData> : Modifier<TModifierApplierData> where TModifierApplierData : ModifierApplierData
    {
        public ModifierApplier(TModifierApplierData poisonModifierBuffData)
        {
            Data = poisonModifierBuffData;
        }

        protected override void Apply()
        {
            Target.AddModifier(Data.Modifier);
        }
    }
}