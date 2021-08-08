namespace ComboSystem
{
    public class StatChangeDurationComboModifier : SingleUseDurationComboModifier<StatChangeDurationModifierData>
    {
        public StatChangeDurationComboModifier(string id, StatChangeDurationModifierData data, ComboRecipe recipe, ModifierProperties properties = default) :
            base(id, data, recipe, properties)
        {
        }

        protected override bool Apply()
        {
            if (!ApplyIsValid())
                return false;
            Target!.ChangeStat(Data.StatType, Data.Value);
            return true;
        }

        protected override void Remove()
        {
            Target.ChangeStat(Data.StatType, Data.Value);
            base.Remove();
        }
    }
}